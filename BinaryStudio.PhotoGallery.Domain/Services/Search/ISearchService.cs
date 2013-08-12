using BinaryStudio.PhotoGallery.Domain.Services.Search.Results;
using FluentScheduler;

namespace BinaryStudio.PhotoGallery.Domain.Services.Search
{
    public interface ISearchService : ITask
    {
        SearchResult Search(SearchArguments searchArguments);

        int UpdatePeriod { get; set; }
    }
}