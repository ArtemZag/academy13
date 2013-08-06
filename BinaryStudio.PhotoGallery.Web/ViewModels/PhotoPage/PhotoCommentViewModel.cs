using System;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Web.ViewModels.PhotoPage
{
    public class PhotoCommentViewModel
    {
        public UserInfoViewModel UserInfo { get; set; }
        public DateTime DateOfCreating { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public int Reply { get; set; }
    }
}