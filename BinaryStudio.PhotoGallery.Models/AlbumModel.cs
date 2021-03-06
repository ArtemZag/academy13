﻿using System;
using System.Collections.Generic;

namespace BinaryStudio.PhotoGallery.Models
{
    /// <summary>
    ///     The class that represents album
    /// </summary>
    public class AlbumModel
    {
        // Initializes some properties, that must contain some value by default
        public AlbumModel()
        {
            DateOfCreation = DateTime.Now;
            Description = string.Empty;
        }

        public AlbumModel(string name, int ownerId)
        {
            Name = name;
            OwnerId = ownerId;
            DateOfCreation = DateTime.Now;
            Permissions = (int)PermissionsMask.PublicAlbum; // in moment, it is just for future
            Description = string.Empty;
        }

        [Flags]
        public enum PermissionsMask : byte
        {
            PublicAlbum = 0x1,
            //SomePerm1 = 2,
            //SomePerm2 = 4,
        }

        /// <summary>
        ///     Gets or sets the album id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Gets or sets the album state.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        ///     Gets or sets the name of the album.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the description of the album.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the date and time of album creation.
        /// </summary>
        public DateTime DateOfCreation { get; set; }

        /// <summary>
        ///     Gets or sets the permissions for aldum: what goups of user can see and add comments of photo.
        /// </summary>
        public int Permissions { get; set; }

        public int OwnerId { get; set; }

        public virtual ICollection<PhotoModel> Photos { get; set; }
        public virtual ICollection<AvailableGroupModel> AvailableGroups { get; set; }
    }
}