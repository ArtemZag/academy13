using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Domain.Services.Search.Results;

namespace BinaryStudio.PhotoGallery.Domain.Services.Search
{
    public class SearchCache
    {
        public IEnumerable<IFound> Value { get; set; }

        public int LifeTime { get; set; }
    }
}