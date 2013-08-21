using System;

namespace BinaryStudio.PhotoGallery.Models
{
    /// <summary>
    /// The class that represents photo comment.
    /// </summary>
    public class PhotoCommentModel
    {
        // Initializes some properties, that must contain some value by default
        public PhotoCommentModel()
        {
            DateOfCreating = DateTime.Now;
        }

        public PhotoCommentModel(int userId,  int photoId, string text, int reply)
        {
            Reply = reply;
            Text = text;
            PhotoId = photoId;
            UserId = userId;
            DateOfCreating = DateTime.Now;
        }

        

        /// <summary>
        /// Gets or sets the phot comment ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the date and time of comment creation.
        /// </summary>
        public DateTime DateOfCreating { get; set; }

        /// <summary>
        /// Gets or sets the user's rating for each comment.
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Gets or sets the text of comment.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets if user reply in other comment.
        /// </summary>
        public int Reply { get; set; }

        public int PhotoId  { get; set; }
        public int UserId { get; set; }
    }
}