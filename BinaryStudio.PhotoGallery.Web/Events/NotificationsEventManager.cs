using System;
using System.Web;
using BinaryStudio.PhotoGallery.Core.PathUtils;
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

    public class NotificationsEventManager : BaseEventManager, INotificationsEventManager
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
            var mUser = _userService.GetUser(mComment.UserId);
            var mPhoto = _photoService.GetPhoto(mUser.Id, mComment.PhotoId);

            if (mPhoto.OwnerId != mComment.UserId)
            {
                var noty = String.Format("Пользователь <span class='highlight_from'>{0} {1}</span> " +
                                         "добавил комментарий к вашей фотографии."
                                         , mUser.FirstName, mUser.LastName);
                _hubNotify.Clients.Group(mPhoto.OwnerId.ToString("d")).SendNotification(NotificationTitles.CommentAdded, noty, _urlUtil.BuildPhotoViewUrl(mPhoto.Id));
            }
        }

        public void PhotoAddedNotify(PhotoModel mPhoto)
        {
            var mAlbum = _albumService.GetAlbum(mPhoto.AlbumId);

            if (mPhoto.OwnerId != mAlbum.OwnerId)
            {
                var mPhotoOwner = _userService.GetUser(mPhoto.OwnerId);
             
                var noty = String.Format("Пользователь <span class='highlight_from'>{0} {1}</span> " +
                                         "добавил фотографию в ваш альбом"
                                         , mPhotoOwner.FirstName, mPhotoOwner.LastName);
                _hubNotify.Clients.Group(mAlbum.OwnerId.ToString("d"))
                          .SendNotification(NotificationTitles.CommentAdded, noty, _urlUtil.BuildPhotoViewUrl(mPhoto.Id));
            }
        }

        public void LikeToPhotoAddedNotify(UserModel mWhoseLike, int photoId)
        {
            var mPhoto = _photoService.GetPhotoWithoutRightsCheck(photoId);

            var noty = String.Format("Пользователь <span class='highlight_from'>{0} {1}</span> " +
                                         "поставил Like вашей фотографии."
                                         , mWhoseLike.FirstName, mWhoseLike.LastName);

            _hubNotify.Clients.Group(mPhoto.OwnerId.ToString("d"))
                          .SendNotification(NotificationTitles.CommentAdded, noty, _urlUtil.BuildPhotoViewUrl(mPhoto.Id));
        }

        public void SomeoneRepliedToComment(PhotoCommentModel mComment)
        {
            var mWhoseComment = _userService.GetUser(mComment.UserId);
            var mParentComment = _commentService.GetPhotoComment(mComment.Reply);

            var noty = String.Format("Пользователь <span class='highlight_from'>{0} {1}</span> " +
                                       "ответил на ваш комментарий к фотографии."
                                       , mWhoseComment.FirstName, mWhoseComment.LastName);

            _hubNotify.Clients.Group(mParentComment.UserId.ToString("d"))
                          .SendNotification(NotificationTitles.CommentAdded, noty, _urlUtil.BuildCommentUrl(mComment.PhotoId, mComment.Id));
        }

    }
}