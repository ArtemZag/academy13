using System;
using System.Collections.Generic;
using System.Linq;
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
using BinaryStudio.PhotoGallery.Web.Utils;
using BinaryStudio.PhotoGallery.Web.ViewModels.Upload;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
    [Authorize]
    [RoutePrefix("Api/File")]
    public class FileController : ApiController
    {
        private readonly IUserService _userService;
	    private readonly IPathUtil _pathUtil;
	    private readonly IDirectoryWrapper _directoryWrapper;
	    private readonly IFileHelper _fileHelper;
	    private readonly IFileWrapper _fileWrapper;
        private readonly IPhotoService _photoService;
        private readonly IModelConverter _modelConverter;

        public FileController(
            IUserService userService,
            IPathUtil pathUtil,
            IDirectoryWrapper directoryWrapper,
            IFileHelper fileHelper,
            IFileWrapper fileWrapper,
            IPhotoService photoService,
            IModelConverter modelConverter)
        {
            _userService = userService;
            _pathUtil = pathUtil;
	        _directoryWrapper = directoryWrapper;
	        _fileHelper = fileHelper;
	        _fileWrapper = fileWrapper;
	        _photoService = photoService;
            _modelConverter = modelConverter;
        }

        [POST("SavePhotos")]
        public HttpResponseMessage SavePhotos([FromBody] SavePhotosViewModel viewModel)
        {
            if (viewModel == null || viewModel.AlbumId < 0 || !viewModel.PhotoNames.Any())
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            var notAccpetedFiles = new List<string>();

            try
            {
                // Get user ID from DB
                var userId = _userService.GetUserId(User.Identity.Name);

                // Get path to the temporary folder in the user folder
                var pathToTempFolder = _pathUtil.BuildTemporaryDirectoryPath(userId);

                foreach (var photoName in viewModel.PhotoNames)
                {
                    var currentFilePath = string.Format("{0}\\{1}", pathToTempFolder, photoName);

                    var fileExist = _fileWrapper.Exists(currentFilePath);

                    if (fileExist)
                    {
                        var pathToAlbum = _pathUtil.BuildAlbumPath(userId, viewModel.AlbumId);

                        if (!_directoryWrapper.Exists(pathToAlbum))
                        {
                            _directoryWrapper.CreateDirectory(pathToAlbum);
                        }

                        var newFilePath = string.Format("{0}\\{1}", pathToAlbum, photoName);

                        _fileHelper.HardMove(currentFilePath, newFilePath);

                        _photoService.AddPhoto(_modelConverter.GetPhotoModel(userId, viewModel.AlbumId, photoName));
                    }
                    else
                    {
                        notAccpetedFiles.Add(photoName);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            // Create response data
            var listOfNotLoadedFiles = new ObjectContent<IEnumerable<string>>
                (notAccpetedFiles, new JsonMediaTypeFormatter());

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Accepted,
                Content = listOfNotLoadedFiles
            };

            return response;
        }
        
        [POST("Upload")]
        public async Task<HttpResponseMessage> Upload()
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
                // Get user ID from DB
                var userId = _userService.GetUserId(User.Identity.Name);

                // Get path to the temporary folder in the user folder
                var pathToTempFolder = _pathUtil.BuildTemporaryDirectoryPath(userId);

                // Create directory, if it isn't exist
                if (!_directoryWrapper.Exists(pathToTempFolder))
                {
                    _directoryWrapper.CreateDirectory(pathToTempFolder);
                }

                // TODO create this instance with fabrik
                var provider = new MultipartFormDataStreamProvider(pathToTempFolder);

                // Read the form data from request TODO must be wrapped too
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
                        _fileHelper.HardMove(fileData.LocalFileName, destFileName);
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
