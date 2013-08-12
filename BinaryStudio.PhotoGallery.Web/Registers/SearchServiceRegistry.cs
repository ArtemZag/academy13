using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using BinaryStudio.PhotoGallery.Domain.Services.Search;
using FluentScheduler;

namespace BinaryStudio.PhotoGallery.Web.Registers
{
    public class SearchServiceRegistry : Registry
    {
        private readonly ISearchService searchService;

        private readonly int updatePeriod;

        public SearchServiceRegistry(ISearchService searchService)
        {
            updatePeriod = int.Parse(ConfigurationManager.AppSettings["SearchCachesUpdateMinutesPeriod"]);

            this.searchService = searchService;
            this.searchService.UpdatePeriod = updatePeriod;

            Register();
        }

        private void Register()
        {
            Schedule<ISearchService>().ToRunEvery(updatePeriod).Minutes();
        }

        public override ITask GetTaskInstance<T>()
        {
            return searchService;
        }    }
}