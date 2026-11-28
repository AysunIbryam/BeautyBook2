using BeautyBook.Data.Models;
using BeautyBook.Services.Core.Models;

namespace BeautyBook.Web.ViewModels.Admin
{
    public class AppointmentsIndexViewModel
    {
        public PagedResult<Appointment> Appointments { get; set; } = null!;

        public string? Status { get; set; }

        public PaginationViewModel Pagination { get; set; } = null!;
    }
}