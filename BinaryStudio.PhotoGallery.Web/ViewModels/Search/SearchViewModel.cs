namespace BinaryStudio.PhotoGallery.Web.ViewModels.Search
{
    public class SearchViewModel
    {
        public string CacheToken { get; set; }

        public string SearchQuery { get; set; }

        public int Begin { get; set; }
        public int End { get; set; }

        public bool IsSearchPhotosByName { get; set; }
        public bool IsSearchPhotosByTags { get; set; }
        public bool IsSearchPhotosByDescription { get; set; }

        public bool IsSearchAlbumsByName { get; set; }
        public bool IsSearchAlbumsByTags { get; set; }
        public bool IsSearchAlbumsByDescription { get; set; }

        public bool IsSearchUsersByName { get; set; }
        public bool IsSearchUserByDepartment { get; set; }

        public bool IsSearchByComments { get; set; }
    }
}