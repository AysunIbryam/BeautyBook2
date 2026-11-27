using BeautyBook.Data.Models;
using BeautyBook.Data.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautyBook.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = RoleNames.Admin)]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}