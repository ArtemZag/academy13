namespace BinaryStudio.PhotoGallery.Database
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        public IUnitOfWork GetUnitOfWork()
        {
            return new UnitOfWork();
        }
    }
}
