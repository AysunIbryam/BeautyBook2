using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BeautyBook.Web.ViewModels.Admin
{
    public class EmployeeFormViewModel
    {
        [Required(ErrorMessage = "Моля, въведете собствено име.")]
        [StringLength(50, ErrorMessage = "Името може да бъде до 50 символа.")]
        [Display(Name = "Собствено име")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Моля, въведете фамилия.")]
        [StringLength(50, ErrorMessage = "Фамилията може да бъде до 50 символа.")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Моля, въведете длъжност.")]
        [StringLength(100, ErrorMessage = "Длъжността може да бъде до 100 символа.")]
        [Display(Name = "Длъжност")]
        public string Position { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Описанието може да бъде до 500 символа.")]
        [Display(Name = "Кратко описание")]
        public string? Bio { get; set; }

        [Required(ErrorMessage = "Моля, изберете категория.")]
        [Display(Name = "Категория")]
        public int? CategoryId { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
            = new List<SelectListItem>();
    }
}