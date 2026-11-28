using BeautyBook.Data.Models;
using BeautyBook.Data.Repositories.Contracts;
using BeautyBook.Services.Core.Contracts;

namespace BeautyBook.Services.Core.Services
{
    public class SalonServiceService : ISalonServiceService
    {
        private readonly IServiceRepository serviceRepository;
        private readonly ICategoryRepository categoryRepository;

        public SalonServiceService(
            IServiceRepository serviceRepository,
            ICategoryRepository categoryRepository)
        {
            this.serviceRepository = serviceRepository;
            this.categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Service>> GetAllAsync()
        {
            return await serviceRepository.GetAllWithCategoryAsync();
        }

        public async Task<Service?> GetByIdAsync(int id)
        {
            return await serviceRepository.GetByIdWithCategoryAsync(id);
        }

        public async Task<bool> CreateAsync(
            string name,
            string? description,
            decimal price,
            int durationMinutes,
            int categoryId)
        {
            if (!await CategoryExistsAsync(categoryId))
            {
                return false;
            }

            var service = new Service
            {
                Name = name.Trim(),
                Description = NormalizeDescription(description),
                Price = price,
                DurationMinutes = durationMinutes,
                CategoryId = categoryId
            };

            await serviceRepository.AddAsync(service);
            await serviceRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditAsync(
            int id,
            string name,
            string? description,
            decimal price,
            int durationMinutes,
            int categoryId)
        {
            var service = await serviceRepository.GetTrackedByIdAsync(id);

            if (service == null || !await CategoryExistsAsync(categoryId))
            {
                return false;
            }

            service.Name = name.Trim();
            service.Description = NormalizeDescription(description);
            service.Price = price;
            service.DurationMinutes = durationMinutes;
            service.CategoryId = categoryId;

            await serviceRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> HasAppointmentsAsync(int id)
        {
            return await serviceRepository.HasAppointmentsAsync(id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var service = await serviceRepository.GetTrackedByIdAsync(id);

            if (service == null || await serviceRepository.HasAppointmentsAsync(id))
            {
                return false;
            }

            serviceRepository.Delete(service);
            await serviceRepository.SaveChangesAsync();

            return true;
        }

        private async Task<bool> CategoryExistsAsync(int categoryId)
        {
            return await categoryRepository.GetByIdAsync(categoryId) != null;
        }

        private static string? NormalizeDescription(string? description)
        {
            return string.IsNullOrWhiteSpace(description)
                ? null
                : description.Trim();
        }
    }
}