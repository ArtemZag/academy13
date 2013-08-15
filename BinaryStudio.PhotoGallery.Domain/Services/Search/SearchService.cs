using System.Collections.Generic;
using System.Linq;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Services.Search.Results;
using BinaryStudio.PhotoGallery.Domain.Services.Tasks;

namespace BinaryStudio.PhotoGallery.Domain.Services.Search
{
    internal class SearchService : DbService, ISearchService
    {
        private readonly IPhotoSearchService photoSearchService;

        private readonly ISearchCacheTask searchCacheTask;

        public SearchService(IUnitOfWorkFactory workFactory, IPhotoSearchService photoSearchService,
            ISearchCacheTask searchCacheTask)
            : base(workFactory)
        {
            this.photoSearchService = photoSearchService;
            this.searchCacheTask = searchCacheTask;
        }

        public SearchResult Search(SearchArguments searchArguments)
        {
            var resultItems = new List<IFound>();

            string resultToken = searchArguments.SearchCacheToken;

            if (searchCacheTask.ContainsToken(resultToken))
            {
                SearchCache searchCache = searchCacheTask.GetCache(resultToken);

                resultItems.AddRange(searchCache.Value);
            }
            else
            {
                if (searchArguments.IsSearchByPhotos)
                {
                    resultItems.AddRange(photoSearchService.Search(searchArguments));
                }

                // todo: search by other types
                resultToken = searchCacheTask.AddCache(resultItems);
            }

            return new SearchResult
            {
                Value = TakeInterval(resultItems, searchArguments.Begin, searchArguments.End),
                SearchCacheToken = resultToken
            };
        }

        private IEnumerable<IFound> TakeInterval(IEnumerable<IFound> data, int begin, int end)
        {
            return
                data.Select(item => item)
                    .OrderBy(item => item.Relevance)
                    .Skip(begin)
                    .Take(end);
        }
    }
}