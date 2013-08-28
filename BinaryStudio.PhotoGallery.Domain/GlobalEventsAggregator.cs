using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain
{
    public delegate void CommentAddedHandler(PhotoCommentModel mComment);
    public delegate void PhotoAddedHandler(PhotoModel mPhoto);
    public delegate void LikeToPhotoAddedHandler(UserModel user, int photoId);
    public delegate void SomeoneRepliedToCommentHandler(PhotoCommentModel mComment);

    public interface IGlobalEventsAggregator
    {
        event CommentAddedHandler CommentAdded;
        event PhotoAddedHandler PhotoAdded;
        event LikeToPhotoAddedHandler LikeToPhotoAdded;
        event SomeoneRepliedToCommentHandler SomeoneRepliedToComment;

        void PushCommentAddedEvent(PhotoCommentModel mComment);
        void PushPhotoAddedEvent(PhotoModel mModel);
        void PushLikeToPhotoAddedEvent(UserModel user, int photoId);
    }

    public class GlobalEventsAggregator : IGlobalEventsAggregator
    {
        public event CommentAddedHandler CommentAdded;
        public event PhotoAddedHandler PhotoAdded;
        public event LikeToPhotoAddedHandler LikeToPhotoAdded;
        public event SomeoneRepliedToCommentHandler SomeoneRepliedToComment;


        private static GlobalEventsAggregator instance;

        public static GlobalEventsAggregator Instance
        {
            get { return instance ?? (instance = new GlobalEventsAggregator()); }
        }

        public void PushCommentAddedEvent(PhotoCommentModel mComment)
        {
            CommentAddedHandler handler = CommentAdded;
            if (handler != null) handler(mComment);
            if (mComment.Reply != 0)
            {
                SomeoneRepliedToCommentHandler handlerTwo = SomeoneRepliedToComment;
                if (handler != null) handlerTwo(mComment);
            }
        }

        public void PushPhotoAddedEvent(PhotoModel mPhoto)
        {
            PhotoAddedHandler handler = PhotoAdded;
            if (handler != null) handler(mPhoto);
        }

        public void PushLikeToPhotoAddedEvent(UserModel user, int photoId)
        {
            LikeToPhotoAddedHandler handler = LikeToPhotoAdded;
            if (handler != null) handler(user, photoId);
        }

    }

}
