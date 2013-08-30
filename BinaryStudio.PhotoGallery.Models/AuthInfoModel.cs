
namespace BinaryStudio.PhotoGallery.Models
{
    /// <summary>
    /// The class that represents types of user's authentications.
    /// </summary>
    public class AuthInfoModel
    {
        public enum ProviderType
        {
            Facebook,
            Vk,
            Twitter,
            Github
        }

        public AuthInfoModel()
        {
        }

        /// <summary>
        /// Main constructor with important fields
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="authProvider"></param>
        public AuthInfoModel(int userId, string authProvider)
        {
            UserId = userId;
            AuthProvider = authProvider;
        }

        

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the authentication service.
        /// </summary>
        public string AuthProvider { get; set; }

        /// <summary>
        /// Gets or sets ID for work with social webs
        /// </summary>
        public string AuthProviderId { get; set; }

        public int UserId { get; set; }
    }
}