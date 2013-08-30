using System.ComponentModel.DataAnnotations;

namespace BinaryStudio.PhotoGallery.Web.ViewModels
{
    public class RemindPassViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}