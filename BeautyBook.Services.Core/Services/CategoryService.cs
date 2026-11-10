using BeautyBook.Data;
using BeautyBook.Data.Models;
using BeautyBook.Services.Core.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BeautyBook.Services.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext context;

        public CategoryService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await context.Categories
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await context.Categories
                .Include(c => c.Services)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}