using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryStudio.PhotoGallery.Database.Model_Interfaces;

namespace BinaryStudio.PhotoGallery.Database
{
    interface IRepository : IUnitOfWork
    {
        IUserRepository Users { get; }
        IGroupRepository Groups { get; }
        IAvailableGroupRepository AvailableGroups { get; }
        IAuthInfoRepository AuthInfos { get; }
        IPhotoRepository Photos { get; }
        IPhotoCommentRepository PhotoComments { get; }
        IAlbumRepository Albums { get; }
    }
}
