using BeautyBook.Services.Core.Contracts;
using BeautyBook.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BeautyBook.Web.Controllers
{
    public class ServicesController : Controller
    {
        private const int PageSize = 6;

        private readonly ISalonServiceService serviceService;
        private readonly ICategoryService categoryService;

        public ServicesController(
            ISalonServiceService serviceService,
            ICategoryService categoryService)
        {
            this.serviceService = serviceService;
            this.categoryService = categoryService;
        }

        public async Task<IActionResult> Index(
            string? search,
            int? categoryId,
            int page = 1)
        {
            var result = await serviceService.SearchAsync(
                search,
                categoryId,
                page,
                PageSize);

            var categories = await categoryService.GetAllAsync();

            var routeValues = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(search))
            {
                routeValues["search"] = search.Trim();
            }

            if (categoryId.HasValue)
            {
                routeValues["categoryId"] = categoryId.Value.ToString();
            }

            var model = new ServicesIndexViewModel
            {
                Services = result,
                Search = search,
                CategoryId = categoryId,
                Categories = new SelectList(categories, "Id", "Name", categoryId),
                Pagination = new PaginationViewModel
                {
                    Page = result.Page,
                    TotalPages = result.TotalPages,
                    RouteValues = routeValues
                }
            };

            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var service = await serviceService.GetByIdAsync(id);

            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }
    }
}