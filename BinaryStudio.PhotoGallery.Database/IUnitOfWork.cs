using System;
using BinaryStudio.PhotoGallery.Database.ModelInterfaces;

namespace BinaryStudio.PhotoGallery.Database
{
    public interface IUnitOfWork : IDisposable
    {
        void SaveChanges();

        IUserRepository Users { get; }
        IGroupRepository Groups { get; }
        IAvailableGroupRepository AvailableGroups { get; }
        IAuthInfoRepository AuthInfos { get; }
        IPhotoRepository Photos { get; }
        IPhotoCommentRepository PhotoComments { get; }
        IAlbumRepository Albums { get; }
    }
}
