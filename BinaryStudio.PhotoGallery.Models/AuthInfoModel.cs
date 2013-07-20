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
        /// Gets or sets the name of the authentication service.
        /// </summary>
        public string AuthProvider { get; set; }

        /// <summary>
        /// Gets or sets the user's password.
        /// </summary>
        public string UserPassword { get; set; }

        /// <summary>
        /// Gets or sets token for work with social webs
        /// </summary>
        public string AuthProviderToken { get; set; }

        public virtual int UserModelID { get; set; }
    }
}