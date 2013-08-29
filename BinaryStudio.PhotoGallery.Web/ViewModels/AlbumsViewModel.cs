using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;

namespace BinaryStudio.PhotoGallery.Web.ViewModels
{
    public class AlbumsViewModel
    {
        public IEnumerable<AlbumViewModel> albums;
        public string requestsUserName;
        public string ownerUserName;
        public bool noAlbumsToView;
    }
}
