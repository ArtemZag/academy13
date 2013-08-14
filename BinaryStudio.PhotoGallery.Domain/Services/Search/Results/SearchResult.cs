using System.Collections.Generic;

namespace BinaryStudio.PhotoGallery.Domain.Services.Search.Results
{
    public class SearchResult
    {
        public IEnumerable<IFound> Value { get; set; }

        public string CacheToken { get; set; }
    }
}
