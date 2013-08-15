namespace BinaryStudio.PhotoGallery.Domain.Services.Search
{
    public class SearchArguments
    {
        /// <summary>
        /// User is searching
        /// </summary>
        public int UserId { get; set; }

        public string SearchCacheToken { get; set; }

        public string SearchQuery { get; set; }

        public int Interval { get; set; }

        public bool IsSearchPhotosByName { get; set; }
        public bool IsSearchPhotosByTags { get; set; }
        public bool IsSearchPhotosByDescription { get; set; }

        public bool IsSearchByPhotos
        {
            get { return IsSearchPhotosByName || IsSearchPhotosByTags || IsSearchPhotosByDescription; }
        }

        public bool IsSearchAlbumsByName { get; set; }
        public bool IsSearchAlbumsByTags { get; set; }
        public bool IsSearchAlbumsByDescription { get; set; }

        public bool IsSearchByAlbums
        {
            get { return IsSearchAlbumsByName || IsSearchAlbumsByTags || IsSearchAlbumsByDescription; }
        }

        public bool IsSearchUsersByName { get; set; }
        public bool IsSearchUserByDepartment { get; set; }

        public bool IsSearchByUser
        {
            get { return IsSearchUsersByName || IsSearchUserByDepartment; }
        }

        public bool IsSearchByComments { get; set; }
    }
}