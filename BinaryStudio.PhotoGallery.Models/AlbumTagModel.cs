using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the album tag name.
        /// </summary>
        public string TagName { get; set; }

        public virtual ICollection<AlbumModel> AlbumModels { get; set; }
    }
}
