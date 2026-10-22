using BeautyBook.Data.Models;

namespace BeautyBook.Services.Core.Contracts
{
    public interface IAppointmentService
    {
        Task<bool> IsAvailableAsync(DateTime date, TimeSpan startTime, int serviceId);

        Task<IEnumerable<Appointment>> GetUserAppointmentsAsync(string userId);

        Task<Appointment?> GetByIdAsync(int id, string userId);

        Task<bool> CreateAsync(int serviceId, DateTime date, string startTime, string? notes, string userId);

        Task<bool> EditAsync(int id, int serviceId, DateTime date, string startTime, string? notes, string userId);

        Task<bool> DeleteAsync(int id, string userId);

        Task<IEnumerable<Service>> GetAllServicesAsync();

        Task<List<string>> GetAvailableSlotsAsync(DateTime date, int serviceId);
    }
}