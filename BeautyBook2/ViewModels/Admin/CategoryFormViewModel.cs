using System.ComponentModel.DataAnnotations;

namespace BeautyBook.Web.ViewModels.Admin
{
    public class CategoryFormViewModel
    {
        [Required(ErrorMessage = "Моля, въведете име.")]
        [StringLength(50, ErrorMessage = "Името може да бъде до 50 символа.")]
        [Display(Name = "Име на категорията")]
        public string Name { get; set; } = string.Empty;
    }
}