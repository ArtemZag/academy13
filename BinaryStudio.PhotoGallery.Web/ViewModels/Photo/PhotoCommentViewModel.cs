using System;

namespace BinaryStudio.PhotoGallery.Web.ViewModels.Photo
{
    public class PhotoCommentViewModel
    {
        public UserInfoViewModel UserInfo { get; set; }
        public int year, month, day, hour, minute, second;
        public string Text { get; set; }
        public int Rating { get; set; }
        public int Reply { get; set; }
    }
}