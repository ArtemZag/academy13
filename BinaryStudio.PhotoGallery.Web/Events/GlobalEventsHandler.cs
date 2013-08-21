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
        private readonly INotificationsEventManager _notyEventManager;

        public GlobalEventsHandler(){}


        // todo: refactor all methods it this class by the some html template
        // todo: refactor when id will be added to cookies to reduce database requests
        public GlobalEventsHandler(IGlobalEventsAggregator eventsAggregator, INotificationsEventManager notyEventManager)
        {
            _eventsAggregator = eventsAggregator;
            _notyEventManager = notyEventManager;

            //subscribe to events
            _eventsAggregator.CommentAdded += PhotoCommentAddedCaused;
            _eventsAggregator.PhotoAdded += PhotoAddedCaused;
            _eventsAggregator.LikeToPhotoAdded += LikeToPhotoAddedCaused;
            _eventsAggregator.SomeoneRepliedToComment += SomeoneRepliedToCommentCaused;
        }

        public void PhotoCommentAddedCaused(PhotoCommentModel mComment)
        {
            _notyEventManager.PhotoCommentAddedNotify(mComment);
        }

        public void PhotoAddedCaused(PhotoModel mPhoto)
        {
           _notyEventManager.PhotoAddedNotify(mPhoto);
        }

        public void LikeToPhotoAddedCaused(UserModel mWhoseLike, int photoId)
        {
            _notyEventManager.LikeToPhotoAddedNotify(mWhoseLike, photoId);
        }

        public void SomeoneRepliedToCommentCaused(PhotoCommentModel mComment)
        {
           _notyEventManager.SomeoneRepliedToComment(mComment);
        }

        public static GlobalEventsHandler Instance
        {
            get { return _instance ?? (_instance = new GlobalEventsHandler()); }
        }
    }

}