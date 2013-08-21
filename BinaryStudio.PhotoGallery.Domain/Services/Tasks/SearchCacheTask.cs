using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BinaryStudio.PhotoGallery.Core;
using BinaryStudio.PhotoGallery.Domain.Services.Search;
using BinaryStudio.PhotoGallery.Domain.Services.Search.Results;

namespace BinaryStudio.PhotoGallery.Domain.Services.Tasks
{
    internal class SearchCacheTask : ISearchCacheTask
    {
        /// <summary>
        ///     After some minutes cache will be destroyed
        /// </summary>
        private const int TIME_FOR_CACHE_DESTROY = 2;

        private const int TOKEN_LENGTH = 10;

        /// <summary>
        ///     Describes token for cache and cache
        /// </summary>
        private readonly ConcurrentDictionary<string, SearchCache> caches =
            new ConcurrentDictionary<string, SearchCache>();

        /// <summary>
        ///     Time that will be appended to caches lifetime
        /// </summary>
        private int updatePeriod;

        public SearchCacheTask()
        {
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

        public SearchCache GetCache(string token, int skip, int take)
        {
            SearchCache cache = caches[token];

            var result = new SearchCache
            {
                Value = cache.Value.Skip(skip).Take(take),
                LifeTime = cache.LifeTime
            };

            return result;
        }

        public bool ContainsToken(string token)
        {
            return caches.ContainsKey(token);
        }

        public string AddCache(IEnumerable<IFound> cache)
        {
            string token = Randomizer.GetString(TOKEN_LENGTH);

            var searchCache = new SearchCache
            {
                Value = cache,
                LifeTime = 0
            };

            caches.TryAdd(token, searchCache);

            return token;
        }

        public void Execute()
        {
            AppendPeriod();

            UpdateCaches();
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
                SearchCache value;

                caches.TryRemove(token, out value);
            }
        }
    }
}