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
    }
}