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
            var comment = new PhotoCommentModel
                {
                    UserModelID = ownerID,
                    PhotoModelID = photoID,
                    Text = text,
                    Reply = repliedCommentID
                };
            base.Add(comment);
        }
    }
}
