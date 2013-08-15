using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Web.ViewModels
{
    public class AlbumViewModel
    {
        public string collageSource { get; set; }
        public int Id { get; set; }
        public string AlbumName { get; set; }
        public string Description { get; set; }
        public DateTime DateOfCreation { get; set; }
        public int UserModelId { get; set; }
        public virtual ICollection<AlbumTagModel> AlbumTags { get; set; }
    }
}