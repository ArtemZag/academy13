using System;
using BinaryStudio.PhotoGallery.Database.ModelInterfaces;
using BinaryStudio.PhotoGallery.Database.ModelRepositories;

namespace BinaryStudio.PhotoGallery.Database
{
    public class UnitOfWork : IUnitOfWork
    {
        private static DatabaseContext _dbContext;

        private readonly Lazy<IUserRepository> _usersLazy =
            new Lazy<IUserRepository>(() => new UserRepository(_dbContext));


        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public IUserRepository Users
        {
            get { return _usersLazy.Value; }
        }

        public IGroupRepository Groups { get; set; }
        public IAvailableGroupRepository AvailableGroups { get; set; }
        public IAuthInfoRepository AuthInfos { get; set; }
        public IPhotoRepository Photos { get; set; }
        public IPhotoCommentRepository PhotoComments { get; set; }
        public IAlbumRepository Albums { get; set; }
    }
}