using BeautyBook.Data.Models;
using BeautyBook.Data.Models.Models;
using BeautyBook.Services.Core.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautyBook.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = RoleNames.Admin)]
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentService appointmentService;

        public AppointmentsController(IAppointmentService appointmentService)
        {
            this.appointmentService = appointmentService;
        }

        public async Task<IActionResult> Index()
        {
            var appointments = await appointmentService.GetAllForAdminAsync();
            return View(appointments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm(int id)
        {
            await appointmentService.SetStatusAsync(id, "Confirmed");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            await appointmentService.SetStatusAsync(id, "Cancelled");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await appointmentService.DeleteAsAdminAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
