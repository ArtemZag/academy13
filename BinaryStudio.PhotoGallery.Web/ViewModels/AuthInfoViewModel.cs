using System.ComponentModel.DataAnnotations;

namespace BinaryStudio.PhotoGallery.Web.ViewModels
{
    public class AuthInfoViewModel
    {
        [Required(ErrorMessage = "E-mail address is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(20, ErrorMessage = "Password must have {2}-{1} symbols", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}