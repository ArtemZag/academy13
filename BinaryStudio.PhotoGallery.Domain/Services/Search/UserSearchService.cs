using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Services.Search.Results;
using BinaryStudio.PhotoGallery.Domain.Services.Tasks;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services.Search
{
    internal class UserSearchService : DbService, IUserSearchService
    {
        public UserSearchService(IUnitOfWorkFactory workFactory) : base(workFactory)
        {
        }

        public IEnumerable<IFound> Search(SearchArguments searchArguments)
        {
            var result = new List<UserFound>();

            IEnumerable<string> searchWords = searchArguments.SearchQuery.SplitSearchString();

            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                if (searchArguments.IsSearchUsersByName)
                {
                    IEnumerable<UserFound> found = SearchByCondition(searchWords,
                        model => searchWords.Any(searchWord =>
                                    model.FirstName.ToLower().Contains(searchWord) || model.LastName.ToLower().Contains(searchWord)),
                        GetRelevanceByName, unitOfWork);

                    result.AddRange(found);
                }

                if (searchArguments.IsSearchUserByDepartment)
                {
                    IEnumerable<UserFound> found = SearchByCondition(searchWords,
                        model => searchWords.Any(searchWord => model.Department.ToLower().Contains(searchWord)),
                        GetRelevanceByDepartment, unitOfWork);

                    result.AddRange(found);
                }
            }

            return Group(result);
        }

        private IEnumerable<IFound> Group(IEnumerable<UserFound> data)
        {
            return
                data.GroupBy(item => new {item.Department, item.Id, item.IsOnline, item.Name, item.Type})
                    .Select(items => new UserFound
                    {
                        Department = items.Key.Department,
                        IsOnline = items.Key.IsOnline,
                        Name = items.Key.Name,
                        Id = items.Key.Id,
                        Relevance = items.Sum(found => found.Relevance)
                    });
        }

        private IEnumerable<UserFound> SearchByCondition(IEnumerable<string> searchWords,
            Expression<Func<UserModel, bool>> predicate,
            Func<IEnumerable<string>, UserModel, int> getRelevance, IUnitOfWork unitOfWork)
        {
            IEnumerable<UserFound> found = unitOfWork.Users.Filter(predicate).ToList().Select(model => new UserFound
            {
                Id = model.Id,
                Department = model.Department,
                Name = model.FirstName + " " + model.LastName,
                Relevance = getRelevance(searchWords, model)
            });

            return found;
        }

        private int GetRelevanceByName(IEnumerable<string> searchWords, UserModel userModel)
        {
            return
                searchWords.Sum(
                    searchWord =>
                        Regex.Matches(userModel.FirstName.ToLower(), searchWord.ShieldString()).Count +
                        Regex.Matches(userModel.LastName.ToLower(), searchWord.ShieldString()).Count);
        }

        private int GetRelevanceByDepartment(IEnumerable<string> searchWords, UserModel userModel)
        {
            return searchWords.Sum(
                searchWord => Regex.Matches(userModel.Department.ToLower(), searchWord.ShieldString()).Count);
        }
    }
}