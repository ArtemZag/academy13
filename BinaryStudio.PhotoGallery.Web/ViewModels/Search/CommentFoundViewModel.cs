using System;

namespace BinaryStudio.PhotoGallery.Web.ViewModels.Search
{
    public class CommentFoundViewModel : IFoundViewModel
    {
        public string Text { get; set; }

        public string UserAvatarPath { get; set; }

        public string UserViewUrl { get; set; }

        public string UserName { get; set; }

        public DateTime DateOfCreation { get; set; }

        public string Type
        {
            get { return "comment"; }
        }
    }
}