using System.Collections.Generic;

namespace BinaryStudio.PhotoGallery.Domain.Services.Search.Items
{
    public interface IPhotoSearchService
    {
        IEnumerable<IFoundItem> Search(SearchArguments searchArguments);
    }
}
