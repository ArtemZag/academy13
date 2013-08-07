using System.Collections.Generic;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    public interface ISearchService
    {
        IEnumerable<IFoundItem> Serach(SearchArguments searchArguments);
    }
}
