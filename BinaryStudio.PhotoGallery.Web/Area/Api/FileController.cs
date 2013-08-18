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
using BinaryStudio.PhotoGallery.Core.UserUtils;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.Utils;
using BinaryStudio.PhotoGallery.Web.ViewModels.Upload;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
//    [Authorize]
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

        private int MAX_PHOTO_SIZE_IN_BYTES = 30*1024*1024; // 30 MB

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

        [DELETE]
        public HttpResponseMessage Delete(int photoId)
        {
            if (photoId <= 0)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Can't delete photo by invalid id");
            }

            try
            {
                _photoService.DeletePhoto(User.Identity.Name, photoId);
            }
            catch (NoEnoughPrivileges ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [POST]
        public HttpResponseMessage Move([FromBody] SavePhotosViewModel viewModel)
        {
            if (viewModel == null || string.IsNullOrEmpty(viewModel.AlbumName) || !viewModel.PhotosId.Any())
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
                    albumId = _albumService.GetAlbumId(userId, viewModel.AlbumName);
                }
                catch (AlbumNotFoundException)
                {
                    albumId = _albumService.CreateAlbum(userId, viewModel.AlbumName).Id;
                }

                // Get path to the temporary folder in the user folder
                string pathToTempFolder = _pathUtil.BuildAbsoluteTemporaryDirectoryPath(userId);

                string pathToAlbum = _pathUtil.BuildAbsoluteAlbumPath(userId, albumId);

                if (!_directoryWrapper.Exists(pathToAlbum))
                {
                    _directoryWrapper.CreateDirectory(pathToAlbum);
                }

                foreach (int photoId in viewModel.PhotosId)
                {
                    PhotoModel photoModel = _photoService.GetPhoto(userId, photoId);

                    string fileInTemporary = string.Format("{0}\\{1}.{2}", pathToTempFolder, photoModel.Id, photoModel.Format);

                    string fileInAlbum = string.Format("{0}\\{1}.{2}", pathToAlbum, photoModel.Id, photoModel.Format);

                    bool fileExist = _fileWrapper.Exists(fileInTemporary);

                    if (fileExist)
                    {
                        try
                        {
                            _fileWrapper.Move(fileInTemporary, fileInAlbum);
                        }
                        catch (Exception)
                        {
                            uploadFileInfos.Add(new UploadFileInfo
                            {
                                Id = photoId,
                                IsAccepted = false,
                                Error = "Can't save photo to selected album"
                            });

                            continue;
                        }

                        photoModel.AlbumId = albumId;

                        _photoService.UpdatePhoto(photoModel);

                        uploadFileInfos.Add(new UploadFileInfo
                        {
                            Id = photoId,
                            IsAccepted = true
                        });
                    }
                    else
                    {
                        uploadFileInfos.Add(new UploadFileInfo
                        {
                            Id = photoId,
                            IsAccepted = false,
                            Error = "Photo is not found in temporary album"
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

        [POST]
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

                    if (fileSize > MAX_PHOTO_SIZE_IN_BYTES)
                    {
                        _fileWrapper.Delete(fileData.LocalFileName);

                        uploadFileInfos.Add(new UploadFileInfo
                        {
                            Hash = fileHash,
                            IsAccepted = false,
                            Error = "This file contains no image data"
                        });
                    }

                    // Is it really image file format ?
                    if (!_fileHelper.IsImageFile(fileData.LocalFileName))
                    {
                        _fileWrapper.Delete(fileData.LocalFileName);

                        uploadFileInfos.Add(new UploadFileInfo
                        {
                            Hash = fileHash,
                            IsAccepted = false,
                            Error = "This file contains no image data"
                        });

                        continue;
                    }

                    string format = _fileHelper.GetRealFileFormat(fileData.LocalFileName);

                    int albumId = _albumService.GetAlbumId(User.Identity.Name, "Temporary");

                    int photoId = _photoService.AddPhoto(_modelConverter.GetPhotoModel(userId, albumId, format)).Id;

                    string destFileName = string.Format("{0}\\{1}.{2}", pathToTempFolder, photoId, format);

                    try
                    {
                        _fileWrapper.Move(fileData.LocalFileName, destFileName);
                    }
                    catch (Exception)
                    {
                        _fileWrapper.Delete(fileData.LocalFileName);

                        uploadFileInfos.Add(new UploadFileInfo
                        {
                            Hash = fileHash,
                            IsAccepted = false,
                            Error = "File already uploaded"
                        });

                        continue;
                    }

                    uploadFileInfos.Add(new UploadFileInfo
                    {
                        Hash = fileHash,
                        Id = photoId,
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
            public string Hash;
            public int Id;
            public bool IsAccepted;
        }
    }
}