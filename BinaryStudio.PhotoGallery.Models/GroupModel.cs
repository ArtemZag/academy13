using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace BinaryStudio.PhotoGallery.Models
{
    /// <summary>
    /// The class that represents group of users.
    /// </summary>
    public class GroupModel
    {
        /// <summary>
        /// Gets or sets the id of the group.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the name of the group.
        /// </summary>
        [Required(ErrorMessage = "GroupName is required")]
        [StringLength(100, ErrorMessage = "GroupName must contain at least 3 characters.", MinimumLength = 3)]
        public string GroupName { get; set; }

        /// <summary>
        /// Gets or sets the decription of the group.
        /// </summary>
        public string Description { get; set; }

        public virtual ICollection<UserModel> Users { get; set; }
    }
}