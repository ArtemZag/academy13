using System.Configuration;
using BinaryStudio.PhotoGallery.Domain.Services.Tasks;
using FluentScheduler;

namespace BinaryStudio.PhotoGallery.Web.Registers
{
    public class SearchCacheRegistry : Registry
    {
        private readonly ISearchCacheTask searchCacheTask;

        private readonly int updatePeriod;

        public SearchCacheRegistry(ISearchCacheTask searchCacheTask)
        {
            updatePeriod = int.Parse(ConfigurationManager.AppSettings["SearchCachesUpdateMinutesPeriod"]);

            this.searchCacheTask = searchCacheTask;
            this.searchCacheTask.UpdatePeriod = updatePeriod;

            Register();
        }

        private void Register()
        {
            Schedule<ISearchCacheTask>().ToRunEvery(updatePeriod).Minutes();
        }

        public override ITask GetTaskInstance<T>()
        {
            return searchCacheTask;
        }
    }
}