﻿using System.Collections.Generic;
using System.Linq;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Services.Search.Results;
using BinaryStudio.PhotoGallery.Domain.Services.Tasks;

namespace BinaryStudio.PhotoGallery.Domain.Services.Search
{
    internal class SearchService : DbService, ISearchService
    {
        private readonly IAlbumSearchService albumSearchService;
        private readonly ICommentSearchService commentSearchService;
        private readonly IPhotoSearchService photoSearchService;
        private readonly IUserSearchService userSearchService;

        private readonly ISearchCacheTask searchCacheTask;

        public SearchService(IUnitOfWorkFactory workFactory, IPhotoSearchService photoSearchService,
            IUserSearchService userSearchService, IAlbumSearchService albumSearchService,
            ICommentSearchService commentSearchService,
            ISearchCacheTask searchCacheTask)
            : base(workFactory)
        {
            this.photoSearchService = photoSearchService;
            this.userSearchService = userSearchService;
            this.albumSearchService = albumSearchService;
            this.commentSearchService = commentSearchService;

            this.searchCacheTask = searchCacheTask;
        }

        public SearchResult Search(SearchArguments searchArguments)
        {
            var result = new List<IFound>();

            string resultToken = searchArguments.SearchCacheToken;

            if (searchCacheTask.ContainsToken(resultToken))
            {
                SearchCache searchCache = searchCacheTask.GetCache(resultToken);

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

                if (searchArguments.IsSearchByComments)
                {
                    result.AddRange(commentSearchService.Search(searchArguments));
                }

                result = result.OrderByDescending(found => found.Relevance).ToList();

                resultToken = searchCacheTask.AddCache(result);
            }

            return new SearchResult
            {
                Value = result.Skip(searchArguments.Skip).Take(searchArguments.Take),
                SearchCacheToken = resultToken
            };
        }
    }
}