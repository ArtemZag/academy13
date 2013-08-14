using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain
{
    public delegate void CommentAddedHandler(PhotoCommentModel mComment);
    public delegate void PhotoAddedHandler();

    public interface IGlobalEventsAggregator
    {
        event CommentAddedHandler CommentAdded;
        event PhotoAddedHandler PhotoAdded;

        void PushCommentAddedEvent(PhotoCommentModel phCommentModel);
        void PushPhotoAddedEvent(PhotoModel phModel);
    }

    public class GlobalEventsAggregator : IGlobalEventsAggregator
    {
        public event CommentAddedHandler CommentAdded;
        public event PhotoAddedHandler PhotoAdded;

        private static GlobalEventsAggregator _instance;

        public static GlobalEventsAggregator Instance
        {
            get { return _instance ?? (_instance = new GlobalEventsAggregator()); }
        }

        public void PushCommentAddedEvent(PhotoCommentModel mComment)
        {
            CommentAddedHandler handler = CommentAdded;
            if (handler != null) handler(mComment);
        }

        public void PushPhotoAddedEvent(PhotoModel phModel)
        {
            PhotoAddedHandler handler = PhotoAdded;
            if (handler != null) handler();
        }
    }

}
