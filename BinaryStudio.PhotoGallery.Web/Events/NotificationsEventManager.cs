using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BinaryStudio.PhotoGallery.Core.PathUtils;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.Hubs;
using Microsoft.AspNet.SignalR;

namespace BinaryStudio.PhotoGallery.Web.Events
{
    public interface INotificationsEventManager
    {
        void PhotoCommentAddedNotify(PhotoCommentModel mComment);
        void PhotoAddedNotify(PhotoModel mPhoto);
        void LikeToPhotoAddedNotify(UserModel mWhoseLike, int photoId);
        void SomeoneRepliedToComment(PhotoCommentModel mComment);
    }

    public class NotificationsEventManager : INotificationsEventManager
    {
        private readonly IUserService _userService;
        private readonly IPhotoService _photoService;
        private readonly IHubContext _hubNotify;
        private readonly IUrlUtil _urlUtil;
        private readonly IAlbumService _albumService;
        private readonly IPhotoCommentService _commentService;

        public NotificationsEventManager(IUserService userService, IPhotoService photoService,
            IAlbumService albumService, IUrlUtil urlUtil, IPhotoCommentService commentService)
        {
            _userService = userService;
            _photoService = photoService;
            _hubNotify = GlobalHost.ConnectionManager.GetHubContext<NotificationsHub>();
            _urlUtil = urlUtil;
            _albumService = albumService;
            _commentService = commentService;
        }


        public void PhotoCommentAddedNotify(PhotoCommentModel mComment)
        {
            // TODO by Mikhail: mComment.UserId and mUser.Id are the same always 
            var mUser = _userService.GetUser(mComment.UserId); // TODO this is a redundant line
            var mPhoto = _photoService.GetPhoto(mUser.Id, mComment.PhotoId);

            if (mPhoto.OwnerId != mComment.UserId)
            {
                var mPhotoOwner = _userService.GetUser(mPhoto.OwnerId);

                mPhotoOwner.Email = mPhotoOwner.Email.ToLower(); // todo: remove and refactor, when id to cookies will be added

                var noty = String.Format("Пользователь <span class='highlight_from'>{0} {1}</span> " +
                                         "добавил комментарий к вашей фотографии."
                                         , mUser.FirstName, mUser.LastName);
                _hubNotify.Clients.Group(mPhotoOwner.Email).SendNotification(NotificationTitles.CommentAdded, noty, _urlUtil.BuildPhotoViewUrl(mPhoto.Id));
            }
        }

        public void PhotoAddedNotify(PhotoModel mPhoto)
        {
            var mAlbum = _albumService.GetAlbum(mPhoto.AlbumId);

            if (mPhoto.OwnerId != mAlbum.OwnerId)
            {
                var mPhotoOwner = _userService.GetUser(mPhoto.OwnerId);
                var mAlbumOwner = _userService.GetUser(mAlbum.OwnerId);

                mAlbumOwner.Email = mAlbumOwner.Email.ToLower(); // todo: remove and refactor, when id to cookies will be added

                var noty = String.Format("Пользователь <span class='highlight_from'>{0} {1}</span> " +
                                         "добавил фотографию в ваш альбом {2}"
                                         , mPhotoOwner.FirstName, mPhotoOwner.LastName, mPhoto.Name);
                _hubNotify.Clients.Group(mAlbumOwner.Email)
                          .SendNotification(NotificationTitles.CommentAdded, noty, _urlUtil.BuildPhotoViewUrl(mPhoto.Id));
            }
        }

        public void LikeToPhotoAddedNotify(UserModel mWhoseLike, int photoId)
        {
            var mPhoto = _photoService.GetPhotoWithoutRightsCheck(photoId);

            var mPhotoOwner = _userService.GetUser(mPhoto.OwnerId);
            mPhotoOwner.Email = mPhotoOwner.Email.ToLower(); // todo: remove and refactor, when id to cookies will be added

            var noty = String.Format("Пользователь <span class='highlight_from'>{0} {1}</span> " +
                                         "поставил Like вашей фотографии."
                                         , mWhoseLike.FirstName, mWhoseLike.LastName);
            _hubNotify.Clients.Group(mPhotoOwner.Email)
                          .SendNotification(NotificationTitles.CommentAdded, noty, _urlUtil.BuildPhotoViewUrl(mPhoto.Id));
        }

        public void SomeoneRepliedToComment(PhotoCommentModel mComment)
        {
            var mWhoseComment = _userService.GetUser(mComment.UserId);

            var mParentComment = _commentService.GetPhotoComment(mComment.Reply);

            var mParentCommentOwner = _userService.GetUser(mParentComment.UserId);
            mParentCommentOwner.Email = mParentCommentOwner.Email.ToLower(); // todo: remove and refactor, when id to cookies will be added

            var noty = String.Format("Пользователь <span class='highlight_from'>{0} {1}</span> " +
                                       "ответил на ваш комментарий к фотографии."
                                       , mWhoseComment.FirstName, mWhoseComment.LastName);
            _hubNotify.Clients.Group(mParentCommentOwner.Email)
                          .SendNotification(NotificationTitles.CommentAdded, noty, _urlUtil.BuildCommentUrl(mComment.PhotoId, mComment.Id));
        }

    }
}