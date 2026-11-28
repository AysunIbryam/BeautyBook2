using BeautyBook.Data.Models;
using BeautyBook.Data.Models.Models;
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
                .Include(a => a.Employee)
                .Where(a => a.UserId == userId)
                .OrderBy(a => a.StartTime)
                .ToListAsync();
        }

        public async Task<Appointment?> GetByIdForUserAsync(int id, string userId)
        {
            return await AllAsNoTracking()
                .Include(a => a.Service)
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);
        }
        public async Task<Appointment?> GetTrackedByIdForUserAsync(int id, string userId)
        {
            return await All()
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);
        }

        public async Task<List<Appointment>> GetByDateAsync(DateTime date, int employeeId)
        {
            var dayStart = date.Date;
            var dayEnd = dayStart.AddDays(1);

            return await AllAsNoTracking()
                .Where(a => a.EmployeeId == employeeId &&
                            a.StartTime >= dayStart &&
                            a.StartTime < dayEnd)
                .ToListAsync();
        }

        public async Task<bool> HasConflictAsync(
            DateTime start,
            DateTime end,
            int employeeId,
            int? excludeAppointmentId = null)
        {
            return await AllAsNoTracking()
                .AnyAsync(a =>
                    a.EmployeeId == employeeId &&
                    (excludeAppointmentId == null || a.Id != excludeAppointmentId) &&
                    a.StartTime < end &&
                    a.EndTime > start);
        }

        public async Task<List<Appointment>> GetAllWithDetailsAsync()
        {
            return await AllAsNoTracking()
                .Include(a => a.Service)
                .Include(a => a.User)
                .Include(a => a.Employee)
                .OrderBy(a => a.StartTime)
                .ToListAsync();
        }

        public async Task<Appointment?> GetTrackedByIdAsync(int id)
        {
            return await All().FirstOrDefaultAsync(a => a.Id == id);
        }
        public async Task<int> CountForAdminAsync(string? status)
        {
            return await FilterByStatus(AllAsNoTracking(), status).CountAsync();
        }

        public async Task<List<Appointment>> GetPageForAdminAsync(
            string? status,
            int skip,
            int take)
        {
            return await FilterByStatus(AllAsNoTracking(), status)
                .Include(a => a.Service)
                .Include(a => a.User)
                .Include(a => a.Employee)
                .OrderBy(a => a.StartTime)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        private static IQueryable<Appointment> FilterByStatus(
            IQueryable<Appointment> query,
            string? status)
        {
            return string.IsNullOrEmpty(status)
                ? query
                : query.Where(a => a.Status == status);
        }
    }
}