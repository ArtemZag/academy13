using System.Collections.Generic;

namespace BinaryStudio.PhotoGallery.Web.ViewModels
{
    public class AlbumsViewModel
    {
        public IEnumerable<AlbumViewModel> Albums;
        public string RequestsUserName;
        public string OwnerUserName;
        public bool NoAlbumsToView;
    }
}
