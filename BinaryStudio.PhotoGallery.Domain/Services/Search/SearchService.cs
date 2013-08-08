using System;
using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Database;

namespace BinaryStudio.PhotoGallery.Domain.Services.Search
{
    internal class SearchService : DbService, ISearchService
    {
        public SearchService(IUnitOfWorkFactory workFactory) : base(workFactory)
        {
        }

        public IEnumerable<IFoundItem> Serach(SearchArguments searchArguments)
        {
            throw new NotImplementedException();
        }
    }
}