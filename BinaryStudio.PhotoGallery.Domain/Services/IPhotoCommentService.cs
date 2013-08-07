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
        /// <param name="photoID">ID of photo with comments</param>
        /// <param name="begin">Beginning of the interval.</param>
        /// <param name="end">Ending of the interval.</param>
        IEnumerable<PhotoCommentModel> GetPhotoComments(int photoID, int begin, int last);

        void AddPhotoComment(PhotoCommentModel newPhotoCommentModel);
    }
}
