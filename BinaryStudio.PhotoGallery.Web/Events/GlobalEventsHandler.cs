using System;
using BinaryStudio.PhotoGallery.Core.PathUtils;
using BinaryStudio.PhotoGallery.Domain;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.Hubs;
using Microsoft.AspNet.SignalR;

namespace BinaryStudio.PhotoGallery.Web.Events
{
    public interface IGlobalEventsHandler
    {}

    public class GlobalEventsHandler : IGlobalEventsHandler
    {
        private static GlobalEventsHandler _instance;
        private readonly IGlobalEventsAggregator _eventsAggregator;
        private readonly IUserService _userService;
        private readonly IPhotoService _photoService;
        private readonly IHubContext _hubNotify;
        private readonly IUrlUtil _urlUtil;
        private readonly IAlbumService _albumService;
        private readonly IPhotoCommentService _commentService;

        public GlobalEventsHandler(){}


        // todo: refactor all methods it this class by the some html template
        // todo: refactor when id will be added to cookies to reduce database requests
        public GlobalEventsHandler(IGlobalEventsAggregator eventsAggregator, IUserService userService, IPhotoService photoService, 
            IAlbumService albumService, IUrlUtil urlUtil, IPhotoCommentService commentService)
        {
            _eventsAggregator = eventsAggregator;
            _userService = userService;
            _photoService = photoService;
            _hubNotify = GlobalHost.ConnectionManager.GetHubContext<NotificationsHub>();
            _urlUtil = urlUtil;
            _albumService = albumService;
            _commentService = commentService;

            //subscribe to events
            _eventsAggregator.CommentAdded += PhotoCommentAddedCaused;
            _eventsAggregator.PhotoAdded += PhotoAddedCaused;
            _eventsAggregator.LikeToPhotoAdded += LikeToPhotoAddedCaused;
            _eventsAggregator.SomeoneRepliedToComment += SomeoneRepliedToCommentCaused;
        }

        public void PhotoCommentAddedCaused(PhotoCommentModel mComment)
        {

            var mUser = _userService.GetUser(mComment.UserModelId);
            var mPhoto = _photoService.GetPhoto(mUser.Email, mComment.PhotoModelId);

            if (mPhoto.OwnerId != mComment.UserModelId)
            {
                var mPhotoOwner = _userService.GetUser(mPhoto.OwnerId);

                mPhotoOwner.Email = mPhotoOwner.Email.ToLower(); // todo: remove and refactor, when id to cookies will be added

                var noty = String.Format("Пользователь <span class='highlight_from'>{0} {1}</span> " +
                                         "добавил комментарий к вашей фотографии."
                                         , mUser.FirstName, mUser.LastName);
                _hubNotify.Clients.Group(mPhotoOwner.Email).SendNotification(NotificationTitles.CommentAdded, noty, _urlUtil.BuildPhotoViewUrl(mPhoto.Id));
            }
        }

        public void PhotoAddedCaused(PhotoModel mPhoto)
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

        public void LikeToPhotoAddedCaused(UserModel mWhoseLike, int photoId)
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

        public void SomeoneRepliedToCommentCaused(PhotoCommentModel mComment)
        {
            var mWhoseComment = _userService.GetUser(mComment.UserModelId);

            var mParentComment =  _commentService.GetPhotoComment(mComment.Reply);

            var mParentCommentOwner = _userService.GetUser(mParentComment.UserModelId);
            mParentCommentOwner.Email = mParentCommentOwner.Email.ToLower(); // todo: remove and refactor, when id to cookies will be added

            var noty = String.Format("Пользователь <span class='highlight_from'>{0} {1}</span> " +
                                       "ответил на ваш комментарий к фотографии."
                                       , mWhoseComment.FirstName, mWhoseComment.LastName);
            _hubNotify.Clients.Group(mParentCommentOwner.Email)
                          .SendNotification(NotificationTitles.CommentAdded, noty, _urlUtil.BuildCommentUrl(mComment.PhotoModelId ,mComment.Id));
        }

        public static GlobalEventsHandler Instance
        {
            get { return _instance ?? (_instance = new GlobalEventsHandler()); }
        }
    }

}