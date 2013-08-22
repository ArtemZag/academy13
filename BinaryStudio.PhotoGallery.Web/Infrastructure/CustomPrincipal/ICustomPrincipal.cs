using System.Security.Principal;

namespace BinaryStudio.PhotoGallery.Web
{
    internal interface ICustomPrincipal : IPrincipal
    {
        int Id { get; }
        string Email { get; }
        bool IsAdmin { get; }
    }
}