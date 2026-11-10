using BeautyBook.Data.Models;

namespace BeautyBook.Services.Core.Contracts
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllAsync();

        Task<Category?> GetByIdAsync(int id);
    }
}
