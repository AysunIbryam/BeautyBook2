using BeautyBook.Services.Core.Contracts;
using System.Globalization;
using BeautyBook.Web.ViewModels;
using BeautyBook.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeautyBook.Data;

namespace BeautyBook.Web.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAppointmentService appointmentService;


        public AppointmentsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IAppointmentService appointmentService)
        {
            this.context = context;
            this.userManager = userManager;
            this.appointmentService = appointmentService;
        }


        public async Task<IActionResult> Index()
        {
            var userId = userManager.GetUserId(User);

            var appointments = await context.Appointments
                .Include(a => a.Service)
                .Where(a => a.UserId == userId)
                .AsNoTracking()
                .OrderBy(a => a.StartTime)
                .ToListAsync();

            return View(appointments);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var userId = userManager.GetUserId(User);

            var appointment = await context.Appointments
                .Include(a => a.Service)
                .AsNoTracking()
                .FirstOrDefaultAsync(a =>
                    a.Id == id && a.UserId == userId);

            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = userManager.GetUserId(User);

            var appointment = await context.Appointments
                .FirstOrDefaultAsync(a =>
                    a.Id == id && a.UserId == userId);

            if (appointment == null)
            {
                return NotFound();
            }

            var model = new CreateAppointmentViewModel
            {
                ServiceId = appointment.ServiceId,
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
            var userId = userManager.GetUserId(User);

            var appointment = await context.Appointments
                .FirstOrDefaultAsync(a =>
                    a.Id == id && a.UserId == userId);

            if (appointment == null)
            {
                return NotFound();
            }

            TimeSpan selectedTime = TimeSpan.Zero;

            if (!string.IsNullOrWhiteSpace(model.StartTime))
            {
                if (!TimeSpan.TryParseExact(
                    model.StartTime,
                    @"hh\:mm",
                    CultureInfo.InvariantCulture,
                    out selectedTime))
                {
                    ModelState.AddModelError(
                        nameof(model.StartTime),
                        "Моля, избери валиден час.");
                }
            }

            var service = await context.Services
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == model.ServiceId);

            if (service == null)
            {
                ModelState.AddModelError(
                    nameof(model.ServiceId),
                    "Моля, избери валидна услуга.");
            }

            if (ModelState.IsValid)
            {
                var startTime = model.Date!.Value.Date.Add(selectedTime);
                var endTime = startTime.AddMinutes(service!.DurationMinutes);

                if (startTime <= DateTime.Now)
                {
                    ModelState.AddModelError(
                        nameof(model.StartTime),
                        "Избери дата и час в бъдещето.");
                }

                if (startTime.DayOfWeek == DayOfWeek.Sunday)
                {
                    ModelState.AddModelError(
                        nameof(model.Date),
                        "Салонът не работи в неделя.");
                }

                if (selectedTime.TotalMinutes % 15 != 0)
                {
                    ModelState.AddModelError(
                        nameof(model.StartTime),
                        "Избери час през 15 минути.");
                }

                if (startTime.TimeOfDay < new TimeSpan(9, 0, 0)
                    || endTime.TimeOfDay > new TimeSpan(18, 0, 0))
                {
                    ModelState.AddModelError(
                        nameof(model.StartTime),
                        "Процедурата трябва да приключва до 18:00 ч.");
                }

                var hasConflict = await context.Appointments
                    .AnyAsync(a =>
                        a.Id != id
                        && a.StartTime < endTime
                        && a.EndTime > startTime);

                if (hasConflict)
                {
                    ModelState.AddModelError(
                        nameof(model.StartTime),
                        "Този час вече е зает. Избери друг.");
                }

                if (ModelState.IsValid)
                {
                    appointment.ServiceId = service!.Id;
                    appointment.StartTime = startTime;
                    appointment.EndTime = endTime;
                    appointment.Notes = model.Notes;

                    await context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }

            await LoadServicesAsync(model);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = userManager.GetUserId(User);

            var appointment = await context.Appointments
                .Include(a => a.Service)
                .AsNoTracking()
                .FirstOrDefaultAsync(a =>
                    a.Id == id && a.UserId == userId);

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
            var userId = userManager.GetUserId(User);

            var appointment = await context.Appointments
                .FirstOrDefaultAsync(a =>
                    a.Id == id && a.UserId == userId);

            if (appointment == null)
            {
                return NotFound();
            }

            context.Appointments.Remove(appointment);
            await context.SaveChangesAsync();

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
            // Проверяваме дали избраният час е във валиден формат.
            TimeSpan selectedTime = TimeSpan.Zero;

            if (!string.IsNullOrWhiteSpace(model.StartTime))
            {
                if (!TimeSpan.TryParseExact(
                    model.StartTime,
                    @"hh\:mm",
                    CultureInfo.InvariantCulture,
                    out selectedTime))
                {
                    ModelState.AddModelError(
                        nameof(model.StartTime),
                        "Моля, избери валиден час.");
                }
            }

            if (!ModelState.IsValid)
            {
                await LoadServicesAsync(model);
                return View(model);
            }

            // Зареждаме услугата от базата данни.
            var service = await context.Services
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == model.ServiceId);

            if (service == null)
            {
                ModelState.AddModelError(
                    nameof(model.ServiceId),
                    "Моля, избери валидна услуга.");

                await LoadServicesAsync(model);
                return View(model);
            }

            // Създаваме началния час от избраните дата и час.
            var startTime = model.Date!.Value.Date.Add(selectedTime);

            // Проверка за бъдеща дата.
            if (startTime <= DateTime.Now)
            {
                ModelState.AddModelError(
                    nameof(model.StartTime),
                    "Избери дата и час в бъдещето.");
            }

            // Проверка за работен ден.
            if (startTime.DayOfWeek == DayOfWeek.Sunday)
            {
                ModelState.AddModelError(
                    nameof(model.Date),
                    "Салонът не работи в неделя.");
            }

            // Проверка за интервал през 15 минути.
            if (selectedTime.TotalMinutes % 15 != 0)
            {
                ModelState.AddModelError(
                    nameof(model.StartTime),
                    "Избери час през 15 минути.");
            }

            // Изчисляваме края според продължителността на услугата.
            var endTime = startTime.AddMinutes(service.DurationMinutes);

            // Проверка за работното време: 09:00–18:00.
            if (startTime.TimeOfDay < new TimeSpan(9, 0, 0)
                || endTime.TimeOfDay > new TimeSpan(18, 0, 0))
            {
                ModelState.AddModelError(
                    nameof(model.StartTime),
                    "Избери час, при който процедурата приключва до 18:00 ч.");
            }

            if (!ModelState.IsValid)
            {
                await LoadServicesAsync(model);
                return View(model);
            }

            // Проверяваме за застъпване с друга активна резервация.
            var isAvailable = await appointmentService.IsAvailableAsync(
                model.Date!.Value.Date,
                selectedTime,
                service.Id);

            if (!isAvailable)
            {
                ModelState.AddModelError(
                    nameof(model.StartTime),
                    "Този час вече е зает. Моля, избери друг час.");

                await LoadServicesAsync(model);
                return View(model);
            }

            // Създаваме Entity обекта, който ще запишем в базата.
            var appointment = new Appointment
            {
                StartTime = startTime,
                EndTime = endTime,
                Notes = model.Notes,
                Status = "Pending",
                UserId = userManager.GetUserId(User)!,
                ServiceId = service.Id
            };

            context.Appointments.Add(appointment);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> AvailableSlots(
            DateTime date,
            int serviceId)
        {
            if (date.Date < DateTime.Today
                || date.DayOfWeek == DayOfWeek.Sunday)
            {
                return Json(new List<string>());
            }

            var service = await context.Services
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == serviceId);

            if (service == null)
            {
                return Json(new List<string>());
            }

            var appointments = await context.Appointments
                .Where(a => a.StartTime.Date == date.Date)
                .AsNoTracking()
                .ToListAsync();

            var slots = new List<string>();

            var openingTime = new TimeSpan(9, 0, 0);
            var closingTime = new TimeSpan(18, 0, 0);

            for (var time = openingTime;
                 time + TimeSpan.FromMinutes(service.DurationMinutes) <= closingTime;
                 time = time.Add(TimeSpan.FromMinutes(15)))
            {
                var start = date.Date.Add(time);
                var end = start.AddMinutes(service.DurationMinutes);

                if (start <= DateTime.Now)
                {
                    continue;
                }

                var hasConflict = appointments.Any(a =>
                    a.StartTime < end && a.EndTime > start);

                if (!hasConflict)
                {
                    slots.Add(start.ToString("HH:mm"));
                }
            }

            return Json(slots);
        }
        private async Task LoadServicesAsync(
            CreateAppointmentViewModel model)
        {
            var services = await context.Services
                .AsNoTracking()
                .OrderBy(s => s.Name)
                .ToListAsync();

            model.Services = new SelectList(
                services,
                "Id",
                "Name",
                model.ServiceId);
        }
    }
}
