using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Domain.Services.Search.Items;

namespace BinaryStudio.PhotoGallery.Domain.Services.Search
{
    public interface ISearchService
    {
        IEnumerable<IFoundItem> Search(SearchArguments searchArguments);
    }
}