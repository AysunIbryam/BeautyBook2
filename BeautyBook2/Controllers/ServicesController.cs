
using BeautyBook.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeautyBook.Web.Controllers
{
    public class ServicesController : Controller
    {
        private readonly ApplicationDbContext context;

        public ServicesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: Services
        public async Task<IActionResult> Index()
        {
            var services = await context.Services
                .Include(s => s.Category)
                .AsNoTracking()
                .ToListAsync();

            return View(services);
        }

        // GET: Services/Details/1
        public async Task<IActionResult> Details(int id)
        {
            var service = await context.Services
                .Include(s => s.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);

            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }
    }
}
