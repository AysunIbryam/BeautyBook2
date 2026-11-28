using BeautyBook.Data.Models;
using BeautyBook.Services.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BeautyBook.Web.ViewModels
{
    public class ServicesIndexViewModel
    {
        public PagedResult<Service> Services { get; set; } = null!;

        public string? Search { get; set; }

        public int? CategoryId { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
            = new List<SelectListItem>();

        public PaginationViewModel Pagination { get; set; } = null!;
    }
}