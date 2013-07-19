using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryStudio.PhotoGallery.Database.ModelInterfaces;

namespace BinaryStudio.PhotoGallery.Database
{
    using BinaryStudio.PhotoGallery.Database.ModelRepositories;

    public class UnitOfWork : IUnitOfWork
    {
        private Lazy<IUserRepository> _usersLazy = new Lazy<IUserRepository>(() => new UserRepository(_dbContext));

        private static DatabaseContext _dbContext;



        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public IUserRepository Users { get
        {
            return _usersLazy.Value;
        } }
        public IGroupRepository Groups { get; set; }
        public IAvailableGroupRepository AvailableGroups { get; set; }
        public IAuthInfoRepository AuthInfos { get; set; }
        public IPhotoRepository Photos { get; set; }
        public IPhotoCommentRepository PhotoComments { get; set; }
        public IAlbumRepository Albums { get; set; }
    }
}
