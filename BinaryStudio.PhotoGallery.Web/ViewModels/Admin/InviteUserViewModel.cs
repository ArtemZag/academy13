using System.ComponentModel.DataAnnotations;

namespace BinaryStudio.PhotoGallery.Web.ViewModels.Admin
{
    public class InviteUserViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(255, MinimumLength = 5)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(255)]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(255)]
        public string LastName { get; set; }
    }
}