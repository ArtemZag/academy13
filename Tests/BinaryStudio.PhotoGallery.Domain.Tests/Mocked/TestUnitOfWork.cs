using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Database.ModelInterfaces;
using BinaryStudio.PhotoGallery.Domain.Tests.Mocked.ModelRepositories;

namespace BinaryStudio.PhotoGallery.Domain.Tests.Mocked
{
    internal class TestUnitOfWork : IUnitOfWork
    {
        private readonly IAlbumRepository albumRepository;
        private readonly MockedContext mockedContext;
        private readonly IPhotoRepository photoRepository;
        private readonly IUserRepository usersRepository;

        public TestUnitOfWork(MockedContext mockedContext)
        {
            this.mockedContext = mockedContext;

            usersRepository = new TestUserRepository(mockedContext);
            albumRepository = new TestAlbumRepository(mockedContext);
            photoRepository = new TestPhotoRepository(mockedContext);
        }

        public void Dispose()
        {
            // NOP
        }

        public void SaveChanges()
        {
            // NOP
        }

        public IUserRepository Users
        {
            get { return usersRepository; }
        }

        public IAlbumRepository Albums
        {
            get { return albumRepository; }
        }

        public IPhotoRepository Photos
        {
            get { return photoRepository; }
        }

        public IGroupRepository Groups { get; private set; }
        public IAvailableGroupRepository AvailableGroups { get; private set; }
        public IAuthInfoRepository AuthInfos { get; private set; }
        public IPhotoCommentRepository PhotoComments { get; private set; }
        public IAlbumTagRepository AlbumTags { get; private set; }
        public IPhotoTagRepository PhotoTags { get; private set; }
    }
}