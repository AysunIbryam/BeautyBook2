using BeautyBook.Data.Models;
using BeautyBook.Data.Repositories.Contracts;
using BeautyBook.Services.Core.Contracts;

namespace BeautyBook.Services.Core.Services
{
    public class SalonServiceService : ISalonServiceService
    {
        private readonly IServiceRepository serviceRepository;

        public SalonServiceService(IServiceRepository serviceRepository)
        {
            this.serviceRepository = serviceRepository;
        }

        public async Task<IEnumerable<Service>> GetAllAsync()
        {
            return await serviceRepository.GetAllWithCategoryAsync();
        }

        public async Task<Service?> GetByIdAsync(int id)
        {
            return await serviceRepository.GetByIdWithCategoryAsync(id);
        }
    }
}