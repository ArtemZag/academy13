using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BinaryStudio.PhotoGallery.Models
{
    /// <summary>
    /// The class that represents user
    /// </summary>
    public class UserModel
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the nick name of the user
        /// </summary>
        [Required(ErrorMessage = "NickName is required")]
        [StringLength(50, ErrorMessage = "NickName must contain at least 3 characters.", MinimumLength = 3)]
        public string NickName { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        [Required(ErrorMessage = "FirstName is required")]
        [StringLength(50, ErrorMessage = "FistName must contain at least 3 characters.", MinimumLength = 3)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the lastname of the user.
        /// </summary>
        [Required(ErrorMessage = "LastName is required")]
        [StringLength(50, ErrorMessage = "LastName must contain at least 2 characters.", MinimumLength = 2)]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the type of user account.
        /// </summary>
        [Required(ErrorMessage = "Role is required")]
        public bool IsAdmin { get; set; }

        /// <summary>
        /// Gets or sets the name of department.
        /// </summary>
        public string Department { get; set; }


        public virtual ICollection<AlbumModel> Albums { get; set; }
        public virtual ICollection<GroupModel> Groups { get; set; }
        public virtual ICollection<AuthInfoModel> Authinfos { get; set; }
    }
}
