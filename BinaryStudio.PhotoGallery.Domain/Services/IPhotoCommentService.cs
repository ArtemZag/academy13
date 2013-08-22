using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    public interface IPhotoCommentService
    {
        /// <summary>
        /// Returns specified interval of photos (sorted by date) from specified album.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="photoId">ID of photo with comments</param>
        /// <param name="begin">Beginning of the interval.</param>
        /// <param name="end"></param>
        IEnumerable<PhotoCommentModel> GetPhotoComments(int userId, int photoId, int begin, int end);

        /// <summary>
        /// Adds photo comment to photo
        /// </summary>
        /// <param name="userId">Who wants to add comment</param>
        /// <param name="newPhotoCommentModel"></param>
        void AddPhotoComment(int userId, PhotoCommentModel newPhotoCommentModel);

        PhotoCommentModel GetPhotoComment(int commentId);
    }
}
