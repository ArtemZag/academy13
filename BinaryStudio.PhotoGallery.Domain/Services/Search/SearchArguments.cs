namespace BinaryStudio.PhotoGallery.Domain.Services.Search
{
    public class SearchArguments
    {
        public string SearchQuery { get; set; }

        public int Begin { get; set; }
        public int End { get; set; }

        public bool IsSearchUsersByName { get; set; }
        public bool IsSearchUserByDepartment { get; set; }
        public bool IsSearchUserByNickname { get; set; }

        public bool IsSearchPhotosByName { get; set; }
        public bool IsSearchPhotosByTags { get; set; }
        public bool IsSearchPhotosByDescription { get; set; }

        public bool IsSearchAlbumsByName { get; set; }
        public bool IsSearchAlbumsByTags { get; set; }
        public bool IsSearchAlbumsByDescription { get; set; }

        public bool IsSearchByComments { get; set; }
    }
}