using System.ComponentModel.DataAnnotations;

namespace BinaryStudio.PhotoGallery.Web.ViewModels
{
    public class RemindPassViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [MaxLength(255)]
        public string UserEmail { get; set; }
    }
}