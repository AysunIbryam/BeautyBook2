using BeautyBook.Data.Models;

namespace BeautyBook.Data.Repositories.Contracts
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        Task<IEnumerable<Appointment>> GetByUserAsync(string userId);

        // Read-only (AsNoTracking), includes Service.
        Task<Appointment?> GetByIdForUserAsync(int id, string userId);

        // Tracked entity, used for edit and delete.
        Task<Appointment?> GetTrackedByIdForUserAsync(int id, string userId);

        Task<List<Appointment>> GetByDateAsync(DateTime date);

        Task<bool> HasConflictAsync(
            DateTime start,
            DateTime end,
            int? excludeAppointmentId = null);

        Task<List<Appointment>> GetAllWithDetailsAsync();

        Task<Appointment?> GetTrackedByIdAsync(int id);
    }
}