using BeautyBook.Data;
using BeautyBook.Data.Models;
using BeautyBook.Services.Core.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BeautyBook.Services.Core.Services
{
    public class SalonServiceService : ISalonServiceService
    {
        private readonly ApplicationDbContext context;

        public SalonServiceService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Service>> GetAllAsync()
        {
            return await context.Services
                .Include(s => s.Category)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Service?> GetByIdAsync(int id)
        {
            return await context.Services
                .Include(s => s.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}