using BeautyBook.Data.Models;
using BeautyBook.Data.Models.Models;

namespace BeautyBook.Services.Core.Contracts
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<IEnumerable<Employee>> GetByServiceAsync(int serviceId);
        Task<Employee?> GetByIdAsync(int id);

        Task<bool> CreateAsync(
            string firstName,
            string lastName,
            string position,
            string? bio,
            int categoryId);

        // Returns false if the employee or category does not exist, or if the
        // category is changed while the employee has appointments.
        Task<bool> EditAsync(
            int id,
            string firstName,
            string lastName,
            string position,
            string? bio,
            int categoryId);

        Task<bool> HasAppointmentsAsync(int id);

        // Returns false if the employee does not exist or has appointments.
        Task<bool> DeleteAsync(int id);
    }
}
