﻿namespace BinaryStudio.PhotoGallery.Web.ViewModels.Search
{
    public class SearchViewModel
    {
        public string SearchCacheToken { get; set; }

        public string SearchQuery { get; set; }

        public int Skip { get; set; }
        public int Take { get; set; }

        public bool IsSearchPhotosByTags { get; set; }
        public bool IsSearchPhotosByDescription { get; set; }

        public bool IsSearchAlbumsByName { get; set; }
        public bool IsSearchAlbumsByDescription { get; set; }

        public bool IsSearchUsersByName { get; set; }
        public bool IsSearchUserByDepartment { get; set; }

        public bool IsSearchByComments { get; set; }
    }
}