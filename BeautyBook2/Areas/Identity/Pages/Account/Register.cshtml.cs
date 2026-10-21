using BeautyBook.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeautyBook.Web.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [System.ComponentModel.DataAnnotations.Required]
            [System.ComponentModel.DataAnnotations.Display(Name = "Собствено име")]
            public string FirstName { get; set; } = string.Empty;

            [System.ComponentModel.DataAnnotations.Required]
            [System.ComponentModel.DataAnnotations.Display(Name = "Фамилия")]
            public string LastName { get; set; } = string.Empty;

            [System.ComponentModel.DataAnnotations.Required]
            [System.ComponentModel.DataAnnotations.EmailAddress]
            public string Email { get; set; } = string.Empty;

            [System.ComponentModel.DataAnnotations.Required]
            [System.ComponentModel.DataAnnotations.StringLength(
                100, MinimumLength = 6)]
            [System.ComponentModel.DataAnnotations.DataType(
                System.ComponentModel.DataAnnotations.DataType.Password)]
            public string Password { get; set; } = string.Empty;

            [System.ComponentModel.DataAnnotations.Required]
            [System.ComponentModel.DataAnnotations.Compare(
                nameof(Password), ErrorMessage = "Паролите не съвпадат.")]
            [System.ComponentModel.DataAnnotations.DataType(
                System.ComponentModel.DataAnnotations.DataType.Password)]
            [System.ComponentModel.DataAnnotations.Display(
                Name = "Потвърди паролата")]
            public string ConfirmPassword { get; set; } = string.Empty;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = new ApplicationUser
            {
                FirstName = Input.FirstName.Trim(),
                LastName = Input.LastName.Trim(),
                UserName = Input.Email,
                Email = Input.Email
            };

            var result = await userManager.CreateAsync(
                user, Input.Password);

            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}
