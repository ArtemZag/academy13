using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Domain.Services.Search.FoundItems;

namespace BinaryStudio.PhotoGallery.Domain.Services.Search
{
    internal class Cache
    {
        public List<IFoundItem> Value { get; set; }

        public int LifeTime { get; set; }
    }
}
