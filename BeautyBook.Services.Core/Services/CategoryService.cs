using BeautyBook.Data;
using BeautyBook.Data.Models;
using BeautyBook.Data.Repositories.Contracts;
using BeautyBook.Services.Core.Contracts;
using Microsoft.EntityFrameworkCore;

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
    }
}