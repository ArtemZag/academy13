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
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the nick name of the user
        /// </summary>
        [MaxLength(255)] // Standart RFC 5321
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the nick name of the user
        /// </summary>
        [MaxLength(80)]
        public string NickName { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        [MaxLength(255)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the lastname of the user.
        /// </summary>
        [MaxLength(255)]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the type of user account.
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// Gets or sets the name of department.
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// Gets or sets the user's password.
        /// </summary>
        public string UserPassword { get; set; }

        /// <summary>
        /// Salt for password
        /// </summary>
        public string Salt { get; set; }


        public virtual ICollection<AlbumModel> Albums { get; set; }
        public virtual ICollection<GroupModel> Groups { get; set; }
        public virtual ICollection<AuthInfoModel> AuthInfos { get; set; }
    }
}
