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
        private static GlobalEventsHandler instance;
        private readonly IGlobalEventsAggregator eventsAggregator;
        private readonly IUserService userService;
        private readonly IPhotoService photoService;
        private readonly IHubContext hubNotify;
        private readonly IUrlUtil urlUtil;
        private readonly IAlbumService albumService;

        public GlobalEventsHandler(){}

        public GlobalEventsHandler(IGlobalEventsAggregator eventsAggregator, IUserService userService, IPhotoService photoService, 
            IAlbumService albumService, IUrlUtil urlUtil)
        {
            this.eventsAggregator = eventsAggregator;
            this.userService = userService;
            this.photoService = photoService;
            this.hubNotify = GlobalHost.ConnectionManager.GetHubContext<NotificationsHub>();
            this.urlUtil = urlUtil;
            this.albumService = albumService;

            //subscribe to events
            this.eventsAggregator.CommentAdded += PhotoCommentAddedCaused;
            this.eventsAggregator.PhotoAdded += PhotoAddedCaused;
            this.eventsAggregator.LikeToPhotoAdded += LikeToPhotoAddedCaused;
            this.eventsAggregator.SomeoneRepliedToComment += SomeoneRepliedToCommentCaused;
        }

        public void PhotoCommentAddedCaused(PhotoCommentModel mComment)
        {

            var mUser = userService.GetUser(mComment.UserModelId);
            var mPhoto = photoService.GetPhoto(mUser.Email, mComment.PhotoModelId);

            if (mPhoto.OwnerId != mComment.UserModelId)
            {
                var mPhotoOwner = userService.GetUser(mPhoto.OwnerId);

                mPhotoOwner.Email = mPhotoOwner.Email.ToLower(); // todo: remove and refactor, when id to cookies will be added

                var noty = String.Format("Пользователь <span class='highlight_from'>{0} {1}</span> " +
                                         "добавил комментарий к вашей <span class='highlight_what'>фотографии</span>."
                                         , mUser.FirstName, mUser.LastName);
                hubNotify.Clients.Group(mPhotoOwner.Email).SendNotification(NotificationTitles.CommentAdded, noty, urlUtil.BuildPhotoViewUrl(mPhoto.Id));
            }
        }

        public void PhotoAddedCaused(PhotoModel mPhoto)
        {
            var mAlbum = albumService.GetAlbum(mPhoto.AlbumId);

            if (mPhoto.OwnerId != mAlbum.OwnerId)
            {
                var mPhotoOwner = userService.GetUser(mPhoto.OwnerId);
                var mAlbumOwner = userService.GetUser(mAlbum.OwnerId);

                mAlbumOwner.Email = mAlbumOwner.Email.ToLower(); // todo: remove and refactor, when id to cookies will be added

                var noty = String.Format("Пользователь <span class='highlight_from'>{0} {1}</span> " +
                                         "добавил <span class='highlight_what'>фотографию</span> в ваш альбом {2}"
                                         , mPhotoOwner.FirstName, mPhotoOwner.LastName, mPhoto.Name);
                hubNotify.Clients.Group(mAlbumOwner.Email)
                          .SendNotification(NotificationTitles.CommentAdded, noty, urlUtil.BuildPhotoViewUrl(mPhoto.Id));
            }
        }

        public void LikeToPhotoAddedCaused(UserModel mWhoseLike, int photoId)
        {
            var mPhoto = photoService.GetPhotoWithoutRightsCheck(photoId);
            var mPhotoOwner = userService.GetUser(mPhoto.OwnerId);

            mPhotoOwner.Email = mPhotoOwner.Email.ToLower(); // todo: remove and refactor, when id to cookies will be added

            var noty = String.Format("Пользователь <span class='highlight_from'>{0} {1}</span> " +
                                         "поставил Like вашей <span class='highlight_what'>фотографии</span>."
                                         , mWhoseLike.FirstName, mWhoseLike.LastName);
            hubNotify.Clients.Group(mPhotoOwner.Email)
                          .SendNotification(NotificationTitles.CommentAdded, noty, urlUtil.BuildPhotoViewUrl(mPhoto.Id));
        }

        public void SomeoneRepliedToCommentCaused(PhotoCommentModel mComment)
        {
            
        }

        public static GlobalEventsHandler Instance
        {
            get { return instance ?? (instance = new GlobalEventsHandler()); }
        }
    }

}