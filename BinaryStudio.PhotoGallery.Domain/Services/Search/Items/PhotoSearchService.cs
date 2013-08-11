using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services.Search.Items
{
    internal class PhotoSearchService : DbService, IPhotoSearchService
    {
        public PhotoSearchService(IUnitOfWorkFactory workFactory) : base(workFactory)
        {
        }

        public IEnumerable<IFoundItem> Search(SearchArguments searchArguments)
        {
            var result = new List<PhotoFoundItem>();
            string searchQuery = searchArguments.SearchQuery;

            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                if (searchArguments.IsSearchPhotosByName)
                {
                    IEnumerable<PhotoFoundItem> found = SearchByCondition(searchQuery, unitOfWork,
                                                                          model => model.PhotoName.Contains(searchQuery),
                                                                          GetRelevanceByName);

                    result.AddRange(found);
                }

                if (searchArguments.IsSearchPhotosByDescription)
                {
                    IEnumerable<PhotoFoundItem> found = SearchByCondition(searchQuery, unitOfWork,
                                                                          model =>
                                                                          model.Description.Contains(searchQuery),
                                                                          GetRelevanceByDescription);

                    result.AddRange(found);
                }

                if (searchArguments.IsSearchPhotosByTags)
                {
                    IEnumerable<PhotoFoundItem> found = SearchByTags(searchQuery, unitOfWork);

                    result.AddRange(found);
                }
            }

            return Group(result);
        }

        private IEnumerable<IFoundItem> Group(IEnumerable<PhotoFoundItem> data)
        {
            return
                data.GroupBy(item => new {item.Id, item.UserModelId, item.AlbumId, item.PhotoName})
                      .Select(items => new PhotoFoundItem
                          {
                              Id = items.Key.Id,
                              UserModelId = items.Key.UserModelId,
                              AlbumId = items.Key.AlbumId,
                              PhotoName = items.Key.PhotoName,
                              Relevance = items.Sum(item => item.Relevance)
                          });
        }

        private IEnumerable<PhotoFoundItem> SearchByCondition(string searchQuery, IUnitOfWork unitOfWork,
                                                              Expression<Func<PhotoModel, bool>> predicate,
                                                              Func<string, PhotoModel, int> getRelevance)
        {
            return unitOfWork.Photos.Filter(predicate).ToList().Select(model => new PhotoFoundItem
                {
                    Id = model.Id,
                    UserModelId = model.UserModelId,
                    AlbumId = model.AlbumModelId,
                    PhotoName = model.PhotoName,
                    Relevance = getRelevance(searchQuery, model)
                });
        }

        private IEnumerable<PhotoFoundItem> SearchByTags(string searchQuery, IUnitOfWork unitOfWork)
        {
            var result = new List<PhotoFoundItem>();

            List<PhotoTagModel> foundTags =
                unitOfWork.PhotoTags.Filter(model => model.TagName.Contains(searchQuery)).ToList();

            foreach (PhotoTagModel tag in foundTags)
            {
                IEnumerable<PhotoFoundItem> tagPhotos = tag.PhotoModels.Select(model => new PhotoFoundItem
                    {
                        Id = model.Id,
                        UserModelId = model.UserModelId,
                        AlbumId = model.AlbumModelId,
                        PhotoName = model.PhotoName,
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