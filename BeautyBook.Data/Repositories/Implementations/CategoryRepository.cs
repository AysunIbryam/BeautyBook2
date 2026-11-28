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
        public async Task<Category?> GetTrackedByIdAsync(int id)
        {
            return await All()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> HasDependenciesAsync(int id)
        {
            return await context.Services.AnyAsync(s => s.CategoryId == id)
                || await context.Employees.AnyAsync(e => e.CategoryId == id);
        }

        public async Task<bool> NameExistsAsync(string name, int? excludeId = null)
        {
            return await AllAsNoTracking()
                .AnyAsync(c => c.Name == name &&
                               (excludeId == null || c.Id != excludeId));
        }
    }
}
