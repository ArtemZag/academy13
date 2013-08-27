using BinaryStudio.PhotoGallery.Web.CustomStructure;

namespace BinaryStudio.PhotoGallery.Web.Hubs
{
    public interface ICustomPrincipalInHub
    {
        new CustomPrincipal User { get; }
    }
}