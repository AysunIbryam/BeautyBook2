using BeautyBook.Data.Models;

namespace BeautyBook.Services.Core.Contracts
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllAsync();

        Task<Category?> GetByIdAsync(int id);

        Task<bool> NameExistsAsync(string name, int? excludeId = null);

        Task<bool> CreateAsync(string name);

        Task<bool> EditAsync(int id, string name);

        Task<bool> HasDependenciesAsync(int id);

        // Returns false if the category does not exist or has dependencies.
        Task<bool> DeleteAsync(int id);
        
    }
}