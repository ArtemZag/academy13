using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BinaryStudio.PhotoGallery.Core.StringUtils;
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

            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                if (searchArguments.IsSearchPhotosByName)
                {
                    IEnumerable<PhotoFoundItem> found = SearchByName(searchArguments.SearchQuery, unitOfWork);

                    result.AddRange(found);
                }

                if (searchArguments.IsSearchPhotosByDescription)
                {
                    IEnumerable<PhotoFoundItem> found = SearchByDescription(searchArguments.SearchQuery, unitOfWork);

                    result.AddRange(found);
                }

                if (searchArguments.IsSearchPhotosByTags)
                {
                    IEnumerable<PhotoFoundItem> found = SearchByTags(searchArguments.SearchQuery, unitOfWork);

                    result.AddRange(found);
                }
            }

            return result.GroupBy(item => new { item.Id, item.UserModelId, item.AlbumId, item.PhotoName }).Select(items => new PhotoFoundItem
            {
                Id = items.Key.Id,
                UserModelId = items.Key.UserModelId,
                AlbumId = items.Key.AlbumId,
                PhotoName = items.Key.PhotoName,
                Relevance = items.Sum(item => item.Relevance)
            });
        }

        /// <summary>
        ///     Search photos by name
        /// </summary>
        private IEnumerable<PhotoFoundItem> SearchByName(string searchQuery, IUnitOfWork unitOfWork)
        {
            return unitOfWork.Photos.All().Select(model => new PhotoFoundItem
                {
                    Id = model.Id,
                    UserModelId = model.UserModelId,
                    AlbumId = model.AlbumModelId,
                    PhotoName = model.PhotoName,
                    Relevance = GetRelevanceByName(searchQuery, model) // todo: throws "is not supported in linq to entities"
                }).Where(item => item.Relevance != 0);
        }

        /// <summary>
        ///     Search photos by description
        /// </summary>
        private IEnumerable<PhotoFoundItem> SearchByDescription(string searchQuery, IUnitOfWork unitOfWork)
        {
            return unitOfWork.Photos.All().Select(model => new PhotoFoundItem
                {
                    Id = model.Id,
                    UserModelId = model.UserModelId,
                    AlbumId = model.AlbumModelId,
                    PhotoName = model.PhotoName,
                    Relevance = GetRelevanceByDescription(searchQuery, model) // todo: throws "is not supported in linq to entities"
                }).Where(item => item.Relevance != 0);
        }

        /// <summary>
        ///     Search photos by tag
        /// </summary>
        private IEnumerable<PhotoFoundItem> SearchByTags(string searchQuery, IUnitOfWork unitOfWork)
        {
            string[] splittedQuery = searchQuery.SplitBySpace();

            var foundTags = new List<PhotoTagModel>();
            var result = new List<PhotoFoundItem>();

            foreach (string queryPart in splittedQuery)
            {
                string part = queryPart;

                IQueryable<PhotoTagModel> found =
                    unitOfWork.PhotoTags.All().Select(model => model).Where(model => model.TagName.Contains(part));

                foundTags.AddRange(found);
            }

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
            string[] splittedQuery = searchQuery.SplitBySpace();

            return splittedQuery.Sum(queryPart => Regex.Matches(photoModel.PhotoName, queryPart).Count);
        }

        private int GetRelevanceByDescription(string searchQuery, PhotoModel photoModel)
        {
            string[] splittedQuery = searchQuery.SplitBySpace();

            return splittedQuery.Sum(queryPart => Regex.Matches(photoModel.Description, queryPart).Count);
        }
    }
}