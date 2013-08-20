using System.Collections.Generic;

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
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the group.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Gets or sets the decription of the group.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets group owner's ID
        /// </summary>
        public int OwnerId { get; set; }

        public virtual ICollection<UserModel> Users { get; set; }
    }
}