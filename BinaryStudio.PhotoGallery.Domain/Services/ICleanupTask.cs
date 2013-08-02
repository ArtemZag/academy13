using BinaryStudio.PhotoGallery.Domain.Utils;
using FluentScheduler;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    public interface ICleanupTask : ITask
    {
        /// <summary>
        /// For tests.
        /// </summary>
        IStorage Storage { set; }
    }
}
