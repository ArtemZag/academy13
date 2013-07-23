﻿

namespace BinaryStudio.PhotoGallery.Models
{
    /// <summary>
    /// The class that represents groups that avaible for work with a comments and photos (different for each album).
    /// </summary>
    public class AvailableGroupModel
    {
        public AvailableGroupModel(int groupModelID, int albumModelID)
        {
            GroupModelID = groupModelID;
            AlbumModelID = albumModelID;
        }

        /// <summary>
        /// Gets or sets the idintification for each group availability.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the availability for group.
        /// </summary>
        public bool CanSeePhotos { get; set; }
        public bool CanSeeComments { get; set; }
        public bool CanAddPhotos { get; set; }
        public bool CanAddComments { get; set; }


        public int GroupModelID { get; set; }
        public int AlbumModelID { get; set; }
    }
}