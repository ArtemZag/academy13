using System.ComponentModel.DataAnnotations;

namespace BinaryStudio.PhotoGallery.Web.Models
{
    public class AuthInfoViewModel
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string AuthName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Department { get; set; }
    }
}