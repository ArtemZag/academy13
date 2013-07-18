using System;
using System.ComponentModel.DataAnnotations;

namespace BinaryStudio.PhotoGallery.Models
{
    /// <summary>
    /// The class that represents photo comment.
    /// </summary>
    public class PhotoCommentModel
    {
        /// <summary>
        /// Gets or sets the phot comment ID.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the date and time of comment creation.
        /// </summary>
        [Required]
        public DateTime DateOfCreating { get; set; }

        /// <summary>
        /// Gets or sets the user's rating for each comment.
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Gets or sets the text of comment.
        /// </summary>
        [Required]
        [StringLength(500)]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets if user reply in other comment.
        /// </summary>
        public PhotoCommentModel Reply { get; set; }
        public int PhotoModelID  { get; set; }
        public int UserModelID { get; set; }
    }
}