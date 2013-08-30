using System.Collections.Generic;

namespace BinaryStudio.PhotoGallery.Web.ViewModels
{
    public class AlbumGroupsViewModel
    {
        public List<AvailableGroupViewModel> ViewModels { get; set; }

        public int AlbumId { get; set; }
    }
}