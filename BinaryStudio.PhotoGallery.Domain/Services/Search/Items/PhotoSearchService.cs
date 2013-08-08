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
            var result = new List<IFoundItem>();

            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                if (searchArguments.IsSearchPhotosByName)
                {
                    IEnumerable<IFoundItem> found = SearchByName(searchArguments.SearchQuery, unitOfWork);

                    result.AddRange(found);
                }

                if (searchArguments.IsSearchPhotosByDescription)
                {
                    IEnumerable<IFoundItem> found = SearchByDescription(searchArguments.SearchQuery, unitOfWork);

                    result.AddRange(found);
                }
            }

            return result;
        }
        

        private IEnumerable<IFoundItem> SearchByName(string searchQuery, IUnitOfWork unitOfWork)
        {
            return unitOfWork.Photos.All().Select(model => new PhotoFoundItem
                {
                    PhotoName = model.PhotoName,
                    Relevance = GetRelevanceNumberByName(searchQuery, model)
                }).Where(item => item.Relevance != 0);
        }

        private IEnumerable<IFoundItem> SearchByDescription(string searchQuery, IUnitOfWork unitOfWork)
        {
            return unitOfWork.Photos.All().Select(model => new PhotoFoundItem
            {
                PhotoName = model.PhotoName,
                Relevance = GetRelevanceNumberByDescription(searchQuery, model)
            }).Where(item => item.Relevance != 0);
        }

        private int GetRelevanceNumberByName(string searchQuery, PhotoModel photoModel)
        {
            string[] splittedQuery = searchQuery.SplitBySpace();

            return splittedQuery.Sum(queryPart => Regex.Matches(photoModel.PhotoName, queryPart).Count);
        }

        private int GetRelevanceNumberByDescription(string searchQuery, PhotoModel photoModel)
        {
            string[] splittedQuery = searchQuery.SplitBySpace();

            return splittedQuery.Sum(queryPart => Regex.Matches(photoModel.Description, queryPart).Count);
        }
    }
}