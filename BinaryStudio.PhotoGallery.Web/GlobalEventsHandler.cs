using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.Hubs;


namespace BinaryStudio.PhotoGallery.Web
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

        public GlobalEventsHandler(){}

        public GlobalEventsHandler(IGlobalEventsAggregator eventsAggregator, IUserService userService, IPhotoService photoService)
        {
            _eventsAggregator = eventsAggregator;
            _userService = userService;
            _eventsAggregator.CommentAdded += PhotoCommentAddedCaused;
            _photoService = photoService;
        }

        public void PhotoCommentAddedCaused(PhotoCommentModel mComment)
        {
            var mUser = _userService.GetUser(mComment.UserModelId);
            var mPhoto = _photoService.GetPhoto(mUser.Email, mComment.Id);
            //var noty = String.Format("Пользователь {0} {1} добавил комментарий к фотографии {2}", mUser.FirstName, mUser.LastName, ");
        }

        public static GlobalEventsHandler Instance
        {
            get { return _instance ?? (_instance = new GlobalEventsHandler()); }
        }
    }

}