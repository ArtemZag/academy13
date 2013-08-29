namespace BinaryStudio.PhotoGallery.Web.ViewModels
{
    public class AvialableGroupViewModel
    {
        public bool CanSeePhotos { get; set; }

        public bool CanSeeComments { get; set; }

        public string Name { get; set; }

        public int GroupId { get; set; }
    }
}