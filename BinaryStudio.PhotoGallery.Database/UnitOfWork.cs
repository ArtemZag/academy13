using System;
using BinaryStudio.PhotoGallery.Database.ModelInterfaces;
using BinaryStudio.PhotoGallery.Database.ModelRepositories;

namespace BinaryStudio.PhotoGallery.Database
{
    public class UnitOfWork : IUnitOfWork
    {
        private static DatabaseContext _databaseContext;


        private readonly Lazy<IUserRepository> _usersLazy =
            new Lazy<IUserRepository>(() => new UserRepository(_databaseContext));

        private readonly Lazy<IGroupRepository> _groupLazy =
            new Lazy<IGroupRepository>(() => new GroupRepository(_databaseContext));

        private readonly Lazy<IAvailableGroupRepository> _availableGroupLazy =
            new Lazy<IAvailableGroupRepository>(() => new AvailableGroupRepository(_databaseContext));

        private readonly Lazy<IAuthInfoRepository> _authInfoLazy =
            new Lazy<IAuthInfoRepository>(() => new AuthInfoRepository(_databaseContext));

        private readonly Lazy<IPhotoRepository> _photoLazy =
            new Lazy<IPhotoRepository>(() => new PhotoRepository(_databaseContext));

        private readonly Lazy<IPhotoCommentRepository> _photoCommentLazy =
            new Lazy<IPhotoCommentRepository>(() => new PhotoCommentRepository(_databaseContext));

        private readonly Lazy<IAlbumRepository> _albumLazy =
            new Lazy<IAlbumRepository>(() => new AlbumRepository(_databaseContext));

        private readonly Lazy<IPhotoTagRepository> _photoTagLazy =
            new Lazy<IPhotoTagRepository>(() => new PhotoTagRepository(_databaseContext));

        private readonly Lazy<IAlbumTagRepository> _albumTagLazy =
            new Lazy<IAlbumTagRepository>(() => new AlbumTagRepository(_databaseContext));


        /// <summary>
        /// Create unit of work with databaseContext
        /// </summary>
        public UnitOfWork()
        {
            _databaseContext = new DatabaseContext("BinaryStudio.PhotoGallery.Database.DatabaseContext");
        }

        /// <summary>
        /// Dispose databaseContext
        /// </summary>
        public void Dispose()
        {
            _databaseContext.Dispose();
        }

        /// <summary>
        /// Save changes to databaseContext
        /// </summary>
        public void SaveChanges()
        {
            _databaseContext.SaveChanges();
        }

        public IUserRepository Users
        {
            get { return this._usersLazy.Value; }
        }

        public IGroupRepository Groups
        {
            get { return this._groupLazy.Value; }
        }

        public IAvailableGroupRepository AvailableGroups
        {
            get { return this._availableGroupLazy.Value; }
        }

        public IAuthInfoRepository AuthInfos
        {
            get { return this._authInfoLazy.Value; }
        }

        public IPhotoRepository Photos
        {
            get { return this._photoLazy.Value; }
        }

        public IPhotoCommentRepository PhotoComments
        {
            get { return this._photoCommentLazy.Value; }
        }

        public IAlbumRepository Albums
        {
            get { return this._albumLazy.Value; }
        }

        public IAlbumTagRepository AlbumTags
        {
            get { return _albumTagLazy.Value; }
        }

        public IPhotoTagRepository PhotoTags
        {
            get { return _photoTagLazy.Value; }
        }
    }
}