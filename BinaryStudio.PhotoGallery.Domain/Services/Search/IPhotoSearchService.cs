using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Domain.Services.Search.Results;

namespace BinaryStudio.PhotoGallery.Domain.Services.Search
{
    public interface IPhotoSearchService
    {
        IEnumerable<IFound> Search(SearchArguments searchArguments);
    }
}