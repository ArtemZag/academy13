using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BinaryStudio.PhotoGallery.Web.Controllers;

namespace BinaryStudio.PhotoGallery.Web.Extensions
{
    public static class ControllerExtensions
    {
        private const string CRITICAL_MESSAGE = "criticalMessage";
        private const string CONFIRM_MESSAGE = "confirmMessage";
        private const string WARNING_MESSAGE = "warningMessage";

        /// <summary>
        /// Renders the confirm message.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="message">The message.</param>
        public static void AddConfirmMessage(this Controller controller, string message)
        {
            controller.ViewData[CONFIRM_MESSAGE] = message;
        }

        /// <summary>
        /// Renders the model error.
        /// </summary>
        /// <param name="controller">The controller</param>
        /// <param name="message">Model error message</param>
        public static void AddModelError(this Controller controller, string message)
        {
            controller.ModelState.AddModelError(String.Empty, message);
        }

        /// <summary>
        /// Renders the critical error.
        /// </summary>
        /// <param name="controller">The controller</param>
        /// <param name="message">The message</param>
        public static void AddCriticalError(this Controller controller, string message)
        {
            controller.ViewData[CRITICAL_MESSAGE] = message;
        }

        /// <summary>
        /// Renders the warning error.
        /// </summary>
        /// <param name="controller">The controller</param>
        /// <param name="message">The message</param>
        public static void AddWarningError(this Controller controller, string message)
        {
            controller.ViewData[WARNING_MESSAGE] = message;
        }

        /// <summary>
        /// Renders the specified exception and passes it to Error controller.
        /// </summary>
        /// <param name="exception">The exception</param>
        /// <param name="context">The Http context</param>
        public static void Render(this Exception exception, HttpContext context)
        {
            var routeData = new RouteData();
            routeData.Values.Add("controller", "Error");
            routeData.Values.Add("action", "Error");
            routeData.Values.Add("exception", exception);

            IController errorController = new ErrorController();
            errorController.Execute(new RequestContext(new HttpContextWrapper(context), routeData));
        }
    }
}