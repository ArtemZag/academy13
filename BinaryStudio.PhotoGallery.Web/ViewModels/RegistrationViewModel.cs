using System.ComponentModel.DataAnnotations;

namespace BinaryStudio.PhotoGallery.Web.ViewModels
{
    public class RegistrationViewModel
    {
        [Required(ErrorMessage = "E-mail address is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(20, ErrorMessage = "Password {0} must have {1}-{2} symbols", MinimumLength = 6)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [StringLength(20, ErrorMessage = "Confirm password {0} must have {1}-{2} symbols", MinimumLength = 6)]
        [Compare("Password", ErrorMessage = "Passwords are not equel")]
        public string ConfirmPassword { get; set; }
    }
}