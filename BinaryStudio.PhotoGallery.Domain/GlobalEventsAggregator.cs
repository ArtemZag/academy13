using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain
{
    public delegate void CommentAddedHandler(PhotoCommentModel mComment);
    public delegate void PhotoAddedHandler(PhotoModel mPhoto);
    public delegate void LikeToPhotoAddedHandler(PhotoModel mPhoto);

    public interface IGlobalEventsAggregator
    {
        event CommentAddedHandler CommentAdded;
        event PhotoAddedHandler PhotoAdded;
        event LikeToPhotoAddedHandler LikeToPhotoAdded;

        void PushCommentAddedEvent(PhotoCommentModel mComment);
        void PushPhotoAddedEvent(PhotoModel mModel);
        void PushLikeToPhotoAddedEvent(PhotoModel mModel);
    }

    public class GlobalEventsAggregator : IGlobalEventsAggregator
    {
        public event CommentAddedHandler CommentAdded;
        public event PhotoAddedHandler PhotoAdded;
        public event LikeToPhotoAddedHandler LikeToPhotoAdded;

        private static GlobalEventsAggregator instance;

        public static GlobalEventsAggregator Instance
        {
            get { return instance ?? (instance = new GlobalEventsAggregator()); }
        }

        public void PushCommentAddedEvent(PhotoCommentModel mComment)
        {
            CommentAddedHandler handler = CommentAdded;
            if (handler != null) handler(mComment);
        }

        public void PushPhotoAddedEvent(PhotoModel mPhoto)
        {
            PhotoAddedHandler handler = PhotoAdded;
            if (handler != null) handler(mPhoto);
        }

        public void PushLikeToPhotoAddedEvent(PhotoModel mModel)
        {
            LikeToPhotoAddedHandler handler = LikeToPhotoAdded;
            if (handler != null) handler(mModel);
        }
    }

}
