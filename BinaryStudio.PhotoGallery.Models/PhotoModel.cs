using System;
using System.Collections.Generic;

namespace BinaryStudio.PhotoGallery.Models
{
    /// <summary>
    /// The class that represents photo.
    /// </summary>
    public class PhotoModel
    {
        // Initializes some properties, that must contain some value by default
        public PhotoModel()
        {
            DateOfCreation = DateTime.Now;
        }

        public PhotoModel(int albumModelId, int userModelId)
        {
            AlbumModelID = albumModelId;
            UserModelID = userModelId;
            DateOfCreation = DateTime.Now;
        }

        /// <summary>
        /// Gets or sets the photo ID.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets a path to photo.  
        /// todo: if we need more thumbnails - we should create a table of its.
        /// </summary>
        public string PhotoThumbSource { get; set; }

        /// <summary>
        /// Gets or sets the photo name.
        /// </summary>
        public string PhotoName { get; set; }

        /// <summary>
        /// Gets or sets the description of photo.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Time and date of photo creating
        /// </summary>
        public DateTime DateOfCreation { get; set; }

        /// <summary>
        /// Gets or sets the user's rating of each photo in album.
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Gets or sets a photo state
        /// </summary>
        public bool IsDeleted { get; set; }
 

        public int UserModelID { get; set; }
        public int AlbumModelID { get; set; }
        public virtual ICollection<PhotoCommentModel> PhotoComments { get; set; }
        public virtual ICollection<PhotoTagModel> PhotoTags { get; set; } 

    }
}