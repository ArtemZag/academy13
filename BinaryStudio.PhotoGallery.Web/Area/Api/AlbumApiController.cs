﻿using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Core.PathUtils;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.Extensions.ViewModels;
using BinaryStudio.PhotoGallery.Web.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
    [Authorize]
    [RoutePrefix("api/album")]
    public class AlbumApiController : BaseApiController
    {
        private readonly IAlbumService _albumService;
        private readonly IPathUtil _pathUtil;

        public AlbumApiController(IAlbumService albumService, IPathUtil pathUtil)
        {
            _albumService = albumService;
            _pathUtil = pathUtil;
        }

        [GET("{albumId: int}")]
        public HttpResponseMessage GetAlbum(int albumId)
        {
            try
            {
                AlbumViewModel result = _albumService.GetAlbum(albumId).ToAlbumViewModel();

                result.CollagePath = _pathUtil.BuildCollagePath(result.OwnerId, result.Id);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [GET("?{userId:int}&{skip:int}&{take:int}")]
        public HttpResponseMessage GetAlbums(int userId, int skip, int take)
        {
            try
            {
                var albums = _albumService
                    .GetAlbumsRange(userId, skip, take)
                    .Select(album => album.ToAlbumViewModel(
                        _pathUtil.GetCollage(userId, album.Id))).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, albums, new JsonMediaTypeFormatter());
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [POST("")]
        public HttpResponseMessage PostAlbumInfo(AlbumViewModel albumViewModel)
        {
            try
            {
                AlbumModel album = _albumService.GetAlbum(albumViewModel.Id);

                album.Name = albumViewModel.AlbumName;
                album.Description = albumViewModel.Description;
                
                _albumService.UpdateAlbum(album);

                return Request.CreateResponse(HttpStatusCode.Accepted);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            
        }

        [POST("")]
        public HttpResponseMessage Create([FromBody] string albumName)
        {
            if (albumName == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Unknown error");
            }
            try
            {
                _albumService.CreateAlbum(User.Id, albumName);
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (AlbumAlreadyExistException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [GET("all/name")]
        public HttpResponseMessage GetAllNames()
        {
            try
            {
                var albumNames = _albumService
                    .GetAllAlbums(User.Id)
                    .Where(album => album.Name != "Temporary")
                    .Select(album => album.Name)
                    .ToList();

                return Request.CreateResponse(HttpStatusCode.OK, albumNames, new JsonMediaTypeFormatter());
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}