using System.Configuration;
using System.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Services;
using FluentScheduler;

namespace BinaryStudio.PhotoGallery.Web
{
    internal class UsersMonitorRegistry : Registry
    {
        private readonly IUsersMonitorTask usersMonitor;

        private readonly int monitorPeriod;

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