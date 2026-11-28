using BeautyBook.Data.Models;

namespace BeautyBook.Data.Repositories.Contracts
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IEnumerable<Category>> GetAllAsync();

        // Includes the category's Services.
        Task<Category?> GetByIdWithServicesAsync(int id);

        // Tracked entity, used for edit and delete.
        Task<Category?> GetTrackedByIdAsync(int id);

        // True if the category has services or employees.
        Task<bool> HasDependenciesAsync(int id);

        Task<bool> NameExistsAsync(string name, int? excludeId = null);
    }
}