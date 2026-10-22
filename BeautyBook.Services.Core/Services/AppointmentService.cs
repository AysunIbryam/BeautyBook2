using BeautyBook.Data;
using BeautyBook.Services.Core.Contracts;
using Microsoft.EntityFrameworkCore;

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

    }
}
