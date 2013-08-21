using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BinaryStudio.PhotoGallery.Domain.Services;

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
            var userService = httpContext.GetService(typeof (IUserService)) as IUserService;

            return base.AuthorizeCore(httpContext);
        }
    }
}