using System.Collections.Generic;

namespace BinaryStudio.PhotoGallery.Web.ViewModels.Search
{
    public class SearchResultViewModel
    {
        public IEnumerable<IFoundViewModel> Items { get; set; }

        public string SearchCacheToken { get; set; }
    }
}