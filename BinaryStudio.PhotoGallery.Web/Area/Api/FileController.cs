using System;
using System.Collections.Generic;
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
        private readonly IAlbumService _albumService;
        private readonly ICryptoProvider _cryptoProvider;
        private readonly IDirectoryWrapper _directoryWrapper;
        private readonly IFileHelper _fileHelper;
        private readonly IFileWrapper _fileWrapper;
        private readonly IModelConverter _modelConverter;
        private readonly IPathUtil _pathUtil;
        private readonly IPhotoService _photoService;
        private readonly IUserService _userService;

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
                int userId = _userService.GetUserId(User.Identity.Name);

                int albumId = 0;

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
                string pathToTempFolder = _pathUtil.BuildAbsoluteTemporaryDirectoryPath(userId);

                string pathToAlbum = _pathUtil.BuildAbsoluteAlbumPath(userId, albumId);

                if (!_directoryWrapper.Exists(pathToAlbum))
                {
                    _directoryWrapper.CreateDirectory(pathToAlbum);
                }

                foreach (string photoHash in viewModel.PhotoHashes)
                {
                    // Remove all forbidden symbols in file name
                    string fileNameInTemporaryFolder = Path.GetInvalidFileNameChars()
                        .Aggregate(photoHash, (current, c) => current.Replace(c.ToString(), ""));

                    string filePathInTemporaryFolder = string.Format("{0}\\{1}", pathToTempFolder,
                        fileNameInTemporaryFolder);

                    bool fileExist = _fileWrapper.Exists(filePathInTemporaryFolder);

                    if (fileExist)
                    {
                        string realFileFormat = _fileHelper.GetRealFileFormat(filePathInTemporaryFolder);

                        int photoId =
                            _photoService.AddPhoto(_modelConverter.GetPhotoModel(userId, albumId, realFileFormat)).Id;

                        string filePathInAlbumFolder = string.Format("{0}\\{1}.{2}", pathToAlbum, photoId,
                            realFileFormat);

                        try
                        {
                            _fileWrapper.Move(filePathInTemporaryFolder, filePathInAlbumFolder);
                        }
                        catch (Exception)
                        {
                            uploadFileInfos.Add(new UploadFileInfo
                            {
                                FileHash = photoHash,
                                IsAccepted = false,
                                Error = "Can't save photo to selected album"
                            });

                            continue;
                        }

                        uploadFileInfos.Add(new UploadFileInfo
                        {
                            FileHash = photoHash,
                            IsAccepted = true
                        });
                    }
                    else
                    {
                        uploadFileInfos.Add(new UploadFileInfo
                        {
                            FileHash = photoHash,
                            IsAccepted = false,
                            Error = "Photo not found in temporary folder"
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
                int userId = _userService.GetUserId(User.Identity.Name);

                // Get path to the temporary folder in the user folder
                string pathToTempFolder = _pathUtil.BuildAbsoluteTemporaryDirectoryPath(userId);

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
                    string originalFileName = fileData.Headers.ContentDisposition.FileName.Trim('"');

                    long fileSize = _fileHelper.GetFileSize(fileData.LocalFileName);

                    string fileHash = _cryptoProvider.GetHash(string.Format("{0}{1}", originalFileName, fileSize));

                    // Remove all forbidden symbols in file name
                    string fileName = Path.GetInvalidFileNameChars()
                        .Aggregate(fileHash, (current, c) => current.Replace(c.ToString(), ""));

                    string temporaryFileName = string.Format("{0}\\{1}", pathToTempFolder, fileName);

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
                    catch (Exception)
                    {
                        _fileWrapper.Delete(fileData.LocalFileName);

                        uploadFileInfos.Add(new UploadFileInfo
                        {
                            FileHash = fileHash,
                            IsAccepted = false,
                            Error = "File already uploaded"
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

        private struct UploadFileInfo
        {
            public string Error;
            public string FileHash;
            public bool IsAccepted;
        }
    }
}