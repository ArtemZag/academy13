using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Domain.Services.Search;
using BinaryStudio.PhotoGallery.Domain.Services.Search.Results;
using FluentScheduler;

namespace BinaryStudio.PhotoGallery.Domain.Services.Tasks
{
    public interface ISearchCacheTask : ITask
    {
        int UpdatePeriod { get; set; }

        SearchCache GetCache(string token);

        bool ContainsToken(string token);

        string AddCache(IEnumerable<IFound> cache);
    }
}
