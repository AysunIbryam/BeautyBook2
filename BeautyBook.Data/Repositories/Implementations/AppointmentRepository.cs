using BeautyBook.Data.Models;
using BeautyBook.Data.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BeautyBook.Data.Repositories.Implementations
{
    public class AppointmentRepository
        : Repository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Appointment>> GetByUserAsync(string userId)
        {
            return await AllAsNoTracking()
                .Include(a => a.Service)
                .Where(a => a.UserId == userId)
                .OrderBy(a => a.StartTime)
                .ToListAsync();
        }

        public async Task<Appointment?> GetByIdForUserAsync(int id, string userId)
        {
            return await AllAsNoTracking()
                .Include(a => a.Service)
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);
        }

        public async Task<Appointment?> GetTrackedByIdForUserAsync(int id, string userId)
        {
            return await All()
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);
        }

        public async Task<List<Appointment>> GetByDateAsync(DateTime date)
        {
            var dayStart = date.Date;
            var dayEnd = dayStart.AddDays(1);

            return await AllAsNoTracking()
                .Where(a => a.StartTime >= dayStart && a.StartTime < dayEnd)
                .ToListAsync();
        }

        public async Task<bool> HasConflictAsync(
            DateTime start,
            DateTime end,
            int? excludeAppointmentId = null)
        {
            return await AllAsNoTracking()
                .AnyAsync(a =>
                    (excludeAppointmentId == null || a.Id != excludeAppointmentId) &&
                    a.StartTime < end &&
                    a.EndTime > start);
        }

        public async Task<List<Appointment>> GetAllWithDetailsAsync()
        {
            return await AllAsNoTracking()
                .Include(a => a.Service)
                .Include(a => a.User)
                .OrderBy(a => a.StartTime)
                .ToListAsync();
        }

        public async Task<Appointment?> GetTrackedByIdAsync(int id)
        {
            return await All().FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}