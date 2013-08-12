using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Domain.Services.Search.Results;

namespace BinaryStudio.PhotoGallery.Domain.Services.Search
{
    internal class Cache
    {
        public List<IFound> Value { get; set; }

        public int LifeTime { get; set; }
    }
}
