namespace BinaryStudio.PhotoGallery.Core.SocialNetworkUtils
{
    public interface ISocialNetworkFactory
    {
        ISocialNetwork GetFacebookUnit();
        ISocialNetwork GetVk();
    }
}
