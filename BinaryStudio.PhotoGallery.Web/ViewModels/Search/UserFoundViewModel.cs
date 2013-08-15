namespace BinaryStudio.PhotoGallery.Web.ViewModels.Search
{
    public class UserFoundViewModel : IFoundViewModel
    {
        public string Name { get; set; }

        public string UserViewUri { get; set; }

        public string Department { get; set; }

        public bool IsOnline { get; set; }

        public string AvatarPath { get; set; }

        public string Type
        {
            get { return "user"; }
        }
    }
}