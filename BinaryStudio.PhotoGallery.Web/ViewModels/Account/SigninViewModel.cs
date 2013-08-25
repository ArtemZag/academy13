using System.ComponentModel.DataAnnotations;

namespace BinaryStudio.PhotoGallery.Web.ViewModels.Account
{
    public class SigninViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(255, MinimumLength = 5)]
        public string Email { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
       
        public bool RememberMe { get; set; }
    }
}