using System;
using System.Collections.Generic;

namespace BinaryStudio.PhotoGallery.Models
{
    /// <summary>
    /// The class that represents photo.
    /// </summary>
    public class PhotoModel
    {
        // Initializes some properties, that must contain some value by default.
        public PhotoModel()
        {
            DateOfCreation = DateTime.Now;
            Format = string.Empty;
            Description = string.Empty;
        }

        public PhotoModel(int albumId, int userId)
        {
            AlbumId = albumId;
            OwnerId = userId;
            DateOfCreation = DateTime.Now;
            Format = string.Empty;
            Description = string.Empty;
        }

        /// <summary>
        /// Gets or sets the photo ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets real format of photo.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets the description of photo.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Time and date of photo creating.
        /// </summary>
        public DateTime DateOfCreation { get; set; }

        /// <summary>
        /// Gets or sets the user's rating of each photo in album.
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Gets or sets a photo state.
        /// </summary>
        public bool IsDeleted { get; set; }


        public int OwnerId { get; set; }
        public int AlbumId { get; set; }

        public virtual ICollection<PhotoCommentModel> PhotoComments { get; set; }
        public virtual ICollection<PhotoTagModel> Tags { get; set; }

        /// <summary>
        /// Gets or sets how much people like photo ( contains users' ids).
        /// </summary>
        public virtual ICollection<UserModel> Likes { get; set; }
    }
}