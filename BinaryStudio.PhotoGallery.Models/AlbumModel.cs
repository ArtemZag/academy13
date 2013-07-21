using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace BinaryStudio.PhotoGallery.Models
{
    /// <summary>
    /// The class that represents album
    /// </summary>
    public class AlbumModel
    {
        /// <summary>
        /// Gets or sets the album id.
        /// </summary>
        public int ID { get; set; }

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
        public DateTime DataOfCreation { get; set; }

        /// <summary>
        /// Gets or sets the permissions for aldum: what goups of user can see and add comments of photo.
        /// </summary>
        public int Permissions { get; set; } 

        public virtual int UserModelID { get; set; }
        public virtual ICollection<PhotoModel> Photos { get; set; }
        public virtual ICollection<AvailableGroupModel> AvaibleGroups { get; set; }

    }
}