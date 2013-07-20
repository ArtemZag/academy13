using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    internal class AlbumService : Service, IAlbumService
    {
        public AlbumService(IUnitOfWorkFactory workFactory) : base(workFactory)
        {
        }

        public bool CreateAlbum(AlbumModel album)
        {
            try
            {
                using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
                {
                    unitOfWork.Albums.Create(album);
                    unitOfWork.SaveChanges();
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool UpdateAlbum(AlbumModel album)
        {
            try
            {
                using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
                {
                    unitOfWork.Albums.Update(album);
                    unitOfWork.SaveChanges();
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool DeleteAlbum(AlbumModel album)
        {
            try
            {
                using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
                {
                    unitOfWork.Albums.Delete(album);
                    unitOfWork.SaveChanges();
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
