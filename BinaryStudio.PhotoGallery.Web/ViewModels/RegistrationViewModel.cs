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
        [StringLength(20, ErrorMessage = "Password must have {2}-{1} symbols", MinimumLength = 6)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [StringLength(20, ErrorMessage = "Confirm password must have {2}-{1} symbols", MinimumLength = 6)]
        [Compare("Password", ErrorMessage = "Passwords are not equel")]
        public string ConfirmPassword { get; set; }
    }
}