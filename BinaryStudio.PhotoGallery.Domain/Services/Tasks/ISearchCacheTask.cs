using BinaryStudio.PhotoGallery.Domain.Services.Search;
using FluentScheduler;

namespace BinaryStudio.PhotoGallery.Domain.Services.Tasks
{
    public interface ISearchCacheTask : ITask
    {
        int UpdatePeriod { get; set; }

        SearchCache GetCache(string token);

        bool ContainsToken(string token);
    }
}
