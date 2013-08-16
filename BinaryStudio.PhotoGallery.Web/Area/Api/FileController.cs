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
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Web.Utils;
using BinaryStudio.PhotoGallery.Web.ViewModels.Upload;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
    [Authorize]
    [RoutePrefix("Api/File")]
    public class FileController : ApiController
    {
        private struct NotAccpetedFilesData
        {
            public string Name;
            public string Error;
        }

        private readonly IUserService _userService;
	    private readonly IPathUtil _pathUtil;
	    private readonly IDirectoryWrapper _directoryWrapper;
	    private readonly IFileHelper _fileHelper;
	    private readonly IFileWrapper _fileWrapper;
        private readonly IPhotoService _photoService;
        private readonly IModelConverter _modelConverter;
        private readonly IAlbumService _albumService;

        public FileController(
            IUserService userService,
            IPathUtil pathUtil,
            IDirectoryWrapper directoryWrapper,
            IFileHelper fileHelper,
            IFileWrapper fileWrapper,
            IPhotoService photoService,
            IModelConverter modelConverter,
            IAlbumService albumService)
        {
            _userService = userService;
            _pathUtil = pathUtil;
	        _directoryWrapper = directoryWrapper;
	        _fileHelper = fileHelper;
	        _fileWrapper = fileWrapper;
	        _photoService = photoService;
            _modelConverter = modelConverter;
            _albumService = albumService;
        }

        [POST("MovePhotos")]
        public HttpResponseMessage MovePhotos([FromBody] SavePhotosViewModel viewModel)
        {
            if (viewModel == null || string.IsNullOrEmpty(viewModel.AlbumName) || !viewModel.PhotoNames.Any())
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Uknown error");
            }

            var notAccpetedFiles = new List<string>();

            try
            {
                // Get user ID from DB
                var userId = _userService.GetUserId(User.Identity.Name);

                var albumId = 0;

                try
                {
                    albumId = _albumService.GetAlbumId(viewModel.AlbumName);
                }
                catch (AlbumNotFoundException)
                {
                    _albumService.CreateAlbum(User.Identity.Name, viewModel.AlbumName);

                    albumId = _albumService.GetAlbumId(viewModel.AlbumName);
                }

                // Get path to the temporary folder in the user folder
                var pathToTempFolder = _pathUtil.BuildAbsoluteTemporaryDirectoryPath(userId);

                var pathToAlbum = _pathUtil.BuildAbsoluteAlbumPath(userId, albumId);

                if (!_directoryWrapper.Exists(pathToAlbum))
                {
                    _directoryWrapper.CreateDirectory(pathToAlbum);
                }

                foreach (var photoName in viewModel.PhotoNames)
                {
                    var currentFilePath = string.Format("{0}\\{1}", pathToTempFolder, photoName);

                    var fileExist = _fileWrapper.Exists(currentFilePath);

                    if (fileExist)
                    {
                        _photoService.AddPhoto(_modelConverter.GetPhotoModel(userId, albumId, photoName));

                        var newFilePath = string.Format("{0}\\{1}", pathToAlbum, photoName);

                        _fileHelper.HardMove(currentFilePath, newFilePath);
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
            var notAccpetedFiles = new List<NotAccpetedFilesData>();

            try
            {
                // Get user ID from DB
                var userId = _userService.GetUserId(User.Identity.Name);

                // Get path to the temporary folder in the user folder
                var pathToTempFolder = _pathUtil.BuildAbsoluteTemporaryDirectoryPath(userId);

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
                        notAccpetedFiles.Add(new NotAccpetedFilesData
                        {
                            Name = originalFileName,
                            Error = "This file contains no image data"
                        });
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
                        notAccpetedFiles.Add(new NotAccpetedFilesData {Name = originalFileName, Error = ex.Message});
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            // Create response data
            var listOfNotLoadedFiles = new ObjectContent<IList<NotAccpetedFilesData>>
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
