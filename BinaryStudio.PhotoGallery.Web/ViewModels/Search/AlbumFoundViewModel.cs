using System;

namespace BinaryStudio.PhotoGallery.Web.ViewModels.Search
{
    public class AlbumFoundViewModel : IFoundViewModel
    {
        public string Name { get; set; }

        public string AlbumViewUrl { get; set; }

        public string UserName { get; set; }

        public string UserViewUrl { get; set; }

        public DateTime DateOfCreation { get; set; }

        public string CollagePath { get; set; }

        public string Type
        {
            get { return "album"; }
        }
    }
}