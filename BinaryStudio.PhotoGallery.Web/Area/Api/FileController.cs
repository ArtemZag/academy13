using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Core.Helpers;
using BinaryStudio.PhotoGallery.Core.IOUtils;
using BinaryStudio.PhotoGallery.Core.PathUtils;
using BinaryStudio.PhotoGallery.Domain.Services;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
	[RoutePrefix("Api/File")]
    public class FileController : ApiController
    {
        private readonly IUserService _userService;
	    private readonly IPathUtil _pathUtil;
	    private readonly IDirectoryWrapper _directoryWrapper;
	    private readonly IFileHelper _fileHelper;
	    private readonly IFileWrapper _fileWrapper;

	    public FileController(
            IUserService userService,
            IPathUtil pathUtil,
            IDirectoryWrapper directoryWrapper,
            IFileHelper fileHelper,
            IFileWrapper fileWrapper)
        {
            _userService = userService;
            _pathUtil = pathUtil;
	        _directoryWrapper = directoryWrapper;
	        _fileHelper = fileHelper;
	        _fileWrapper = fileWrapper;
        }

	    [POST("Post")]
        public async Task<HttpResponseMessage> Post()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            // Key - file name, Value - error of the loading
            var notAccpetedFiles = new Dictionary<string, string>();

            try
            {
                // Get user ID from BD
                var userId = _userService.GetUserId(User.Identity.Name);

                // Combine of the path to temporary folder in the user folder
                var pathToTempFolder = string.Format("{0}\\{1}\\temporary", _pathUtil.GetAbsoluteRoot(), userId);

                // Create directory, if it isn't exist
                if (!_directoryWrapper.Exists(pathToTempFolder))
                {
                    _directoryWrapper.CreateDirectory(pathToTempFolder);
                }

                // TODO what I must to do here? Create new instance of MFDSPW oder it come
                var provider = new MultipartFormDataStreamProvider(pathToTempFolder);

                // Read the form data from request
                await Request.Content.ReadAsMultipartAsync(provider);

                // Check all files
                foreach (MultipartFileData fileData in provider.FileData)
                {
                    var originalFileName = fileData.Headers.ContentDisposition.FileName.Replace("\"", "");
                    var destFileName = string.Format("{0}\\{1}", pathToTempFolder, originalFileName);

                    // Is it really image file format ?
                    if (!_fileHelper.IsImageFile(fileData.LocalFileName))
                    {
                        _fileWrapper.Delete(fileData.LocalFileName);
                        notAccpetedFiles.Add(originalFileName, "This file contains no image data");
                        continue;
                    }

                    // try to rename file to source name (users file name)
                    try
                    {
                        _fileHelper.HardRename(fileData.LocalFileName, destFileName);
                    }
                    catch (FileRenameException ex)
                    {
                        _fileWrapper.Delete(fileData.LocalFileName);        // delete temp file
                        notAccpetedFiles.Add(originalFileName, ex.Message);
                    }
                }

                if (notAccpetedFiles.Count == provider.FileData.Count)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            // Create response data
            var listOfNotLoadedFiles = new ObjectContent<IDictionary<string, string>>
                (notAccpetedFiles, new JsonMediaTypeFormatter());

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Created,
                Content = listOfNotLoadedFiles
            };

            return response;
        }
    }
}
