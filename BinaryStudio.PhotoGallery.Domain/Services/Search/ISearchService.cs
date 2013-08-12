using BinaryStudio.PhotoGallery.Domain.Services.Search.Results;

namespace BinaryStudio.PhotoGallery.Domain.Services.Search
{
    public interface ISearchService
    {
        SearchResult Search(SearchArguments searchArguments);
    }
}