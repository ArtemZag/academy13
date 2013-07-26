using System.Configuration;
using System.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Services;
using FluentScheduler;

namespace BinaryStudio.PhotoGallery.Web
{
    internal class CleanupServiceRegistry : Registry
    {
        private readonly IPhotoCleanupService cleanupService;

        public CleanupServiceRegistry()
        {
            cleanupService = DependencyResolver.Current.GetService<IPhotoCleanupService>();

            Register();
        }

        private void Register()
        {
            int dayFrequency = int.Parse(ConfigurationManager.AppSettings["PhotoCleanupDayFrequency"]);
            int hours = int.Parse(ConfigurationManager.AppSettings["PhotoCleanupHour"]);

            Schedule<IPhotoCleanupService>().ToRunEvery(dayFrequency).Days().At(hours, 0);
        }

        public override ITask GetTaskInstance<T>()
        {
            return cleanupService;
        }
    }
}