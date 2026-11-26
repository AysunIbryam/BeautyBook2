using BeautyBook.Data.Models;
using BeautyBook.Data.Models.Models;
using BeautyBook.Data.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BeautyBook.Data.Repositories.Implementations
{
    public class EmployeeRepository
        : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Employee>> GetAllWithCategoryAsync()
        {
            return await AllAsNoTracking()
                .Include(e => e.Category)
                .OrderBy(e => e.Category.Name)
                .ThenBy(e => e.FirstName)
                .ToListAsync();
        }
        public async Task<IEnumerable<Employee>> GetByCategoryAsync(int categoryId)
        {
            return await AllAsNoTracking()
                .Where(e => e.CategoryId == categoryId)
                .OrderBy(e => e.FirstName)
                .ToListAsync();
        }

        public async Task<Employee?> GetByIdReadOnlyAsync(int id)
        {
            return await AllAsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}