using System.ComponentModel.DataAnnotations;

namespace BinaryStudio.PhotoGallery.Web.Models
{
    public class AuthInfoViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public bool RememberMe { get; set; }
    }
}