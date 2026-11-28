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
    public class EmployeesController : Controller
    {
        private readonly IEmployeeService employeeService;
        private readonly ICategoryService categoryService;

        public EmployeesController(
            IEmployeeService employeeService,
            ICategoryService categoryService)
        {
            this.employeeService = employeeService;
            this.categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var employees = await employeeService.GetAllAsync();

            return View(employees);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new EmployeeFormViewModel();
            await LoadCategoriesAsync(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync(model);
                return View(model);
            }

            var success = await employeeService.CreateAsync(
                model.FirstName,
                model.LastName,
                model.Position,
                model.Bio,
                model.CategoryId!.Value);

            if (!success)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Служителят не може да бъде добавен. Проверете избраната категория.");

                await LoadCategoriesAsync(model);
                return View(model);
            }

            TempData["Success"] = "Служителят е добавен успешно.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await employeeService.GetByIdAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            var model = new EmployeeFormViewModel
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Position = employee.Position,
                Bio = employee.Bio,
                CategoryId = employee.CategoryId
            };

            await LoadCategoriesAsync(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeeFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync(model);
                return View(model);
            }

            if (await employeeService.GetByIdAsync(id) == null)
            {
                return NotFound();
            }

            var success = await employeeService.EditAsync(
                id,
                model.FirstName,
                model.LastName,
                model.Position,
                model.Bio,
                model.CategoryId!.Value);

            if (!success)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Промяната не е запазена. Служител с резервации не може да сменя категорията си.");

                await LoadCategoriesAsync(model);
                return View(model);
            }

            TempData["Success"] = "Промените са запазени.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await employeeService.GetByIdAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            ViewData["HasAppointments"] =
                await employeeService.HasAppointmentsAsync(id);

            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await employeeService.DeleteAsync(id))
            {
                TempData["Success"] = "Служителят е изтрит.";
            }
            else
            {
                TempData["Error"] =
                    "Служителят не може да бъде изтрит, защото има свързани резервации.";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadCategoriesAsync(EmployeeFormViewModel model)
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