using System.Collections.Generic;

namespace BinaryStudio.PhotoGallery.Domain.Services.Search
{
    public interface ISearchService
    {
        IEnumerable<IFoundItem> Serach(SearchArguments searchArguments);
    }
}