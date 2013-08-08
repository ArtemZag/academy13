using System;
using System.Collections.Generic;
using System.Linq;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Services.Search.Items;

namespace BinaryStudio.PhotoGallery.Domain.Services.Search
{
    internal class SearchService : DbService, ISearchService
    {
        private IPhotoSearchService photoSearchService;

        public SearchService(IUnitOfWorkFactory workFactory, IPhotoSearchService photoSearchService)
            : base(workFactory)
        {
            this.photoSearchService = photoSearchService;
        }

        public IEnumerable<IFoundItem> Search(SearchArguments searchArguments)
        {
            throw new NotImplementedException();
        }
    }
}