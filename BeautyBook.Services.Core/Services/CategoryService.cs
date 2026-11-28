using BeautyBook.Data.Models;
using BeautyBook.Data.Repositories.Contracts;
using BeautyBook.Services.Core.Contracts;

namespace BeautyBook.Services.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await categoryRepository.GetAllAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await categoryRepository.GetByIdWithServicesAsync(id);
        }

        public async Task<bool> NameExistsAsync(string name, int? excludeId = null)
        {
            return await categoryRepository.NameExistsAsync(name.Trim(), excludeId);
        }

        public async Task<bool> CreateAsync(string name)
        {
            var trimmed = name.Trim();

            if (await categoryRepository.NameExistsAsync(trimmed))
            {
                return false;
            }

            await categoryRepository.AddAsync(new Category { Name = trimmed });
            await categoryRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditAsync(int id, string name)
        {
            var category = await categoryRepository.GetTrackedByIdAsync(id);
            var trimmed = name.Trim();

            if (category == null ||
                await categoryRepository.NameExistsAsync(trimmed, id))
            {
                return false;
            }

            category.Name = trimmed;
            await categoryRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> HasDependenciesAsync(int id)
        {
            return await categoryRepository.HasDependenciesAsync(id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await categoryRepository.GetTrackedByIdAsync(id);

            if (category == null ||
                await categoryRepository.HasDependenciesAsync(id))
            {
                return false;
            }

            categoryRepository.Delete(category);
            await categoryRepository.SaveChangesAsync();

            return true;
        }
    }
}