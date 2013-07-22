using System.ComponentModel.DataAnnotations;

namespace BinaryStudio.PhotoGallery.Web.ViewModels
{
    public class RemindPassViewModel
    {
        [Required(ErrorMessage = "E-mail or login is required")]
        [DataType(DataType.Text)]
        public string EmailOrLogin { get; set; }
    }
}