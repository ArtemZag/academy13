using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Domain.Services.Search.Results;

namespace BinaryStudio.PhotoGallery.Domain.Services.Search
{
    internal interface IUserSearchService
    {
        IEnumerable<IFound> Search(SearchArguments searchArguments);
    }
}
