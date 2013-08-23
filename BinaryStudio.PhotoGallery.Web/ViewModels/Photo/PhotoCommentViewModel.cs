using System;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Web.ViewModels.Photo
{
    public class PhotoCommentViewModel
    {
        public UserInfoViewModel UserInfo { get; set; }
        public DateTime DateOfCreating { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public int Reply { get; set; }

        public static PhotoCommentViewModel FromModel(PhotoCommentModel photoComment, UserModel user)
        {
            var userInfo = new UserInfoViewModel
            {
                OwnerFirstName = user.FirstName,
                OwnerLastName = user.LastName
            };

            var photoCommentViewModel = new PhotoCommentViewModel
            {
                UserInfo = userInfo,
                Rating = photoComment.Rating,
                DateOfCreating = photoComment.DateOfCreating,
                Reply = photoComment.Reply,
                Text = photoComment.Text
            };

            return photoCommentViewModel;
        }
    }
}