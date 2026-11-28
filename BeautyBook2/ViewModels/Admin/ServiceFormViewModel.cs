using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BeautyBook.Web.ViewModels.Admin
{
    public class ServiceFormViewModel
    {
        [Required(ErrorMessage = "Моля, въведете име.")]
        [StringLength(100, ErrorMessage = "Името може да бъде до 100 символа.")]
        [Display(Name = "Име")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Описанието може да бъде до 500 символа.")]
        [Display(Name = "Описание")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Моля, въведете цена.")]
        [Range(1, 1000, ErrorMessage = "Цената трябва да е между 1 и 1000.")]
        [Display(Name = "Цена (€)")]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "Моля, въведете продължителност.")]
        [Range(15, 480, ErrorMessage = "Продължителността трябва да е между 15 и 480 минути.")]
        [Display(Name = "Продължителност (мин.)")]
        public int? DurationMinutes { get; set; }

        [Required(ErrorMessage = "Моля, изберете категория.")]
        [Display(Name = "Категория")]
        public int? CategoryId { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
            = new List<SelectListItem>();
    }
}