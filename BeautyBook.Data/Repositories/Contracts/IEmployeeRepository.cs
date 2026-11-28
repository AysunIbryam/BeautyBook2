using BeautyBook.Data.Models;
using BeautyBook.Data.Models.Models;

namespace BeautyBook.Data.Repositories.Contracts
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        // Read-only, includes Category, ordered by category and name.
        Task<IEnumerable<Employee>> GetAllWithCategoryAsync();
        Task<IEnumerable<Employee>> GetByCategoryAsync(int categoryId);
        Task<Employee?> GetByIdReadOnlyAsync(int id);

        // Read-only, includes Category.
        Task<Employee?> GetByIdWithCategoryAsync(int id);

        // Tracked entity, used for edit and delete.
        Task<Employee?> GetTrackedByIdAsync(int id);

        Task<bool> HasAppointmentsAsync(int employeeId);
    }
}