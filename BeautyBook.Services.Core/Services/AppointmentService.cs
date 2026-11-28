using BeautyBook.Data.Models;
using BeautyBook.Data.Models.Models;
using BeautyBook.Data.Repositories.Contracts;
using BeautyBook.Services.Core.Contracts;
using System.Globalization;
using BeautyBook.Services.Core.Models;

namespace BeautyBook.Services.Core.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository appointmentRepository;
        private readonly IServiceRepository serviceRepository;
        private readonly IEmployeeRepository employeeRepository;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IServiceRepository serviceRepository,
            IEmployeeRepository employeeRepository)
        {
            this.appointmentRepository = appointmentRepository;
            this.serviceRepository = serviceRepository;
            this.employeeRepository = employeeRepository;
        }

        public async Task<bool> IsAvailableAsync(
            DateTime date,
            TimeSpan startTime,
            int serviceId,
            int employeeId)
        {
            var service = await serviceRepository.GetByIdReadOnlyAsync(serviceId);

            if (service == null)
            {
                return false;
            }

            var appointmentStart = date.Date.Add(startTime);
            var appointmentEnd = appointmentStart.AddMinutes(service.DurationMinutes);

            var hasConflict = await appointmentRepository
                .HasConflictAsync(appointmentStart, appointmentEnd, employeeId);

            return !hasConflict;
        }

        public async Task<IEnumerable<Appointment>> GetUserAppointmentsAsync(string userId)
        {
            return await appointmentRepository.GetByUserAsync(userId);
        }

        public async Task<Appointment?> GetByIdAsync(int id, string userId)
        {
            return await appointmentRepository.GetByIdForUserAsync(id, userId);
        }

        public async Task<bool> CreateAsync(
            int serviceId,
            int employeeId,
            DateTime date,
            string startTimeStr,
            string? notes,
            string userId)
        {
            if (!TryParseTime(startTimeStr, out var selectedTime))
            {
                return false;
            }

            var service = await serviceRepository.GetByIdReadOnlyAsync(serviceId);

            if (service == null) return false;

            if (!await CanPerformServiceAsync(employeeId, service)) return false;

            var startTime = date.Date.Add(selectedTime);
            var endTime = startTime.AddMinutes(service.DurationMinutes);

            if (!IsValidAppointmentTime(startTime, endTime, selectedTime))
            {
                return false;
            }

            var isAvailable = await IsAvailableAsync(
                date.Date, selectedTime, service.Id, employeeId);

            if (!isAvailable) return false;

            var appointment = new Appointment
            {
                StartTime = startTime,
                EndTime = endTime,
                Notes = notes,
                Status = "Pending",
                UserId = userId,
                ServiceId = service.Id,
                EmployeeId = employeeId
            };

            await appointmentRepository.AddAsync(appointment);
            await appointmentRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditAsync(
            int id,
            int serviceId,
            int employeeId,
            DateTime date,
            string startTimeStr,
            string? notes,
            string userId)
        {
            var appointment = await appointmentRepository
                .GetTrackedByIdForUserAsync(id, userId);

            if (appointment == null) return false;

            if (!TryParseTime(startTimeStr, out var selectedTime))
            {
                return false;
            }

            var service = await serviceRepository.GetByIdReadOnlyAsync(serviceId);

            if (service == null) return false;

            if (!await CanPerformServiceAsync(employeeId, service)) return false;

            var startTime = date.Date.Add(selectedTime);
            var endTime = startTime.AddMinutes(service.DurationMinutes);

            if (!IsValidAppointmentTime(startTime, endTime, selectedTime))
            {
                return false;
            }

            var hasConflict = await appointmentRepository
                .HasConflictAsync(startTime, endTime, employeeId, id);

            if (hasConflict) return false;

            appointment.ServiceId = service.Id;
            appointment.EmployeeId = employeeId;
            appointment.StartTime = startTime;
            appointment.EndTime = endTime;
            appointment.Notes = notes;
            appointment.Status = "Pending";

            await appointmentRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id, string userId)
        {
            var appointment = await appointmentRepository
                .GetTrackedByIdForUserAsync(id, userId);

            if (appointment == null) return false;

            appointmentRepository.Delete(appointment);
            await appointmentRepository.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Service>> GetAllServicesAsync()
        {
            return await serviceRepository.GetAllOrderedByNameAsync();
        }

        public async Task<List<string>> GetAvailableSlotsAsync(
            DateTime date,
            int serviceId,
            int employeeId)
        {
            if (date.Date < DateTime.Today || date.DayOfWeek == DayOfWeek.Sunday)
            {
                return new List<string>();
            }

            var service = await serviceRepository.GetByIdReadOnlyAsync(serviceId);

            if (service == null) return new List<string>();

            if (!await CanPerformServiceAsync(employeeId, service))
            {
                return new List<string>();
            }

            var appointments = await appointmentRepository
                .GetByDateAsync(date, employeeId);

            var slots = new List<string>();
            var openingTime = new TimeSpan(9, 0, 0);
            var closingTime = new TimeSpan(18, 0, 0);

            for (var time = openingTime;
                 time + TimeSpan.FromMinutes(service.DurationMinutes) <= closingTime;
                 time = time.Add(TimeSpan.FromMinutes(15)))
            {
                var start = date.Date.Add(time);
                var end = start.AddMinutes(service.DurationMinutes);

                if (start <= DateTime.Now) continue;

                var hasConflict = appointments.Any(a => a.StartTime < end && a.EndTime > start);

                if (!hasConflict)
                {
                    slots.Add(start.ToString("HH:mm"));
                }
            }

            return slots;
        }

        public async Task<IEnumerable<Appointment>> GetAllForAdminAsync()
        {
            return await appointmentRepository.GetAllWithDetailsAsync();
        }

        public async Task<PagedResult<Appointment>> GetPageForAdminAsync(string? status,int page,int pageSize)
        {
            var totalItems = await appointmentRepository.CountForAdminAsync(status);
            page = PagedResult<Appointment>.ClampPage(page, totalItems, pageSize);

            var items = await appointmentRepository.GetPageForAdminAsync(
                status,
                (page - 1) * pageSize,
                pageSize);

            return new PagedResult<Appointment>(items, page, pageSize, totalItems);
        }

        public async Task<bool> SetStatusAsync(int id, string status)
        {
            var appointment = await appointmentRepository.GetTrackedByIdAsync(id);
            if (appointment == null) return false;

            appointment.Status = status;
            await appointmentRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsAdminAsync(int id)
        {
            var appointment = await appointmentRepository.GetTrackedByIdAsync(id);
            if (appointment == null) return false;

            appointmentRepository.Delete(appointment);
            await appointmentRepository.SaveChangesAsync();
            return true;
        }

        // The employee must exist and work in the service's category.
        private async Task<bool> CanPerformServiceAsync(int employeeId, Service service)
        {
            var employee = await employeeRepository.GetByIdReadOnlyAsync(employeeId);

            return employee != null && employee.CategoryId == service.CategoryId;
        }

        private bool TryParseTime(string? timeString, out TimeSpan selectedTime)
        {
            selectedTime = TimeSpan.Zero;
            if (string.IsNullOrWhiteSpace(timeString)) return false;

            return TimeSpan.TryParseExact(timeString, @"hh\:mm", CultureInfo.InvariantCulture, out selectedTime);
        }

        private bool IsValidAppointmentTime(DateTime startTime, DateTime endTime, TimeSpan selectedTime)
        {
            if (startTime <= DateTime.Now) return false;
            if (startTime.DayOfWeek == DayOfWeek.Sunday) return false;
            if (selectedTime.TotalMinutes % 15 != 0) return false;
            if (startTime.TimeOfDay < new TimeSpan(9, 0, 0) || endTime.TimeOfDay > new TimeSpan(18, 0, 0)) return false;

            return true;
        }
    }
}