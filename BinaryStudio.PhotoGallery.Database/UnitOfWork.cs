using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryStudio.PhotoGallery.Database.ModelInterfaces;

namespace BinaryStudio.PhotoGallery.Database
{
    public class UnitOfWork : IUnitOfWork
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public int SaveChanges()
        {
            throw new NotImplementedException();
        }

        public IUserRepository Users { get; set; }
        public IGroupRepository Groups { get; set; }
        public IAvailableGroupRepository AvailableGroups { get; set; }
        public IAuthInfoRepository AuthInfos { get; set; }
        public IPhotoRepository Photos { get; set; }
        public IPhotoCommentRepository PhotoComments { get; set; }
        public IAlbumRepository Albums { get; set; }
    }
}
