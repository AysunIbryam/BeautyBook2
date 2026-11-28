using BeautyBook.Data.Models;
using BeautyBook.Data.Models.Models;
using BeautyBook.Services.Core.Contracts;
using BeautyBook.Web.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BeautyBook.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = RoleNames.Admin)]
    public class ServicesController : Controller
    {
        private readonly ISalonServiceService serviceService;
        private readonly ICategoryService categoryService;

        public ServicesController(
            ISalonServiceService serviceService,
            ICategoryService categoryService)
        {
            this.serviceService = serviceService;
            this.categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var services = await serviceService.GetAllAsync();

            return View(services);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new ServiceFormViewModel();
            await LoadCategoriesAsync(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync(model);
                return View(model);
            }

            var success = await serviceService.CreateAsync(
                model.Name,
                model.Description,
                model.Price!.Value,
                model.DurationMinutes!.Value,
                model.CategoryId!.Value);

            if (!success)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Услугата не може да бъде добавена. Проверете избраната категория.");

                await LoadCategoriesAsync(model);
                return View(model);
            }

            TempData["Success"] = "Услугата е добавена успешно.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var service = await serviceService.GetByIdAsync(id);

            if (service == null)
            {
                return NotFound();
            }

            var model = new ServiceFormViewModel
            {
                Name = service.Name,
                Description = service.Description,
                Price = service.Price,
                DurationMinutes = service.DurationMinutes,
                CategoryId = service.CategoryId
            };

            await LoadCategoriesAsync(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ServiceFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync(model);
                return View(model);
            }

            var success = await serviceService.EditAsync(
                id,
                model.Name,
                model.Description,
                model.Price!.Value,
                model.DurationMinutes!.Value,
                model.CategoryId!.Value);

            if (!success)
            {
                return NotFound();
            }

            TempData["Success"] = "Промените са запазени.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var service = await serviceService.GetByIdAsync(id);

            if (service == null)
            {
                return NotFound();
            }

            ViewData["HasAppointments"] =
                await serviceService.HasAppointmentsAsync(id);

            return View(service);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await serviceService.DeleteAsync(id);

            if (success)
            {
                TempData["Success"] = "Услугата е изтрита.";
            }
            else
            {
                TempData["Error"] =
                    "Услугата не може да бъде изтрита, защото има свързани резервации.";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadCategoriesAsync(ServiceFormViewModel model)
        {
            var categories = await categoryService.GetAllAsync();

            model.Categories = new SelectList(
                categories,
                "Id",
                "Name",
                model.CategoryId);
        }
    }
}
