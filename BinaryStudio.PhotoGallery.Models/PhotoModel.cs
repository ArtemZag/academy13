using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BinaryStudio.PhotoGallery.Models
{
    /// <summary>
    /// The class that represents photo.
    /// </summary>
    public class PhotoModel
    {
        /// <summary>
        /// Gets or sets the photo ID.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the photo name.
        /// </summary>
        public string PhotoName { get; set; }

        /// <summary>
        /// Gets or sets the description of photo.
        /// </summary>
        public string Decription { get; set; }

        /// <summary>
        /// Time and date of photo creating
        /// </summary>
        public DateTime  DataOfCreation { get; set; }

        /// <summary>
        /// Gets or sets the user's rating of each photo in album.
        /// </summary>
        public int Rating { get; set; }
 

        public int UserModelID { get; set; }
        public int AlbumModelID { get; set; }
        public virtual ICollection<PhotoCommentModel> PhotoComments { get; set; }

    }
}