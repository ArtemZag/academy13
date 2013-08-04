using System;
using System.Collections.Generic;

namespace BinaryStudio.PhotoGallery.Models
{
    /// <summary>
    /// The class that represents album
    /// </summary>
    public class AlbumModel
    {
        // Initializes some properties, that must contain some value by default
        public AlbumModel()
        {
            DateOfCreation = DateTime.Now;
        }

        public AlbumModel(string albumName, int ownerId)
        {
            AlbumName = albumName;
            UserModelId = ownerId;
            DateOfCreation = DateTime.Now;
            Permissions = 111; // in moment, it is just for future
        }
        

        /// <summary>
        /// Gets or sets the album id.
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Gets or sets the album state.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the name of the album.
        /// </summary>
        public string AlbumName { get; set; }

        /// <summary>
        /// Gets or sets the description of the album.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the date and time of album creation.
        /// </summary>
        public DateTime DateOfCreation { get; set; }

        /// <summary>
        /// Gets or sets the permissions for aldum: what goups of user can see and add comments of photo.
        /// </summary>
        public int Permissions { get; set; } 

        public int UserModelId { get; set; }
        public virtual ICollection<PhotoModel> Photos { get; set; }
        public virtual ICollection<AvailableGroupModel> AvailableGroups { get; set; }
        public virtual ICollection<AlbumTagModel> AlbumTags { get; set; }

    }
}