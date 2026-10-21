
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BeautyBook.Web.ViewModels
{
    public class CreateAppointmentViewModel
    {
        [Required(ErrorMessage = "Моля, изберете услуга.")]
        [Display(Name = "Услуга")]
        public int? ServiceId { get; set; }

        [Required(ErrorMessage = "Моля, изберете дата.")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата")]
        public DateTime? Date { get; set; }

        [Required(ErrorMessage = "Моля, изберете начален час.")]
        [Display(Name = "Начален час")]
        public string? StartTime { get; set; }

        [StringLength(500, ErrorMessage = "Бележките могат да бъдат до 500 символа.")]
        [Display(Name = "Бележки")]
        public string? Notes { get; set; }

        public IEnumerable<SelectListItem> Services { get; set; }
            = new List<SelectListItem>();
    }
}
