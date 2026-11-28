using BeautyBook.Data.Models.Models;
using BeautyBook.Data.Repositories.Contracts;
using BeautyBook.Services.Core.Contracts;

namespace BeautyBook.Services.Core.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly IServiceRepository serviceRepository;
        private readonly ICategoryRepository categoryRepository;

        public EmployeeService(
            IEmployeeRepository employeeRepository,
            IServiceRepository serviceRepository,
            ICategoryRepository categoryRepository)
        {
            this.employeeRepository = employeeRepository;
            this.serviceRepository = serviceRepository;
            this.categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await employeeRepository.GetAllWithCategoryAsync();
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await employeeRepository.GetByIdWithCategoryAsync(id);
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

        public async Task<bool> CreateAsync(
            string firstName,
            string lastName,
            string position,
            string? bio,
            int categoryId)
        {
            if (!await CategoryExistsAsync(categoryId))
            {
                return false;
            }

            var employee = new Employee
            {
                FirstName = firstName.Trim(),
                LastName = lastName.Trim(),
                Position = position.Trim(),
                Bio = NormalizeBio(bio),
                CategoryId = categoryId
            };

            await employeeRepository.AddAsync(employee);
            await employeeRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditAsync(
            int id,
            string firstName,
            string lastName,
            string position,
            string? bio,
            int categoryId)
        {
            var employee = await employeeRepository.GetTrackedByIdAsync(id);

            if (employee == null || !await CategoryExistsAsync(categoryId))
            {
                return false;
            }

            if (employee.CategoryId != categoryId &&
                await employeeRepository.HasAppointmentsAsync(id))
            {
                return false;
            }

            employee.FirstName = firstName.Trim();
            employee.LastName = lastName.Trim();
            employee.Position = position.Trim();
            employee.Bio = NormalizeBio(bio);
            employee.CategoryId = categoryId;

            await employeeRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> HasAppointmentsAsync(int id)
        {
            return await employeeRepository.HasAppointmentsAsync(id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var employee = await employeeRepository.GetTrackedByIdAsync(id);

            if (employee == null ||
                await employeeRepository.HasAppointmentsAsync(id))
            {
                return false;
            }

            employeeRepository.Delete(employee);
            await employeeRepository.SaveChangesAsync();

            return true;
        }

        private async Task<bool> CategoryExistsAsync(int categoryId)
        {
            return await categoryRepository.GetByIdAsync(categoryId) != null;
        }

        private static string? NormalizeBio(string? bio)
        {
            return string.IsNullOrWhiteSpace(bio) ? null : bio.Trim();
        }
    }
}