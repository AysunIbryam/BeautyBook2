using BeautyBook.Data.Models;
using BeautyBook.Data.Models.Models;
using BeautyBook.Services.Core.Models;

namespace BeautyBook.Services.Core.Contracts
{
    public interface IAppointmentService
    {
        Task<bool> IsAvailableAsync(DateTime date, TimeSpan startTime, int serviceId, int employeeId);

        Task<IEnumerable<Appointment>> GetUserAppointmentsAsync(string userId);

        Task<Appointment?> GetByIdAsync(int id, string userId);

        Task<bool> CreateAsync(int serviceId, int employeeId, DateTime date, string startTime, string? notes, string userId);

        Task<bool> EditAsync(int id, int serviceId, int employeeId, DateTime date, string startTime, string? notes, string userId);

        Task<bool> DeleteAsync(int id, string userId);

        Task<IEnumerable<Service>> GetAllServicesAsync();

        Task<List<string>> GetAvailableSlotsAsync(DateTime date, int serviceId, int employeeId);

        Task<IEnumerable<Appointment>> GetAllForAdminAsync();

        Task<bool> SetStatusAsync(int id, string status);

        Task<bool> DeleteAsAdminAsync(int id);
        Task<PagedResult<Appointment>> GetPageForAdminAsync(string? status, int page,int pageSize);
    }
}