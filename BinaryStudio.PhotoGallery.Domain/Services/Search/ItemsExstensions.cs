using System.Collections.Generic;
using System.Linq;
using BinaryStudio.PhotoGallery.Domain.Services.Search.Results;

namespace BinaryStudio.PhotoGallery.Domain.Services.Search
{
    public static class ItemsExtensions
    {
        /// <summary>
        /// Removes first N elements
        /// </summary>
        public static IEnumerable<IFound> RemoveElements(this IEnumerable<IFound> items, int interval)
        {
            List<IFound> result = items.OrderByDescending(found => found.Relevance).ToList();

            if (interval > result.Count)
            {
                interval = result.Count;
            }

            result.RemoveRange(0, interval);

            return result;
        }

        public static IEnumerable<IFound> TakeInterval(this IEnumerable<IFound> items, int interval)
        {
            return items.OrderByDescending(found => found.Relevance).Take(interval);            
        }
    }
}
