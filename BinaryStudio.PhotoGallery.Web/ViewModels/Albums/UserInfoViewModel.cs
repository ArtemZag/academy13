namespace BinaryStudio.PhotoGallery.Web.ViewModels.Albums
{
    public class UserInfoViewModel
    {
        public UserInfoViewModel(string albumCount, string photoCount, string fullName, string lastPhotoAdded, string isAdmin, string department, string userAvatar)
        {
            AlbumCount = albumCount;
            PhotoCount = photoCount;
            FullName = fullName;
            LastPhotoAdded = lastPhotoAdded;
            IsAdmin = isAdmin;
            Department = department;
            UserAvatar = userAvatar;
        }

        public string AlbumCount { get; set; }
        public string PhotoCount { get; set; }
        public string FullName { get; set; }
        public string LastPhotoAdded { get; set; }
        public string IsAdmin { get; set; }
        public string Department { get; set; }
        public string UserAvatar { get; set; }
    }
}