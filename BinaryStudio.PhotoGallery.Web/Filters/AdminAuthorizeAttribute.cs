using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BinaryStudio.PhotoGallery.Web.CustomStructure;

namespace BinaryStudio.PhotoGallery.Web.Filters
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new {controller = "Error", action = "AccessDenied"}));
            }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = httpContext.User as CustomPrincipal;

            return user != null && user.IsAdmin && user.Identity.IsAuthenticated;
        }
    }
}