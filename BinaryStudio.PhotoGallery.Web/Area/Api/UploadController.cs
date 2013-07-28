using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Services;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
	[RoutePrefix("Api/Upload")]
    public class UploadController : ApiController
    {
        private readonly IUserService _userService;

        public UploadController(IUserService userService)
        {
            _userService = userService;
        }

        [POST]
        public async Task<HttpResponseMessage> Index()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            try
            {
                // Get user ID from BD
                var userId = _userService.GetUserId(User.Identity.Name);

                // Find user folder 'temporary' for uploading files into it
                var dirForSave = HttpContext.Current.Server.MapPath(string.Format(@"~/App_Data/{0}/temporary", userId));

                // Create directory, if it isn't exist
                if (!Directory.Exists(dirForSave))
                {
                    Directory.CreateDirectory(dirForSave);
                }
                
                var provider = new MultipartFormDataStreamProvider(dirForSave);

                // Read the form data from request
                await Request.Content.ReadAsMultipartAsync(provider);

                // This illustrates how to get the file names
                foreach (MultipartFileData file in provider.FileData)
                {
                    var originalFileName = file.Headers.ContentDisposition.FileName.Replace("\"", "");
                    File.Move(file.LocalFileName, string.Format("{0}/{1}", dirForSave, originalFileName));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return new HttpResponseMessage(HttpStatusCode.Created);
        }
    }
}
