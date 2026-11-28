using BeautyBook.Data.Models;
using BeautyBook.Services.Core.Models;

namespace BeautyBook.Services.Core.Contracts
{
    public interface ISalonServiceService
    {
        Task<IEnumerable<Service>> GetAllAsync();

        Task<Service?> GetByIdAsync(int id);

        Task<bool> CreateAsync(
            string name,
            string? description,
            decimal price,
            int durationMinutes,
            int categoryId);

        Task<bool> EditAsync(
            int id,
            string name,
            string? description,
            decimal price,
            int durationMinutes,
            int categoryId);

        Task<bool> HasAppointmentsAsync(int id);

        // Returns false if the service does not exist or has appointments.
        Task<bool> DeleteAsync(int id);

        Task<PagedResult<Service>> SearchAsync(
            string? search,
            int? categoryId,
            int page,
            int pageSize);
    }
}