using System.Configuration;
using BinaryStudio.PhotoGallery.Domain.Services.Tasks;
using FluentScheduler;

namespace BinaryStudio.PhotoGallery.Web.Registers
{
    internal class UsersMonitorRegistry : Registry
    {
        private readonly IUsersMonitorTask usersMonitor;

        private readonly int updatePeriod;

        public UsersMonitorRegistry(IUsersMonitorTask usersMonitor)
        {
            updatePeriod = int.Parse(ConfigurationManager.AppSettings["UsersMonitorMinutesPeriod"]);

            this.usersMonitor = usersMonitor;
            this.usersMonitor.Period = updatePeriod;

            Register();
        }

        private void Register()
        {
            Schedule<IUsersMonitorTask>().ToRunEvery(updatePeriod).Minutes();
        }

        public override ITask GetTaskInstance<T>()
        {
            return usersMonitor;
        }
    }
}