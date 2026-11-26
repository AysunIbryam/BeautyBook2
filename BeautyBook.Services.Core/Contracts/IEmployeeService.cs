using BeautyBook.Data.Models;
using BeautyBook.Data.Models.Models;

namespace BeautyBook.Services.Core.Contracts
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<IEnumerable<Employee>> GetByServiceAsync(int serviceId);
    }
}
