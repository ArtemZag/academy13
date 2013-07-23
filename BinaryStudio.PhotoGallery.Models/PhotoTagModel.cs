using System.Collections.Generic;


namespace BinaryStudio.PhotoGallery.Models
{
    /// <summary>
    /// The class that represents photo tag.
    /// </summary>
    public class PhotoTagModel
    {
        public PhotoTagModel(string tagName)
        {
            TagName = tagName;
        }

        /// <summary>
        /// Gets or sets the photo tag ID.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the photo tag name.
        /// </summary>
        public string TagName { get; set; }

        public virtual ICollection<PhotoModel> PhotoModels { get; set; }
    }
}
