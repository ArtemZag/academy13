namespace BinaryStudio.PhotoGallery.Database
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork GetUnitOfWork();
    }
}
