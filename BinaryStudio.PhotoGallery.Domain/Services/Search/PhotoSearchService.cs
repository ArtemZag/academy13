﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Services.Search.Results;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services.Search
{
    internal class PhotoSearchService : DbService, IPhotoSearchService
    {
        public PhotoSearchService(IUnitOfWorkFactory workFactory) : base(workFactory)
        {
        }

        public IEnumerable<IFound> Search(SearchArguments searchArguments)
        {
            var result = new List<PhotoFound>();

            string searchQuery = searchArguments.SearchQuery;

            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                if (searchArguments.IsSearchPhotosByName)
                {
                    IEnumerable<PhotoFound> found = SearchByCondition(searchQuery, unitOfWork,
                                                                          model =>
                                                                          model.PhotoName.Contains(searchQuery) &&
                                                                          !model.IsDeleted,
                                                                          GetRelevanceByName);

                    result.AddRange(found);
                }

                if (searchArguments.IsSearchPhotosByDescription)
                {
                    IEnumerable<PhotoFound> found = SearchByCondition(searchQuery, unitOfWork,
                                                                          model =>
                                                                          model.Description.Contains(searchQuery) &&
                                                                          !model.IsDeleted,
                                                                          GetRelevanceByDescription);

                    result.AddRange(found);
                }

                if (searchArguments.IsSearchPhotosByTags)
                {
                    IEnumerable<PhotoFound> found = SearchByTags(searchQuery, unitOfWork);

                    result.AddRange(found);
                }
            }

            return Group(result);
        }

        private IEnumerable<IFound> Group(IEnumerable<PhotoFound> data)
        {
            return
                data.GroupBy(item => new {item.Id, item.AuthorId, item.AlbumId, item.Rating, item.PhotoName})
                    .Select(items => new PhotoFound
                        {
                            Id = items.Key.Id,
                            AlbumId = items.Key.AlbumId,
                            Rating = items.Key.Rating,
                            PhotoName = items.Key.PhotoName,
                            Relevance = items.Sum(item => item.Relevance)
                        });
        }

        private IEnumerable<PhotoFound> SearchByCondition(string searchQuery, IUnitOfWork unitOfWork,
                                                              Expression<Func<PhotoModel, bool>> predicate,
                                                              Func<string, PhotoModel, int> getRelevance)
        {
            return unitOfWork.Photos.Filter(predicate).ToList().Select(model => new PhotoFound
                {
                    Id = model.Id,
                    AuthorId = model.UserId,
                    AlbumId = model.AlbumId,
                    PhotoName = model.PhotoName,
                    Rating = model.Rating,
                    Relevance = getRelevance(searchQuery, model)
                });
        }

        private IEnumerable<PhotoFound> SearchByTags(string searchQuery, IUnitOfWork unitOfWork)
        {
            var result = new List<PhotoFound>();

            List<PhotoTagModel> foundTags =
                unitOfWork.PhotoTags.Filter(model => model.TagName.Contains(searchQuery)).ToList();

            foreach (PhotoTagModel tag in foundTags)
            {
                IEnumerable<PhotoModel> presentPhotos =
                    tag.PhotoModels.Select(model => model).Where(model => !model.IsDeleted);

                IEnumerable<PhotoFound> tagPhotos = presentPhotos.Select(model => new PhotoFound
                    {
                        Id = model.Id,
                        AuthorId = model.UserId,
                        AlbumId = model.AlbumId,
                        PhotoName = model.PhotoName,
                        Rating = model.Rating,
                        Relevance = 1
                    });

                result.AddRange(tagPhotos);
            }

            return result;
        }

        private int GetRelevanceByName(string searchQuery, PhotoModel photoModel)
        {
            return Regex.Matches(photoModel.PhotoName, searchQuery).Count;
        }

        private int GetRelevanceByDescription(string searchQuery, PhotoModel photoModel)
        {
            return Regex.Matches(photoModel.Description, searchQuery).Count;
        }
    }
}