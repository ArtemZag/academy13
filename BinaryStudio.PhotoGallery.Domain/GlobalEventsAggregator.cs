using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain
{
    public interface IGlobalEventsAggregator
    {
        void PushCommentAddedEvent(PhotoCommentModel phCommentModel);
        void PushPhotoAddedEvent(PhotoModel phModel);
    }

    public class GlobalEventsAggregator : IGlobalEventsAggregator
    {
        public delegate void CommentAddedHandler();

        public event CommentAddedHandler CommentAdded;

        private static GlobalEventsAggregator _instance;

        private GlobalEventsAggregator() { }

        public static GlobalEventsAggregator Instance
        {
            get { return _instance ?? (_instance = new GlobalEventsAggregator()); }
        }

        public void PushCommentAddedEvent(PhotoCommentModel phCommentModel)
        {
            CommentAddedHandler handler = CommentAdded;
            if (handler != null) handler();
        }

        public void PushPhotoAddedEvent(PhotoModel phModel)
        {
            throw new NotImplementedException();
        }
    }

}
