using System;

namespace BinaryStudio.PhotoGallery.Web.ViewModels.Albums
{
    public class UserInfoViewModel
    {
        public string FullName { get; set; }
        public string Department { get; set; }
        public DateTime LastPhotoAdded { get; set; }
        public string AvatarPath { get; set; }
        public int AlbumCount { get; set; }
        public int PhotoCount { get; set; }
    }
}