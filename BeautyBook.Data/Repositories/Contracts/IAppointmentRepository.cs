using BeautyBook.Data.Models;
using BeautyBook.Data.Models.Models;

namespace BeautyBook.Data.Repositories.Contracts
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        Task<IEnumerable<Appointment>> GetByUserAsync(string userId);

        // Read-only (AsNoTracking), includes Service and Employee.
        Task<Appointment?> GetByIdForUserAsync(int id, string userId);

        // Tracked entity, used for edit and delete.
        Task<Appointment?> GetTrackedByIdForUserAsync(int id, string userId);

        // Appointments of one employee on a given day.
        Task<List<Appointment>> GetByDateAsync(DateTime date, int employeeId);

        // Checks for overlap with the same employee's other appointments.
        Task<bool> HasConflictAsync(
            DateTime start,
            DateTime end,
            int employeeId,
            int? excludeAppointmentId = null);

        Task<List<Appointment>> GetAllWithDetailsAsync();

        Task<Appointment?> GetTrackedByIdAsync(int id);
        Task<int> CountForAdminAsync(string? status);

        Task<List<Appointment>> GetPageForAdminAsync(string? status, int skip, int take);
    }
}