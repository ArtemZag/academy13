using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    public interface IPhotoCommentService
    {
        /// <summary>
        /// Returns specified interval of photos (sorted by date) from specified album.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="photoID">ID of photo with comments</param>
        /// <param name="begin">Beginning of the interval.</param>
        /// <param name="end"></param>
        IEnumerable<PhotoCommentModel> GetPhotoComments(int userID, int photoID, int begin, int end);

        /// <summary>
        /// Adds photo comment to photo
        /// </summary>
        /// <param name="userID">Who wants to add comment</param>
        /// <param name="newPhotoCommentModel"></param>
        void AddPhotoComment(int userID, PhotoCommentModel newPhotoCommentModel);

        PhotoCommentModel GetPhotoComment(int commentId);
    }
}
