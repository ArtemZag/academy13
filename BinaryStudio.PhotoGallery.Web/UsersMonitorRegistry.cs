using System.Configuration;
using System.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Services;
using FluentScheduler;

namespace BinaryStudio.PhotoGallery.Web
{
    internal class UsersMonitorRegistry : Registry
    {
        private readonly IUsersMonitorTask usersMonitor;

        public UsersMonitorRegistry(IUsersMonitorTask usersMonitor)
        {
            this.usersMonitor = usersMonitor;

            Register();
        }

        private void Register()
        {
            int monitorPeriod = int.Parse(ConfigurationManager.AppSettings["UsersMonitorPeriod"]);

            Schedule<IUsersMonitorTask>().ToRunEvery(monitorPeriod).Minutes();
        }

        public override ITask GetTaskInstance<T>()
        {
            return usersMonitor;
        }
    }
}