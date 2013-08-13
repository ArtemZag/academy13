﻿using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using BinaryStudio.PhotoGallery.Domain.Services.Search;

namespace BinaryStudio.PhotoGallery.Domain.Services.Tasks
{
    internal class SearchCacheTask : ISearchCacheTask
    {
        /// <summary>
        ///     After some minutes cache will be destroyed
        /// </summary>
        private const int TIME_FOR_CACHE_DESTROY = 2;

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

        public SearchCache GetCache(string token)
        {
            return caches[token];
        }

        public bool ContainsToken(string token)
        {
            return caches.ContainsKey(token);
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