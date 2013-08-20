using System;
using BinaryStudio.PhotoGallery.Database.ModelInterfaces;
using BinaryStudio.PhotoGallery.Database.ModelRepositories;

namespace BinaryStudio.PhotoGallery.Database
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext databaseContext;


        private readonly Lazy<IUserRepository> usersLazy;

        private readonly Lazy<IGroupRepository> groupLazy;

        private readonly Lazy<IAvailableGroupRepository> availableGroupLazy;

        private readonly Lazy<IAuthInfoRepository> authInfoLazy;

        private readonly Lazy<IPhotoRepository> photoLazy;

        private readonly Lazy<IPhotoCommentRepository> photoCommentLazy;

        private readonly Lazy<IAlbumRepository> albumLazy;

        private readonly Lazy<IPhotoTagRepository> photoTagLazy;

        private readonly Lazy<IAlbumTagRepository> albumTagLazy;


        /// <summary>
        /// Create unit of work with databaseContext
        /// </summary>
        public UnitOfWork()
        {
            databaseContext = new DatabaseContext("BinaryStudio.PhotoGallery.Database.DatabaseContext");
            
            albumTagLazy = new Lazy<IAlbumTagRepository>(() => new AlbumTagRepository(databaseContext));
            photoTagLazy = new Lazy<IPhotoTagRepository>(() => new PhotoTagRepository(databaseContext));
            albumLazy = new Lazy<IAlbumRepository>(() => new AlbumRepository(databaseContext));
            photoCommentLazy = new Lazy<IPhotoCommentRepository>(() => new PhotoCommentRepository(databaseContext));
            photoLazy = new Lazy<IPhotoRepository>(() => new PhotoRepository(databaseContext));
            authInfoLazy = new Lazy<IAuthInfoRepository>(() => new AuthInfoRepository(databaseContext));
            availableGroupLazy = new Lazy<IAvailableGroupRepository>(() => new AvailableGroupRepository(databaseContext));
            groupLazy = new Lazy<IGroupRepository>(() => new GroupRepository(databaseContext));
            usersLazy = new Lazy<IUserRepository>(() => new UserRepository(databaseContext));
            
        }

        /// <summary>
        /// Dispose databaseContext
        /// </summary>
        public void Dispose()
        {
            databaseContext.Dispose();
        }

        /// <summary>
        /// Save changes to databaseContext
        /// </summary>
        public void SaveChanges()
        {
            databaseContext.SaveChanges();
        }

        public IUserRepository Users
        {
            get { return this.usersLazy.Value; }
        }

        public IGroupRepository Groups
        {
            get { return this.groupLazy.Value; }
        }

        public IAvailableGroupRepository AvailableGroups
        {
            get { return this.availableGroupLazy.Value; }
        }

        public IAuthInfoRepository AuthInfos
        {
            get { return this.authInfoLazy.Value; }
        }

        public IPhotoRepository Photos
        {
            get { return this.photoLazy.Value; }
        }

        public IPhotoCommentRepository PhotoComments
        {
            get { return this.photoCommentLazy.Value; }
        }

        public IAlbumRepository Albums
        {
            get { return this.albumLazy.Value; }
        }

        public IAlbumTagRepository AlbumTags
        {
            get { return albumTagLazy.Value; }
        }

        public IPhotoTagRepository PhotoTags
        {
            get { return photoTagLazy.Value; }
        }
    }
}