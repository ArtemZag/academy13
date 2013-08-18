﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Services.Search.Results;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services.Search
{
    internal class PhotoSearchService : DbService, IPhotoSearchService
    {
        private readonly ISecureService secureService;

        public PhotoSearchService(IUnitOfWorkFactory workFactory, ISecureService secureService) : base(workFactory)
        {
            this.secureService = secureService;
        }

        public IEnumerable<IFound> Search(SearchArguments searchArguments)
        {
            var result = new List<PhotoFound>();

            IEnumerable<string> searchWords = searchArguments.SearchQuery.SplitSearchString();

            IEnumerable<AlbumModel> avialableAlbums = secureService.GetAvailableAlbums(searchArguments.UserId);

            if (searchArguments.IsSearchPhotosByDescription)
            {
                IEnumerable<PhotoFound> found = SearchByDescription(avialableAlbums, searchWords);

                result.AddRange(found);
            }

            if (searchArguments.IsSearchPhotosByTags)
            {
                IEnumerable<PhotoFound> found = SearchByTags(avialableAlbums, searchWords);

                result.AddRange(found);
            }

            return Group(result);
        }

        private IEnumerable<PhotoFound> SearchByDescription(IEnumerable<AlbumModel> fromAlbums,
            IEnumerable<string> searchWords)
        {
            var result = new List<PhotoFound>();

            foreach (AlbumModel album in fromAlbums)
            {
                IEnumerable<PhotoFound> found =
                    album.Photos.Where(
                        model => searchWords.Any(model.Description.ToLower().Contains) && !model.IsDeleted)
                        .Select(model => new PhotoFound
                        {
                            Id = model.Id,
                            OwnerId = model.OwnerId,
                            AlbumId = model.AlbumId,
                            Format = model.Format,
                            Rating = model.Rating,
                            DateOfCreation = model.DateOfCreation,
                            Relevance = CalculateRelevanceByDescription(searchWords, model)
                        });

                result.AddRange(found);
            }

            return result;
        }

        private IEnumerable<PhotoFound> SearchByTags(IEnumerable<AlbumModel> fromAlbums, IEnumerable<string> searchWords)
        {
            var result = new List<PhotoFound>();

            foreach (AlbumModel albumModel in fromAlbums)
            {
                IEnumerable<PhotoFound> found = albumModel.Photos.Where(
                    model => model.PhotoTags.Any(tagModel => searchWords.Any(tagModel.TagName.ToLower().Contains)))
                    .Select(model => new PhotoFound
                    {
                        Id = model.Id,
                        OwnerId = model.OwnerId,
                        AlbumId = model.AlbumId,
                        Format = model.Format,
                        Rating = model.Rating,
                        DateOfCreation = model.DateOfCreation,
                        Relevance = 1
                    });

                result.AddRange(found);
            }

            return result;
        }

        private int CalculateRelevanceByDescription(IEnumerable<string> searchWords, PhotoModel photoModel)
        {
            return searchWords.Sum(searchWord => Regex.Matches(photoModel.Description.ToLower(), searchWord).Count);
        }

        private IEnumerable<IFound> Group(IEnumerable<PhotoFound> data)
        {
            return
                data.GroupBy(
                    item => new {item.Id, item.OwnerId, item.AlbumId, item.Rating, item.DateOfCreation, item.Format})
                    .Select(items => new PhotoFound
                    {
                        Id = items.Key.Id,
                        OwnerId = items.Key.OwnerId,
                        AlbumId = items.Key.AlbumId,
                        Format = items.Key.Format,
                        Rating = items.Key.Rating,
                        DateOfCreation = items.Key.DateOfCreation,
                        Relevance = items.Sum(item => item.Relevance)
                    });
        }
    }
}