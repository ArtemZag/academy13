using FluentScheduler;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    public interface IUsersMonitorTask : ITask
    {
        /// <summary>
        /// Marks and updates status of specified user as online.
        /// </summary>
        void SetOnline(string userEmail);

        /// <summary>
        /// Marks status of specified user as offline. 
        /// </summary>
        void SetOffline(string userEmail);

        /// <summary>
        /// Returns true if specified user online or false if not. 
        /// </summary>
        bool IsOnline(string userEmail);
    }
}
