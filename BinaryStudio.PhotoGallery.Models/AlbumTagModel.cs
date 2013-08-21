using System.Collections.Generic;

namespace BinaryStudio.PhotoGallery.Models
{
    /// <summary>
    /// The class that represents album tag.
    /// </summary>
    public class AlbumTagModel
    {
        /// <summary>
        /// Gets or sets the album tag ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the album tag name.
        /// </summary>
        public string TagName { get; set; }

        public virtual ICollection<AlbumModel> Albums { get; set; }
    }
}
