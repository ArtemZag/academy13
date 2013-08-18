namespace BinaryStudio.PhotoGallery.Web.ViewModels.Upload
{
    public class UploadResultViewModel
    {
        public string Error { get; set; }
        public string Hash { get; set; }
        public int Id { get; set; }
        public bool IsAccepted { get; set; }
    }
}