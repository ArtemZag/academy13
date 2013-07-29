using System.Configuration;
using System.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Services;
using FluentScheduler;

namespace BinaryStudio.PhotoGallery.Web
{
    internal class CleanupServiceRegistry : Registry
    {
        private readonly ICleanupTask cleanupService;

        public CleanupServiceRegistry()
        {
            cleanupService = DependencyResolver.Current.GetService<ICleanupTask>();

            Register();
        }

        private void Register()
        {
            int dayFrequency = int.Parse(ConfigurationManager.AppSettings["PhotoCleanupDayFrequency"]);
            int hours = int.Parse(ConfigurationManager.AppSettings["PhotoCleanupHour"]);

            Schedule<ICleanupTask>().ToRunEvery(dayFrequency).Days().At(hours, 0);
        }

        public override ITask GetTaskInstance<T>()
        {
            return cleanupService;
        }
    }
}