
using BeautyBook.Data;
using BeautyBook.Services.Core.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeautyBook.Web.Controllers
{
    public class ServicesController : Controller
    {
        private readonly ISalonServiceService serviceService;

        public ServicesController(ISalonServiceService serviceService)
        {
            this.serviceService = serviceService;
        }

        public async Task<IActionResult> Index()
        {
            var services = await serviceService.GetAllAsync();
            return View(services);
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
