using BeautyBook.Data.Models;

namespace BeautyBook.Data.Repositories.Contracts
{
    public interface IServiceRepository : IRepository<Service>
    {
        // Includes Category.
        Task<IEnumerable<Service>> GetAllWithCategoryAsync();

        // Includes Category.
        Task<Service?> GetByIdWithCategoryAsync(int id);

        // Ordered by name, without Category (used for the booking dropdown).
        Task<IEnumerable<Service>> GetAllOrderedByNameAsync();

        // Read-only lookup without includes.
        Task<Service?> GetByIdReadOnlyAsync(int id);

        // Tracked entity, used for edit and delete.
        Task<Service?> GetTrackedByIdAsync(int id);

        Task<bool> HasAppointmentsAsync(int serviceId);
    }
}