using System.Configuration;
using BinaryStudio.PhotoGallery.Domain.Services.Tasks;
using FluentScheduler;

namespace BinaryStudio.PhotoGallery.Web.Registers
{
    internal class CleanupRegistry : Registry
    {
        private readonly ICleanupTask cleanupTask;

        private readonly int dayFrequency;
        private readonly int hours;

        public CleanupRegistry(ICleanupTask cleanupTask)
        {
            dayFrequency = int.Parse(ConfigurationManager.AppSettings["CleanupDayFrequency"]);
            hours = int.Parse(ConfigurationManager.AppSettings["CleanupHour"]);

            this.cleanupTask = cleanupTask;

            Register();
        }

        private void Register()
        {
            Schedule<ICleanupTask>().ToRunEvery(dayFrequency).Days().At(hours, 0);
        }

        public override ITask GetTaskInstance<T>()
        {
            return cleanupTask;
        }
    }
}