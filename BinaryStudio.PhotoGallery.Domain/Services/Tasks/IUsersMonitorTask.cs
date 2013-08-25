using FluentScheduler;

namespace BinaryStudio.PhotoGallery.Domain.Services.Tasks
{
    public interface IUsersMonitorTask : ITask
    {
        /// <summary>
        ///     Update period in minutes of users statuses.
        /// </summary>
        int Period { get; set; }

        /// <summary>
        ///     Marks and updates status of specified user as online.
        /// </summary>
        void SetOnline(int userId);

        /// <summary>
        ///     Marks status of specified user as offline.
        /// </summary>
        void SetOffline(int userId);

        /// <summary>
        ///     Returns true if specified user online or false if not.
        /// </summary>
        bool IsOnline(int userId);
    }
}