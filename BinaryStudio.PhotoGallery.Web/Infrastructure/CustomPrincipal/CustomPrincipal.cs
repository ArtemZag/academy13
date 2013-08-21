using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Principal;
using System.Web.Security;

namespace BinaryStudio.PhotoGallery.Web
{
    [Serializable]
    public class CustomPrincipal : ICustomPrincipal, ISerializable
    {
        public CustomPrincipal(int userId, string userEmail, bool isAdmin)
        {
            Id = userId;
            Email = userEmail;
            IsAdmin = isAdmin;

            Identity = new GenericIdentity(userId.ToString());
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

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (context.State == StreamingContextStates.CrossAppDomain)
            {
                info.SetType(GetType());

                MemberInfo[] serializableMembers = FormatterServices.GetSerializableMembers(GetType());
                object[] serializableValues = FormatterServices.GetObjectData(this, serializableMembers);

                for (int i = 0; i < serializableMembers.Length; i++)
                {
                    info.AddValue(serializableMembers[i].Name, serializableValues[i]);
                }
            }
            else
            {
                throw new InvalidOperationException("Serialization in CustomPrincipal not supported");
            }
        }
    }
}