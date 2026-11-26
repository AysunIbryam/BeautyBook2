using BeautyBook.Services.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BeautyBook.Web.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeeService employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public async Task<IActionResult> Index()
        {
            var employees = await employeeService.GetAllAsync();

            return View(employees);
        }
    }
}