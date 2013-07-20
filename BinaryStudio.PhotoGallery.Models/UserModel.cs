using System.Collections.Generic;

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
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the nick name of the user
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the lastname of the user.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the type of user account.
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// Gets or sets the name of department.
        /// </summary>
        public string Department { get; set; }


        public virtual ICollection<AlbumModel> Albums { get; set; }
        public virtual ICollection<GroupModel> Groups { get; set; }
        public virtual ICollection<AuthInfoModel> Authinfos { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((UserModel) obj);
        }

        protected bool Equals(UserModel other)
        {
            return string.Equals(Email, other.Email);
        }

        public override int GetHashCode()
        {
            return (Email != null ? Email.GetHashCode() : 0);
        }
    }
}
