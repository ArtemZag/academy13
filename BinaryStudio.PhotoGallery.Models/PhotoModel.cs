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
        [Required(ErrorMessage = "PhotoName is required")]
        [StringLength(100, ErrorMessage = "PhotoName must contain at least 3 characters.", MinimumLength = 3)]
        public string PhotoName { get; set; }

        /// <summary>
        /// Gets or sets the description of photo.
        /// </summary>
        public string Decription { get; set; }

        [Required]
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