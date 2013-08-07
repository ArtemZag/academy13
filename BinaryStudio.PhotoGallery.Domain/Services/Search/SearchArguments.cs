namespace BinaryStudio.PhotoGallery.Domain.Services.Search
{
    public class SearchArguments
    {
        public string SearcQuery { get; set; }

        public int Begin { get; set; }
        public int End { get; set; }

        public bool SearchByPhotos { get; set; }
        public bool SearchByAlbums { get; set; }
        public bool SearchByUsers { get; set; }
        public bool SearchByComments { get; set; }
    }
}