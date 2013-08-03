using BinaryStudio.PhotoGallery.Database;

namespace BinaryStudio.PhotoGallery.Domain.Tests.Mocked
{
    internal class TestUnitOfWorkFactory : IUnitOfWorkFactory
    {
        private static readonly MockedContext mockedContext = new MockedContext();

        public IUnitOfWork GetUnitOfWork()
        {
            return new TestUnitOfWork(mockedContext);
        }
    }
}