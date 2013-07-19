using System.ComponentModel.DataAnnotations;

namespace BinaryStudio.PhotoGallery.Web.ViewModels
{
    public class RegistrationViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Nickname { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string AuthName { get; set; }
    }
}