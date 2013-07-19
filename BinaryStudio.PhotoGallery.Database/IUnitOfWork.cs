using System;
using BinaryStudio.PhotoGallery.Database.ModelInterfaces;

namespace BinaryStudio.PhotoGallery.Database
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();

        IUserRepository Users { get; }
        IGroupRepository Groups { get; set; }
        IAvailableGroupRepository AvailableGroups { get; set; }
        IAuthInfoRepository AuthInfos { get; set; }
        IPhotoRepository Photos { get; set; }
        IPhotoCommentRepository PhotoComments { get; set; }
        IAlbumRepository Albums { get; set; }
    }
}
