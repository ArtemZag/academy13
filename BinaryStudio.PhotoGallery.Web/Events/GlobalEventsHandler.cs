using System;
using BinaryStudio.PhotoGallery.Core.PathUtils;
using BinaryStudio.PhotoGallery.Domain;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.Hubs;
using Microsoft.AspNet.SignalR;

namespace BinaryStudio.PhotoGallery.Web.Utils
{
    public interface IGlobalEventsHandler
    {

    }

    public class GlobalEventsHandler : IGlobalEventsHandler
    {
        private static GlobalEventsHandler _instance;
        private readonly IGlobalEventsAggregator _eventsAggregator;
        private readonly IUserService _userService;
        private readonly IPhotoService _photoService;
        private static IHubContext _hubNotify;
        private readonly IUrlUtil _urlUtil;
        private readonly IAlbumService _albumService;

        public GlobalEventsHandler(){}

        public GlobalEventsHandler(IGlobalEventsAggregator eventsAggregator, IUserService userService, IPhotoService photoService, 
            IAlbumService albumService, IUrlUtil urlUtil)
        {
            _eventsAggregator = eventsAggregator;
            _userService = userService;
            _photoService = photoService;
            _hubNotify = GlobalHost.ConnectionManager.GetHubContext<NotificationsHub>();
            _urlUtil = urlUtil;
            _albumService = albumService;

            //subscribe to events
            _eventsAggregator.CommentAdded += PhotoCommentAddedCaused;
            _eventsAggregator.PhotoAdded += PhotoAddedCaused;
            _eventsAggregator.LikeToPhotoAdded += LikeToPhotoAddedCaused;
        }

        public void PhotoCommentAddedCaused(PhotoCommentModel mComment)
        {
            var mUser = _userService.GetUser(mComment.UserId);
            var mPhoto = _photoService.GetPhoto(mUser.Email, mComment.PhotoId);

            if (mPhoto.OwnerId != mComment.UserId)
            {
                var mPhotoOwner = _userService.GetUser(mPhoto.OwnerId);
                var _hubNotify = GlobalHost.ConnectionManager.GetHubContext<NotificationsHub>();
                var noty = String.Format("Пользователь <span class='highlight_from'>{0} {1}</span> " +
                                         "добавил комментарий к фотографии <span class='highlight_what'>\"{2}\"</span>"
                                         , mUser.FirstName, mUser.LastName, mPhoto.Name);
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

                var _hubNotify = GlobalHost.ConnectionManager.GetHubContext<NotificationsHub>();
                var noty = String.Format("Пользователь <span class='highlight_from'>{0} {1}</span> " +
                                         "добавил фотографию <span class='highlight_what'>\"{2}\"</span> в ваш альбом {3}"
                                         , mPhotoOwner.FirstName, mPhotoOwner.LastName, mPhoto.Name, mAlbum.Name);
                _hubNotify.Clients.Group(mAlbumOwner.Email)
                          .SendNotification(NotificationTitles.CommentAdded, noty, _urlUtil.BuildPhotoViewUrl(mPhoto.Id));
            }
        }

        public void LikeToPhotoAddedCaused(PhotoModel mPhoto)
        {
            
        }

        public static GlobalEventsHandler Instance
        {
            get { return _instance ?? (_instance = new GlobalEventsHandler()); }
        }
    }

}