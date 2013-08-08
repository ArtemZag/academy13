using System.Collections.Generic;
using System.Linq;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Services.Search.Items;

namespace BinaryStudio.PhotoGallery.Domain.Services.Search
{
    internal class SearchService : DbService, ISearchService
    {
        private readonly IPhotoSearchService photoSearchService;

        public SearchService(IUnitOfWorkFactory workFactory, IPhotoSearchService photoSearchService)
            : base(workFactory)
        {
            this.photoSearchService = photoSearchService;
        }

        public IEnumerable<IFoundItem> Search(SearchArguments searchArguments)
        {
            var result = new List<IFoundItem>();

            if (searchArguments.IsSearchByPhotos)
            {
                result.AddRange(photoSearchService.Search(searchArguments));
            }

            // todo: search by other types

            return
                result.Select(item => item)
                      .OrderBy(item => item.Relevance)
                      .Skip(searchArguments.Begin)
                      .Take(searchArguments.End);
        }
    }
}