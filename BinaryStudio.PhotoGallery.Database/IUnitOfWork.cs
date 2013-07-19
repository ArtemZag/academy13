using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryStudio.PhotoGallery.Database.ModelInterfaces;

namespace BinaryStudio.PhotoGallery.Database
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();

        IUserRepository Users { get; set; }
        IGroupRepository Groups { get; set; }
        IAvailableGroupRepository AvailableGroups { get; set; }
        IAuthInfoRepository AuthInfos { get; set; }
        IPhotoRepository Photos { get; set; }
        IPhotoCommentRepository PhotoComments { get; set; }
        IAlbumRepository Albums { get; set; }
    }
}
