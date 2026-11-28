using BeautyBook.Data.Models;
using BeautyBook.Data.Models.Models;
using BeautyBook.Services.Core.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BeautyBook.Web.ViewModels.Admin;
using BeautyBook.Web.ViewModels;

namespace BeautyBook.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = RoleNames.Admin)]
    public class AppointmentsController : Controller
    {
        private const int PageSize = 10;

        private static readonly string[] Statuses = { "Pending", "Confirmed", "Cancelled" };

        private readonly IAppointmentService appointmentService;

        public AppointmentsController(IAppointmentService appointmentService)
        {
            this.appointmentService = appointmentService;
        }

        public async Task<IActionResult> Index(string? status, int page = 1)
        {
            // Ignore unknown status values coming from the query string.
            if (!Statuses.Contains(status))
            {
                status = null;
            }

            var result = await appointmentService.GetPageForAdminAsync(
                status,
                page,
                PageSize);

            var routeValues = new Dictionary<string, string>();

            if (status != null)
            {
                routeValues["status"] = status;
            }

            var model = new AppointmentsIndexViewModel
            {
                Appointments = result,
                Status = status,
                Pagination = new PaginationViewModel
                {
                    Page = result.Page,
                    TotalPages = result.TotalPages,
                    RouteValues = routeValues
                }
            };

            return View(model);
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
