using BinaryStudio.PhotoGallery.Database.ModelInterfaces;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Database.ModelRepositories
{
    internal class PhotoCommentRepository : BaseRepository<PhotoCommentModel>, IPhotoCommentRepository
    {
        public PhotoCommentRepository(DatabaseContext dataBaseContext)
            : base(dataBaseContext)
        {
        }

        public void Add(int ownerID, int photoID, string text, PhotoCommentModel repliedCommentID)
        {
            base.Add(new PhotoCommentModel(ownerID, photoID, text, repliedCommentID));
        }
    }
}
