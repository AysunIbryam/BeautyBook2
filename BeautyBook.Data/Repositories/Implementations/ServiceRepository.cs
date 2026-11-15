using BeautyBook.Data.Models;
using BeautyBook.Data.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BeautyBook.Data.Repositories.Implementations
{
    public class ServiceRepository
        : Repository<Service>, IServiceRepository
    {
        public ServiceRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Service>> GetAllWithCategoryAsync()
        {
            return await AllAsNoTracking()
                .Include(s => s.Category)
                .ToListAsync();
        }

        public async Task<Service?> GetByIdWithCategoryAsync(int id)
        {
            return await AllAsNoTracking()
                .Include(s => s.Category)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Service>> GetAllOrderedByNameAsync()
        {
            return await AllAsNoTracking()
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<Service?> GetByIdReadOnlyAsync(int id)
        {
            return await AllAsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}