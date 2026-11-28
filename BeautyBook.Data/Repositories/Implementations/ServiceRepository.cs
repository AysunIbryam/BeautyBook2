using BeautyBook.Data.Models;
using BeautyBook.Data.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BeautyBook.Data.Repositories.Implementations
{
    public class ServiceRepository
        : Repository<Service>, IServiceRepository
    {
        public ServiceRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Service>> GetAllWithCategoryAsync()
        {
            return await AllAsNoTracking()
                .Include(s => s.Category)
                .ToListAsync();
        }

        public async Task<Service?> GetByIdWithCategoryAsync(int id)
        {
            return await AllAsNoTracking()
                .Include(s => s.Category)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Service>> GetAllOrderedByNameAsync()
        {
            return await AllAsNoTracking()
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<Service?> GetByIdReadOnlyAsync(int id)
        {
            return await AllAsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Service?> GetTrackedByIdAsync(int id)
        {
            return await All()
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<bool> HasAppointmentsAsync(int serviceId)
        {
            return await context.Appointments
                .AnyAsync(a => a.ServiceId == serviceId);
        }
        public async Task<int> CountFilteredAsync(string? search, int? categoryId)
        {
            return await ApplyFilter(AllAsNoTracking(), search, categoryId)
                .CountAsync();
        }

        public async Task<List<Service>> GetFilteredPageAsync(
            string? search,
            int? categoryId,
            int skip,
            int take)
        {
            return await ApplyFilter(AllAsNoTracking(), search, categoryId)
                .Include(s => s.Category)
                .OrderBy(s => s.Category.Name)
                .ThenBy(s => s.Name)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        private static IQueryable<Service> ApplyFilter(
            IQueryable<Service> query,
            string? search,
            int? categoryId)
        {
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s =>
                    s.Name.Contains(search) ||
                    (s.Description != null && s.Description.Contains(search)));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(s => s.CategoryId == categoryId.Value);
            }

            return query;
        }
    }
}