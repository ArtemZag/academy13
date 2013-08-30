﻿using System;
using BinaryStudio.PhotoGallery.Core.PhotoUtils;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels.Photo;

namespace BinaryStudio.PhotoGallery.Web.Extensions.ViewModels
{
    public static class PhotoConvertionExtensions
    {
        public static PhotoCommentViewModel ToPhotoCommentViewModel(this PhotoCommentModel photoComment, UserModel user)
        {
            var userInfo = new UserInfoViewModel
            {
                OwnerFirstName = user.FirstName,
                OwnerLastName = user.LastName,
            };
            userInfo.OwnerPhotoSource = userInfo.PathUtil.BuildAvatarPath(user.Id, ImageSize.Small);
            var d = photoComment.DateOfCreating;
            var photoCommentViewModel = new PhotoCommentViewModel
            {
                UserInfo = userInfo,
                Rating = photoComment.Rating,
                year = d.Year,
                month = d.Month,
                day = d.Day,
                hour = d.Hour,
                minute = d.Minute,
                second = d.Second,
                Reply = photoComment.Reply,
                Text = photoComment.Text
            };

            return photoCommentViewModel;
        }

        public static PhotoLikeViewModel ToPhotoLikeViewModel(this UserModel userModel)
        {
            var viewModel = new PhotoLikeViewModel
            {
                FirstName = userModel.FirstName,
                LastName = userModel.LastName
            };

            return viewModel;
        }

        public static PhotoViewModel ToPhotoViewModel(this PhotoModel photoModel)
        {
            var viewModel = new PhotoViewModel
            {
                AlbumId = photoModel.AlbumId,
                PhotoId = photoModel.Id
            };

            viewModel.PhotoSource = 
                viewModel.PathUtil.BuildOriginalPhotoPath(photoModel.OwnerId, photoModel.AlbumId, photoModel.Id, photoModel.Format);

            // todo owner
            viewModel.PhotoThumbSource = viewModel.PathUtil.BuildThumbnailPath(photoModel.OwnerId, photoModel.AlbumId,
                photoModel.Id, photoModel.Format, ImageSize.Medium);

            viewModel.PhotoViewPageUrl = viewModel.UrlUtil.BuildPhotoViewUrl(photoModel.Id);

            return viewModel;
        }
    }
}