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
        private readonly IUserSearchService userSearchService;
        private readonly IAlbumSearchService albumSearchService;

        public SearchService(IUnitOfWorkFactory workFactory, IPhotoSearchService photoSearchService,
            IUserSearchService userSearchService, IAlbumSearchService albumSearchService, ISearchCacheTask searchCacheTask)
            : base(workFactory)
        {
            this.photoSearchService = photoSearchService;
            this.userSearchService = userSearchService;
            this.albumSearchService = albumSearchService;
            this.searchCacheTask = searchCacheTask;
        }

        public SearchResult Search(SearchArguments searchArguments)
        {
            var result = new List<IFound>();

            string resultToken = searchArguments.SearchCacheToken;

            if (searchCacheTask.ContainsToken(resultToken))
            {
                SearchCache searchCache = searchCacheTask.DeductCache(resultToken, searchArguments.Interval);

                result.AddRange(searchCache.Value);
            }
            else
            {
                if (searchArguments.IsSearchByPhotos)
                {
                    result.AddRange(photoSearchService.Search(searchArguments));
                }

                if (searchArguments.IsSearchByUsers)
                {
                    result.AddRange(userSearchService.Search(searchArguments));
                }

                if (searchArguments.IsSearchByAlbums)
                {
                    result.AddRange(albumSearchService.Search(searchArguments));
                }

                // todo: search by comments

                resultToken = searchCacheTask.AddCache(result.RemoveElements(searchArguments.Interval));
                result = result.TakeInterval(searchArguments.Interval).ToList();
            }

            return new SearchResult
            {
                Value = result,
                SearchCacheToken = resultToken
            };
        }
    }
}