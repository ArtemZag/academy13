using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Services.Search.Results;

namespace BinaryStudio.PhotoGallery.Domain.Services.Search
{
    internal class SearchService : DbService, ISearchService
    {
        /// <summary>
        ///     After some minutes cache will be destroyed
        /// </summary>
        private const int TIME_FOR_CACHE_DESTROY = 2;

        /// <summary>
        ///     Describes token for cache and cache
        /// </summary>
        private readonly ConcurrentDictionary<string, Cache> caches = new ConcurrentDictionary<string, Cache>();

        private readonly IPhotoSearchService photoSearchService;

        /// <summary>
        ///     Time that will be appended to caches lifetime
        /// </summary>
        private int updatePeriod;

        public SearchService(IUnitOfWorkFactory workFactory, IPhotoSearchService photoSearchService)
            : base(workFactory)
        {
            this.photoSearchService = photoSearchService;

            updatePeriod = 1;
        }

        public int UpdatePeriod
        {
            get { return updatePeriod; }
            set
            {
                if (value > 0)
                {
                    updatePeriod = value;
                }
            }
        }

        public SearchResult Search(SearchArguments searchArguments)
        {
            List<IFound> resultItems;

            string resultToken = searchArguments.CacheToken;

            if (IsTokenPresent(resultToken))
            {
                Cache cache = caches[resultToken];

                resultItems = cache.Value;
            }
            else
            {
                resultItems = new List<IFound>();

                if (searchArguments.IsSearchByPhotos)
                {
                    resultItems.AddRange(photoSearchService.Search(searchArguments));
                }

                resultToken = string.Empty;
                // todo: search by other types
                // todo: add resultItems to all caches
            }

            return new SearchResult
                {
                    Value = TakeInterval(resultItems, searchArguments.Begin, searchArguments.End),
                    Token = resultToken
                };
        }

        /// <summary>
        ///     Sheldule operation
        /// </summary>
        public void Execute()
        {
            AppendPeriod();

            UpdateCaches();
        }

        private bool IsTokenPresent(string token)
        {
            return caches.ContainsKey(token);
        }

        private void AppendPeriod()
        {
            foreach (string token in caches.Keys)
            {
                caches[token].LifeTime += updatePeriod;
            }
        }

        private void UpdateCaches()
        {
            var cachesToRemove = new Collection<string>();

            foreach (string token in caches.Keys)
            {
                if (caches[token].LifeTime >= TIME_FOR_CACHE_DESTROY)
                {
                    cachesToRemove.Add(token);
                }
            }

            foreach (string token in cachesToRemove)
            {
                Cache value;

                caches.TryRemove(token, out value);
            }
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