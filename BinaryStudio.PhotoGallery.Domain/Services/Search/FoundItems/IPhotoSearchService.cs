using System.Collections.Generic;

namespace BinaryStudio.PhotoGallery.Domain.Services.Search.FoundItems
{
    public interface IPhotoSearchService
    {
        IEnumerable<IFoundItem> Search(SearchArguments searchArguments);
    }
}
