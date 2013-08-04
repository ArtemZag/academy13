using System.Configuration;
using BinaryStudio.PhotoGallery.Domain.Services;
using FluentScheduler;

namespace BinaryStudio.PhotoGallery.Web.Registers
{
    internal class UsersMonitorRegistry : Registry
    {
        private readonly int monitorPeriod;
        private readonly IUsersMonitorTask usersMonitor;

        public UsersMonitorRegistry(IUsersMonitorTask usersMonitor)
        {
            monitorPeriod = int.Parse(ConfigurationManager.AppSettings["UsersMonitorMinutesPeriod"]);

            this.usersMonitor = usersMonitor;
            this.usersMonitor.Period = monitorPeriod;

            Register();
        }

        private void Register()
        {
            Schedule<IUsersMonitorTask>().ToRunEvery(monitorPeriod).Minutes();
        }

        public override ITask GetTaskInstance<T>()
        {
            return usersMonitor;
        }
    }
}