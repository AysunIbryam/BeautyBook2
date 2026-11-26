using BeautyBook.Data.Models;
using BeautyBook.Data.Models.Models;

namespace BeautyBook.Data.Repositories.Contracts
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        // Read-only, includes Category, ordered by category and name.
        Task<IEnumerable<Employee>> GetAllWithCategoryAsync();
    }
}