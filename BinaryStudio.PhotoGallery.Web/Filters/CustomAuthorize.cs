﻿using System.Web.Mvc;
using System.Web.Routing;

namespace BinaryStudio.PhotoGallery.Web.Filters
{
    public class CustomAuthorize : AuthorizeAttribute
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
    }
}