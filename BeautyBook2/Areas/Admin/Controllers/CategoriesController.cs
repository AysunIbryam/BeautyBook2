using BeautyBook.Data.Models;
using BeautyBook.Data.Models.Models;
using BeautyBook.Services.Core.Contracts;
using BeautyBook.Web.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautyBook.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = RoleNames.Admin)]
    public class CategoriesController : Controller
    {
        private const string DuplicateNameMessage =
            "Категория с това име вече съществува.";

        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await categoryService.GetAllAsync();

            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CategoryFormViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!await categoryService.CreateAsync(model.Name))
            {
                ModelState.AddModelError(nameof(model.Name), DuplicateNameMessage);
                return View(model);
            }

            TempData["Success"] = "Категорията е добавена успешно.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await categoryService.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(new CategoryFormViewModel { Name = category.Name });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await categoryService.GetByIdAsync(id) == null)
            {
                return NotFound();
            }

            if (!await categoryService.EditAsync(id, model.Name))
            {
                ModelState.AddModelError(nameof(model.Name), DuplicateNameMessage);
                return View(model);
            }

            TempData["Success"] = "Промените са запазени.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await categoryService.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            ViewData["HasDependencies"] =
                await categoryService.HasDependenciesAsync(id);

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await categoryService.DeleteAsync(id))
            {
                TempData["Success"] = "Категорията е изтрита.";
            }
            else
            {
                TempData["Error"] =
                    "Категорията не може да бъде изтрита, защото има свързани услуги или служители.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}