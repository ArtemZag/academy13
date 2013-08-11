using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Services.Search.FoundItems;

namespace BinaryStudio.PhotoGallery.Domain.Services.Search
{
    internal class SearchService : DbService, ISearchService
    {
        private readonly IPhotoSearchService photoSearchService;

        /// <summary>
        /// Describes token for cache and cache
        /// </summary>
        private readonly ConcurrentDictionary<string, Cache> caches = new ConcurrentDictionary<string, Cache>(); 

        public SearchService(IUnitOfWorkFactory workFactory, IPhotoSearchService photoSearchService)
            : base(workFactory)
        {
            this.photoSearchService = photoSearchService;
        }

        public IEnumerable<IFoundItem> Search(SearchArguments searchArguments)
        {
            List<IFoundItem> result;

            string cacheToken = searchArguments.CacheToken;

            if (cacheToken.Equals(string.Empty))
            {
                result = new List<IFoundItem>();

                if (searchArguments.IsSearchByPhotos)
                {
                    result.AddRange(photoSearchService.Search(searchArguments));
                }

                // todo: search by other types
                // todo: add cache to all caches
            }
            else
            {
                Cache cache = caches[cacheToken];

                result = cache.Value;
            }

            return TakeInterval(result, searchArguments.Begin, searchArguments.End);
        }

        private IEnumerable<IFoundItem> TakeInterval(IEnumerable<IFoundItem> data, int begin, int end)
        {
            return
                data.Select(item => item)
                      .OrderBy(item => item.Relevance)
                      .Skip(begin)
                      .Take(end);
        }
    }
}