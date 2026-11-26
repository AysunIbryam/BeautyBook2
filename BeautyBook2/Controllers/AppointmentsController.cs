
using BeautyBook.Services.Core.Contracts;
using BeautyBook.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BeautyBook.Data.Models;

namespace BeautyBook.Web.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAppointmentService appointmentService;
        private readonly IEmployeeService employeeService;
        public AppointmentsController(
            UserManager<ApplicationUser> userManager,
            IAppointmentService appointmentService,
            IEmployeeService employeeService)
        {
            this.userManager = userManager;
            this.appointmentService = appointmentService;
            this.employeeService= employeeService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = userManager.GetUserId(User)!;
            var appointments =
                await appointmentService.GetUserAppointmentsAsync(userId);

            return View(appointments);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var userId = userManager.GetUserId(User)!;
            var appointment =
                await appointmentService.GetByIdAsync(id, userId);

            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = userManager.GetUserId(User)!;
            var appointment =
                await appointmentService.GetByIdAsync(id, userId);

            if (appointment == null)
            {
                return NotFound();
            }

            var model = new CreateAppointmentViewModel
            {
                ServiceId = appointment.ServiceId,
                EmployeeId = appointment.EmployeeId,
                Date = appointment.StartTime.Date,
                StartTime = appointment.StartTime.ToString("HH:mm"),
                Notes = appointment.Notes
            };

            await LoadServicesAsync(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            CreateAppointmentViewModel model)
        {
            var userId = userManager.GetUserId(User)!;

            if (!ModelState.IsValid ||
                !model.ServiceId.HasValue ||
                !model.EmployeeId.HasValue ||
                !model.Date.HasValue ||
                string.IsNullOrWhiteSpace(model.StartTime))
            {
                await LoadServicesAsync(model);
                return View(model);
            }

            var success = await appointmentService.EditAsync(
                id,
                model.ServiceId.Value,
                model.EmployeeId.Value,
                model.Date.Value,
                model.StartTime,
                model.Notes,
                userId);

            if (!success)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Възникна грешка при обновяване на резервацията или часът е зает/невалиден.");

                await LoadServicesAsync(model);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = userManager.GetUserId(User)!;
            var appointment =
                await appointmentService.GetByIdAsync(id, userId);

            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = userManager.GetUserId(User)!;
            var success =
                await appointmentService.DeleteAsync(id, userId);

            if (!success)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CreateAppointmentViewModel();

            await LoadServicesAsync(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CreateAppointmentViewModel model)
        {
            if (!ModelState.IsValid ||
                !model.ServiceId.HasValue ||
                !model.EmployeeId.HasValue ||
                !model.Date.HasValue ||
                string.IsNullOrWhiteSpace(model.StartTime))
            {
                await LoadServicesAsync(model);
                return View(model);
            }

            var userId = userManager.GetUserId(User)!;

            var success = await appointmentService.CreateAsync(
                model.ServiceId.Value,
                model.EmployeeId.Value,
                model.Date.Value,
                model.StartTime,
                model.Notes,
                userId);

            if (!success)
            {
                ModelState.AddModelError(
                    nameof(model.StartTime),
                    "Избраният час е невалиден или вече е зает.");

                await LoadServicesAsync(model);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> AvailableSlots(
             DateTime date,
             int serviceId,
             int employeeId)
        {
            var slots = await appointmentService.GetAvailableSlotsAsync(
                date,
                serviceId,
                employeeId);

            return Json(slots);
        }

        [HttpGet]
        public async Task<IActionResult> EmployeesByService(int serviceId)
        {
            var employees = await employeeService.GetByServiceAsync(serviceId);

            return Json(employees.Select(e => new
            {
                id = e.Id,
                name = $"{e.FullName} – {e.Position}"
            }));
        }

        private async Task LoadServicesAsync(
            CreateAppointmentViewModel model)
        {
            var services =
                await appointmentService.GetAllServicesAsync();

            model.Services = new SelectList(
                services,
                "Id",
                "Name",
                model.ServiceId);
        }
    }
}
