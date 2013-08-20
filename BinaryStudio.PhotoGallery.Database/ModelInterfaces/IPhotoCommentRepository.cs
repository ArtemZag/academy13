using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Database.ModelInterfaces
{
    public interface IPhotoCommentRepository : IBaseRepository<PhotoCommentModel>
    {
        /// <summary>
        /// Adds comment to photo
        /// </summary>
        /// <param name="ownerId">ID of user who is owner.</param>
        /// <param name="photoId">What photo is commened.</param>
        /// <param name="text">Text of comment.</param>
        /// <param name="repliedCommentId">What comment is replied. If null - just a comment without reply.</param>
        void Add(int ownerId, int photoId, string text, int repliedCommentId);
    }
}
