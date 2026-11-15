using BeautyBook.Data.Models;
using BeautyBook.Data.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BeautyBook.Data.Repositories.Implementations
{
    public class CategoryRepository
        : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await AllAsNoTracking()
                .ToListAsync();
        }

        public async Task<Category?> GetByIdWithServicesAsync(int id)
        {
            return await AllAsNoTracking()
                .Include(c => c.Services)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
