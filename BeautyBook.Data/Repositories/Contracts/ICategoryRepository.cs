using BeautyBook.Data.Models;

namespace BeautyBook.Data.Repositories.Contracts
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IEnumerable<Category>> GetAllAsync();

        // Includes the category's Services.
        Task<Category?> GetByIdWithServicesAsync(int id);
    }
}