using System.ComponentModel.DataAnnotations;


namespace BinaryStudio.PhotoGallery.Models
{
    /// <summary>
    /// The class that represents types of user's authentications.
    /// </summary>
    public class AuthInfoModel
    {
        /// <summary>
        /// Strings that represents type of auth.
        /// </summary>
        public const string LOCAL_PROFILE = "local";
        public const string GOOGLE_PROFILE = "google";

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the name of the authentication.
        /// </summary>
        [Required(ErrorMessage = "AuthName is required")]
        [StringLength(50, ErrorMessage = "AuthName must contain at least 3 characters.", MinimumLength = 3)]
        public string AuthName { get; set; }

        /// <summary>
        /// Gets or sets the user's E-mail.
        /// </summary>
        [Required(ErrorMessage = "E-mail is required")]
        [DataType(DataType.EmailAddress)]
        public string UserEmail { get; set; }

        /// <summary>
        /// Gets or sets the user's password.
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        [StringLength(50, ErrorMessage = "Password must contain at least 6 characters.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }

        public virtual int UserModelID { get; set; }
    }
}