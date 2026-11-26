using BeautyBook.Data.Models.Models;
using BeautyBook.Data.Repositories.Contracts;
using BeautyBook.Services.Core.Contracts;

namespace BeautyBook.Services.Core.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly IServiceRepository serviceRepository;

        public EmployeeService(
            IEmployeeRepository employeeRepository,
            IServiceRepository serviceRepository)
        {
            this.employeeRepository = employeeRepository;
            this.serviceRepository = serviceRepository;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await employeeRepository.GetAllWithCategoryAsync();
        }

        public async Task<IEnumerable<Employee>> GetByServiceAsync(int serviceId)
        {
            var service = await serviceRepository.GetByIdReadOnlyAsync(serviceId);

            if (service == null)
            {
                return Enumerable.Empty<Employee>();
            }

            return await employeeRepository.GetByCategoryAsync(service.CategoryId);
        }
    }
}