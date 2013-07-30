using System.Configuration;
using System.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Services;
using FluentScheduler;

namespace BinaryStudio.PhotoGallery.Web
{
    internal class CleanupRegistry : Registry
    {
        private readonly ICleanupTask cleanupTask;

        public CleanupRegistry()
        {
            cleanupTask = DependencyResolver.Current.GetService<ICleanupTask>();

            Register();
        }

        private void Register()
        {
            int dayFrequency = int.Parse(ConfigurationManager.AppSettings["CleanupDayFrequency"]);
            int hours = int.Parse(ConfigurationManager.AppSettings["CleanupHour"]);

            Schedule<ICleanupTask>().ToRunEvery(dayFrequency).Days().At(hours, 0);
        }

        public override ITask GetTaskInstance<T>()
        {
            return cleanupTask;
        }
    }
}