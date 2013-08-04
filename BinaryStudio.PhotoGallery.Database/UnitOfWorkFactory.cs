namespace BinaryStudio.PhotoGallery.Database
{
    internal class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        public IUnitOfWork GetUnitOfWork()
        {
            return new UnitOfWork();
        }
    }
}
