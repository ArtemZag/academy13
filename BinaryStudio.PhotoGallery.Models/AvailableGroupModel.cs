using System.ComponentModel.DataAnnotations;


namespace BinaryStudio.PhotoGallery.Models
{
    /// <summary>
    /// The class that represents groups that avaible for work with a comments and photos (different for each album).
    /// </summary>
    public class AvailableGroupModel
    {
        /// <summary>
        /// Gets or sets the idintification for each group availability.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the availability for group.
        /// </summary>
        [Required]
        public bool CanSeePhotos { get; set; }
        [Required]
        public bool CanSeeComments { get; set; }
        [Required]
        public bool CanAddPhotos { get; set; }
        [Required]
        public bool CanAddComments { get; set; }


        public virtual int GroupModelID { get; set; }
        public virtual int AlbumModelID { get; set; }
    }
}