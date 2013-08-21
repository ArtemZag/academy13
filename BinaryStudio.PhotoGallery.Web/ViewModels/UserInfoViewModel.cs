using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BinaryStudio.PhotoGallery.Web.ViewModels
{
    public class UserInfoViewModel
    {
        public UserInfoViewModel(string albumCount, string photoCount, string fullName, string lastPhotoAdded, string isAdmin, string department, string userAvatar)
        {
            this.albumCount = albumCount;
            this.photoCount = photoCount;
            this.fullName = fullName;
            this.lastPhotoAdded = lastPhotoAdded;
            this.isAdmin = isAdmin;
            this.department = department;
            this.userAvatar = userAvatar;
        }

        public string albumCount { get; set; }
        public string photoCount { get; set; }
        public string fullName { get; set; }
        public string lastPhotoAdded { get; set; }
        public string isAdmin { get; set; }
        public string department { get; set; }
        public string userAvatar { get; set; }
    }
}