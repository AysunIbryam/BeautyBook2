using BeautyBook.Data.Models;

namespace BeautyBook.Services.Core.Contracts
{
    public interface ISalonServiceService
    {
        Task<IEnumerable<Service>> GetAllAsync();

        Task<Service?> GetByIdAsync(int id);
    }
}