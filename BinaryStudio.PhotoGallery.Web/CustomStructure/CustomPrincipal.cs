using System.Security.Principal;
using System.Web.Security;

namespace BinaryStudio.PhotoGallery.Web.CustomStructure
{
    public class CustomPrincipal : ICustomPrincipal
    {
        public CustomPrincipal(int userId, string userEmail, bool isAdmin)
        {
            Id = userId;
            Email = userEmail;
            IsAdmin = isAdmin;

            Identity = new GenericIdentity(userEmail);
        }

        public bool IsInRole(string role)
        {
            return Identity.IsAuthenticated &&
                   !string.IsNullOrWhiteSpace(role) &&
                   Roles.IsUserInRole(Identity.Name, role);
        }

        public IIdentity Identity { get; private set; }

        public int Id { get; private set; }
        public string Email { get; private set; }
        public bool IsAdmin { get; private set; }
    }
}