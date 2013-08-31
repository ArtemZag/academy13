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
using BinaryStudio.PhotoGallery.Core.PhotoUtils;
using BinaryStudio.PhotoGallery.Core.UserUtils;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels.Photo;
using BinaryStudio.PhotoGallery.Web.ViewModels.Upload;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
    [Authorize]
    [RoutePrefix("api/upload")]
    public class UploadApiController : BaseApiController
    {
        private readonly IAlbumService _albumService;
        private readonly ICryptoProvider _cryptoProvider;
        private readonly IPhotoProcessor _photoProcessor;
        private readonly ICollageProcessor _collageProcessor;
        private readonly IDirectoryWrapper _directoryWrapper;
        private readonly IFileHelper _fileHelper;
        private readonly IFileWrapper _fileWrapper;
        private readonly IPathUtil _pathUtil;
        private readonly IPhotoService _photoService;

        private const int MAX_PHOTO_SIZE_IN_BYTES = 30 * 1024 * 1024; // 30 MB

        public UploadApiController(
            IPathUtil pathUtil,
            IDirectoryWrapper directoryWrapper,
            IFileHelper fileHelper,
            IFileWrapper fileWrapper,
            IPhotoService photoService,
            IAlbumService albumService,
            ICryptoProvider cryptoProvider,
            IPhotoProcessor photoProcessor,
            ICollageProcessor collageProcessor)
        {
            _pathUtil = pathUtil;
            _directoryWrapper = directoryWrapper;
            _fileHelper = fileHelper;
            _fileWrapper = fileWrapper;
            _photoService = photoService;
            _albumService = albumService;
            _cryptoProvider = cryptoProvider;
            _photoProcessor = photoProcessor;
            _collageProcessor = collageProcessor;
        }

        [POST("move")]
        public HttpResponseMessage Move([FromBody] MovePhotosViewModel viewModel)
        {
            if (viewModel == null || string.IsNullOrEmpty(viewModel.AlbumName) || !viewModel.PhotosId.Any())
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Uknown error");
            }

            var uploadFileInfos = new List<UploadResultViewModel>();

            try
            {
                int albumId;

                try
                {
                    albumId = _albumService.GetAlbumId(User.Id, viewModel.AlbumName);
                }
                catch (AlbumNotFoundException)
                {
                    albumId = _albumService.CreateAlbum(User.Id, viewModel.AlbumName).Id;
                }

                // Get temporary album Id
                int tempAlbumId = _albumService.GetAlbumId(User.Id, "Temporary");

                // Get path to the temporary album folder
                string pathToTempAlbum = _pathUtil.BuildAbsoluteAlbumPath(User.Id, tempAlbumId);

                // Get path to the destination album folder
                string pathToDestAlbum = _pathUtil.BuildAbsoluteAlbumPath(User.Id, albumId);

                if (!_directoryWrapper.Exists(pathToDestAlbum))
                {
                    _directoryWrapper.CreateDirectory(pathToDestAlbum);
                }
                
                foreach (int photoId in viewModel.PhotosId)
                {
                    PhotoModel photoModel = _photoService.GetPhoto(User.Id, photoId);

                    string fileInTempAlbum = string.Format("{0}\\{1}.{2}", pathToTempAlbum, photoModel.Id,
                        photoModel.Format);

                    string fileInDestAlbum = string.Format("{0}\\{1}.{2}", pathToDestAlbum, photoModel.Id,
                        photoModel.Format);

                    bool fileExist = _fileWrapper.Exists(fileInTempAlbum);

                    if (fileExist)
                    {
                        try
                        {
                            _fileWrapper.Move(fileInTempAlbum, fileInDestAlbum);
                        }
                        catch (Exception)
                        {
                            uploadFileInfos.Add(new UploadResultViewModel
                            {
                                Id = photoId,
                                IsAccepted = false,
                                Error = "Can't save photo to selected album"
                            });

                            continue;
                        }

                        photoModel.AlbumModelId = albumId;

                        _photoService.UpdatePhoto(photoModel);

                        // Create thumbnails for photo
                        _photoProcessor.CreateThumbnails(User.Id, albumId, photoModel.Id, photoModel.Format);

                        uploadFileInfos.Add(new UploadResultViewModel
                        {
                            Id = photoId,
                            IsAccepted = true
                        });
                    }
                    else
                    {
                        uploadFileInfos.Add(new UploadResultViewModel
                        {
                            Id = photoId,
                            IsAccepted = false,
                            Error = "Photo is not found in temporary album"
                        });
                    }
                }

                // Create collage for album
                _collageProcessor.CreateCollage(User.Id, albumId, _photoService.GetAllPhotos(User.Id, albumId));
                //todo: we dont use it
                //_collageProcessor.CreateCollage(User.Id,tempAlbumId);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return Request.CreateResponse(HttpStatusCode.OK, uploadFileInfos, new JsonMediaTypeFormatter());
        }

        [POST("")]
        public async Task<HttpResponseMessage> Upload()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            // Key - file name, Value - error of the loading
            var uploadFileInfos = new List<UploadResultViewModel>();

            try
            {
                // Get temporary album Id
                int tempAlbumId = _albumService.GetAlbumId(User.Id, "Temporary");

                // Get path to the temporary album folder
                string pathToTempAlbum = _pathUtil.BuildAbsoluteAlbumPath(User.Id, tempAlbumId);

                // Create directory, if it isn't exist
                if (!_directoryWrapper.Exists(pathToTempAlbum))
                {
                    _directoryWrapper.CreateDirectory(pathToTempAlbum);
                }

                // TODO create this instance with fabrik
                var provider = new MultipartFormDataStreamProvider(pathToTempAlbum);

                // Read the form data from request (save all files in selected folder) TODO must be wrapped too
                await Request.Content.ReadAsMultipartAsync(provider);

                int albumId = _albumService.GetAlbumId(User.Id, "Temporary");

                // Check all uploaded files
                foreach (MultipartFileData fileData in provider.FileData)
                {
                    string originalFileName = fileData.Headers.ContentDisposition.FileName.Trim('"');

                    long fileSize = _fileHelper.GetFileSize(fileData.LocalFileName);

                    string fileHash = _cryptoProvider.GetHash(string.Format("{0}{1}", originalFileName, fileSize));

                    if (fileSize > MAX_PHOTO_SIZE_IN_BYTES)
                    {
                        _fileWrapper.Delete(fileData.LocalFileName);

                        uploadFileInfos.Add(new UploadResultViewModel
                        {
                            Hash = fileHash,
                            IsAccepted = false,
                            Error = "This file contains no image data"
                        });
                        continue;
                    }

                    // Is it really image file format ?
                    if (!_fileHelper.IsImageFile(fileData.LocalFileName))
                    {
                        _fileWrapper.Delete(fileData.LocalFileName);

                        uploadFileInfos.Add(new UploadResultViewModel
                        {
                            Hash = fileHash,
                            IsAccepted = false,
                            Error = "This file contains no image data"
                        });

                        continue;
                    }

                    string format = _fileHelper.GetRealFileFormat(fileData.LocalFileName);

                    var photoModel = _photoService.AddPhoto(PhotoViewModel.ToModel(User.Id, albumId, format));

                    string destFileName = string.Format("{0}\\{1}.{2}", pathToTempAlbum, photoModel.Id, format);

                    try
                    {
                        if (File.Exists(destFileName))
                        {
                            _fileWrapper.Delete(destFileName);
                        }

                        _fileWrapper.Move(fileData.LocalFileName, destFileName);
                    }
                    catch (IOException)
                    {
                        _fileWrapper.Delete(fileData.LocalFileName);

                        uploadFileInfos.Add(new UploadResultViewModel
                        {
                            Hash = fileHash,
                            IsAccepted = false,
                            Error = "File already uploaded"
                        });

                        continue;
                    }

                    // Create thumbnails for photo
                //todo: we dont use this thumbnails
                    //_photoProcessor.CreateThumbnails(User.Id, albumId, photoModel.Id, photoModel.Format);

                    uploadFileInfos.Add(new UploadResultViewModel
                    {
                        Hash = fileHash,
                        Id = photoModel.Id,
                        IsAccepted = true
                    });
                }

                // Create collage for album 'Temporary'
                //todo: we dont use this collages and this executes 'n' times, when n is number of photos
                //_collageProcessor.CreateCollage(User.Id, albumId);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return Request.CreateResponse(HttpStatusCode.OK, uploadFileInfos, new JsonMediaTypeFormatter());
        }
    }
}