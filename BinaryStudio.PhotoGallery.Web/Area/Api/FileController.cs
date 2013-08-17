using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using BinaryStudio.PhotoGallery.Core.UserUtils;
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
        private struct UploadFileInfo
        {
            public string FileHash;
            public bool IsAccepted;
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
        private readonly ICryptoProvider _cryptoProvider;

        public FileController(
            IUserService userService,
            IPathUtil pathUtil,
            IDirectoryWrapper directoryWrapper,
            IFileHelper fileHelper,
            IFileWrapper fileWrapper,
            IPhotoService photoService,
            IModelConverter modelConverter,
            IAlbumService albumService,
            ICryptoProvider cryptoProvider)
        {
            _userService = userService;
            _pathUtil = pathUtil;
	        _directoryWrapper = directoryWrapper;
	        _fileHelper = fileHelper;
	        _fileWrapper = fileWrapper;
	        _photoService = photoService;
            _modelConverter = modelConverter;
            _albumService = albumService;
            _cryptoProvider = cryptoProvider;
        }

        [POST("MovePhotos")]
        public HttpResponseMessage MovePhotos([FromBody] SavePhotosViewModel viewModel)
        {
            if (viewModel == null || string.IsNullOrEmpty(viewModel.AlbumName) || !viewModel.PhotoHashes.Any())
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Uknown error");
            }

            var uploadFileInfos = new List<UploadFileInfo>();

            try
            {
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

                foreach (var fileName in viewModel.PhotoHashes)
                {
                    var filePath = string.Format("{0}\\{1}", pathToTempFolder, fileName);

                    var fileExist = _fileWrapper.Exists(filePath);

                    if (fileExist)
                    {
                        _photoService.AddPhoto(_modelConverter.GetPhotoModel(userId, albumId, fileName));

                        var newFilePath = string.Format("{0}\\{1}", pathToAlbum, fileName);

                        try
                        {
                            _fileWrapper.Move(filePath, newFilePath);
                        }
                        catch (Exception)
                        {
                            uploadFileInfos.Add(new UploadFileInfo
                            {
                                FileHash = fileName,
                                IsAccepted = false,
                                Error = string.Format("Can't save photo to album '{0}'", viewModel.AlbumName)
                            });

                            continue;
                        }

                        uploadFileInfos.Add(new UploadFileInfo
                        {
                            FileHash = fileName,
                            IsAccepted = true
                        });
                    }
                    else
                    {
                        uploadFileInfos.Add(new UploadFileInfo
                        {
                            FileHash = fileName,
                            IsAccepted = false,
                            Error = "Photo not found in temp folder"
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            var responseData = new ObjectContent<IEnumerable<UploadFileInfo>>
                (uploadFileInfos, new JsonMediaTypeFormatter());

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = responseData
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
            var uploadFileInfos = new List<UploadFileInfo>();

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
                    var originalFileName = fileData.Headers.ContentDisposition.FileName.Trim('"');

                    var fileSize = _fileHelper.GetFileSize(fileData.LocalFileName);

                    var fileHash = _cryptoProvider.GetHash(string.Format("{0}{1}", originalFileName, fileSize));

                    var fileName = Path.GetInvalidFileNameChars().Aggregate(fileHash, (current, c) => current.Replace(c.ToString(), ""));

                    var temporaryFileName = string.Format("{0}\\{1}", pathToTempFolder, fileName);

                    // Is it really image file format ?
                    if (!_fileHelper.IsImageFile(fileData.LocalFileName))
                    {
                        _fileWrapper.Delete(fileData.LocalFileName);

                        uploadFileInfos.Add(new UploadFileInfo
                        {
                            FileHash = fileHash,
                            Error = "This file contains no image data"
                        });

                        continue;
                    }

                    try
                    {
                        _fileWrapper.Move(fileData.LocalFileName, temporaryFileName);
                    }
                    catch (Exception ex)
                    {
                        _fileWrapper.Delete(fileData.LocalFileName);

                        uploadFileInfos.Add(new UploadFileInfo
                        {
                            FileHash = fileHash,
                            IsAccepted = false,
                            Error = "Can't save this file"
                        });

                        continue;
                    }

                    uploadFileInfos.Add(new UploadFileInfo
                    {
                        FileHash = fileHash,
                        IsAccepted = true
                    });
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            var responseData = new ObjectContent<IList<UploadFileInfo>>
                (uploadFileInfos, new JsonMediaTypeFormatter());

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = responseData
            };

            return response;
        }
    }
}
