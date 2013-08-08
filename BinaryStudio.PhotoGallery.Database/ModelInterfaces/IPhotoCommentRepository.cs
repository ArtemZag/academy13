using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Database.ModelInterfaces
{
    public interface IPhotoCommentRepository : IBaseRepository<PhotoCommentModel>
    {
        /// <summary>
        /// Adds comment to photo
        /// </summary>
        /// <param name="ownerID">ID of user who is owner.</param>
        /// <param name="photoID">What photo is commened.</param>
        /// <param name="text">Text of comment.</param>
        /// <param name="repliedCommentID">What comment is replied. If null - just a comment without reply.</param>
        void Add(int ownerID, int photoID, string text, int repliedCommentID);
    }
}
