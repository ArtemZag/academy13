using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryStudio.PhotoGallery.Database.Model_Interfaces;

namespace BinaryStudio.PhotoGallery.Database
{
    class Repository : IRepository
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public int SaveChanges()
        {
            throw new NotImplementedException();
        }

        public IUserRepository Users { get; private set; }
        public IGroupRepository Groups { get; private set; }
        public IAvailableGroupRepository AvailableGroups { get; private set; }
        public IAuthInfoRepository AuthInfos { get; private set; }
        public IPhotoRepository Photos { get; private set; }
        public IPhotoCommentRepository PhotoComments { get; private set; }
        public IAlbumRepository Albums { get; private set; }
    }
}
