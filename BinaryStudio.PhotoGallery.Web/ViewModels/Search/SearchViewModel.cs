namespace BinaryStudio.PhotoGallery.Web.ViewModels.Search
{
    public class SearchViewModel
    {
        public enum SearchType
        {
            Photos,
            Albums,
            Users,
            Comments
        }

        public string SearchQuery { get; set; }

        public SearchType Type { get; set; }

        public int Begin { get; set; }

        public int End { get; set; }
    }
}