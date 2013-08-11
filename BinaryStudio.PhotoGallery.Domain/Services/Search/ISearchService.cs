using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Domain.Services.Search.FoundItems;
using FluentScheduler;

namespace BinaryStudio.PhotoGallery.Domain.Services.Search
{
    public interface ISearchService : ITask
    {
        IEnumerable<IFoundItem> Search(SearchArguments searchArguments);

        int UpdatePeriod { get; set; }
    }
}