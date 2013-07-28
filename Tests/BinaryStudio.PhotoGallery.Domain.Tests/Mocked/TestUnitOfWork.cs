using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Database.ModelInterfaces;
using BinaryStudio.PhotoGallery.Domain.Tests.Mocked.ModelRepositories;

namespace BinaryStudio.PhotoGallery.Domain.Tests.Mocked
{
    internal class TestUnitOfWork : IUnitOfWork
    {
        private readonly MockedContext mockedContext;
        private readonly IUserRepository usersRepository;

        public TestUnitOfWork(MockedContext mockedContext)
        {
            this.mockedContext = mockedContext;

            usersRepository = new TestUserRepository(mockedContext);
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

        public IGroupRepository Groups { get; private set; }
        public IAvailableGroupRepository AvailableGroups { get; private set; }
        public IAuthInfoRepository AuthInfos { get; private set; }
        public IPhotoRepository Photos { get; private set; }
        public IPhotoCommentRepository PhotoComments { get; private set; }
        public IAlbumRepository Albums { get; private set; }
        public IAlbumTagRepository AlbumTags { get; private set; }
        public IPhotoTagRepository PhotoTags { get; private set; }
    }
}