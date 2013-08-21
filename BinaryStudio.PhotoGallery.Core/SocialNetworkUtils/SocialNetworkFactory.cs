using BinaryStudio.PhotoGallery.Core.SocialNetworkUtils.Vkontakte;


namespace BinaryStudio.PhotoGallery.Core.SocialNetworkUtils
{
    public class SocialNetworkFactory : ISocialNetworkFactory
    {

        public ISocialNetwork GetFacebookUnit()
        {
            return null;
        }

        public ISocialNetwork GetVk()
        {
            return new VK();
        }
    }
}
