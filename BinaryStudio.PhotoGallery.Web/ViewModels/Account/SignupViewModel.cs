using System.ComponentModel.DataAnnotations;

namespace BinaryStudio.PhotoGallery.Web.ViewModels.Account
{
    public class SignupViewModel
    {
        [DataType(DataType.Text)]
        [StringLength(255)]
        public string Invite { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(255, MinimumLength = 5)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}