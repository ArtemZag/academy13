using System.Security.Principal;

namespace BinaryStudio.PhotoGallery.Web.CustomStructure
{
    internal interface ICustomPrincipal : IPrincipal
    {
        int Id { get; }
        string Email { get; }
        bool IsAdmin { get; }
    }
}