using BeautyBook.Services.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BeautyBook.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            var categories = await categoryService.GetAllAsync();

            return View(categories);
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var category = await categoryService.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
    }
}