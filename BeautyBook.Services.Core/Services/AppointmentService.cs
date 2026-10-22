using BeautyBook.Data;
using BeautyBook.Data.Models;
using BeautyBook.Services.Core.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace BeautyBook.Services.Core.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext context;

        public AppointmentService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> IsAvailableAsync(
            DateTime date,
            TimeSpan startTime,
            int serviceId)
        {
            var service = await context.Services
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == serviceId);

            if (service == null)
            {
                return false;
            }

            var appointmentStart = date.Date.Add(startTime);
            var appointmentEnd = appointmentStart.AddMinutes(service.DurationMinutes);

            var hasConflict = await context.Appointments
                .AnyAsync(a =>
                    a.StartTime < appointmentEnd &&
                    a.EndTime > appointmentStart);

            return !hasConflict;
        }

        public async Task<IEnumerable<Appointment>> GetUserAppointmentsAsync(string userId)
        {
            return await context.Appointments
                .Include(a => a.Service)
                .Where(a => a.UserId == userId)
                .AsNoTracking()
                .OrderBy(a => a.StartTime)
                .ToListAsync();
        }

        public async Task<Appointment?> GetByIdAsync(int id, string userId)
        {
            return await context.Appointments
                .Include(a => a.Service)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);
        }

        public async Task<bool> CreateAsync(int serviceId, DateTime date, string startTimeStr, string? notes, string userId)
        {
            if (!TryParseTime(startTimeStr, out var selectedTime))
            {
                return false;
            }

            var service = await context.Services
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == serviceId);

            if (service == null) return false;

            var startTime = date.Date.Add(selectedTime);
            var endTime = startTime.AddMinutes(service.DurationMinutes);

            if (!IsValidAppointmentTime(startTime, endTime, selectedTime))
            {
                return false;
            }

            var isAvailable = await IsAvailableAsync(date.Date, selectedTime, service.Id);
            if (!isAvailable) return false;

            var appointment = new Appointment
            {
                StartTime = startTime,
                EndTime = endTime,
                Notes = notes,
                Status = "Pending",
                UserId = userId,
                ServiceId = service.Id
            };

            context.Appointments.Add(appointment);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditAsync(int id, int serviceId, DateTime date, string startTimeStr, string? notes, string userId)
        {
            var appointment = await context.Appointments
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

            if (appointment == null) return false;

            if (!TryParseTime(startTimeStr, out var selectedTime))
            {
                return false;
            }

            var service = await context.Services
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == serviceId);

            if (service == null) return false;

            var startTime = date.Date.Add(selectedTime);
            var endTime = startTime.AddMinutes(service.DurationMinutes);

            if (!IsValidAppointmentTime(startTime, endTime, selectedTime))
            {
                return false;
            }

            var hasConflict = await context.Appointments
                .AnyAsync(a => a.Id != id && a.StartTime < endTime && a.EndTime > startTime);

            if (hasConflict) return false;

            appointment.ServiceId = service.Id;
            appointment.StartTime = startTime;
            appointment.EndTime = endTime;
            appointment.Notes = notes;

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id, string userId)
        {
            var appointment = await context.Appointments
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

            if (appointment == null) return false;

            context.Appointments.Remove(appointment);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Service>> GetAllServicesAsync()
        {
            return await context.Services
                .AsNoTracking()
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<List<string>> GetAvailableSlotsAsync(DateTime date, int serviceId)
        {
            if (date.Date < DateTime.Today || date.DayOfWeek == DayOfWeek.Sunday)
            {
                return new List<string>();
            }

            var service = await context.Services
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == serviceId);

            if (service == null) return new List<string>();

            var appointments = await context.Appointments
                .Where(a => a.StartTime.Date == date.Date)
                .AsNoTracking()
                .ToListAsync();

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