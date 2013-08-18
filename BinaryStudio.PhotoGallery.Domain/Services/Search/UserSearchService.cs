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
        private readonly IUsersMonitorTask usersMonitorTask;

        public UserSearchService(IUnitOfWorkFactory workFactory, IUsersMonitorTask usersMonitorTask) : base(workFactory)
        {
            this.usersMonitorTask = usersMonitorTask;
        }

        public IEnumerable<IFound> Search(SearchArguments searchArguments)
        {
            var result = new List<UserFound>();

            string searchQuery = searchArguments.SearchQuery;

            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                if (searchArguments.IsSearchUsersByName)
                {
                    IEnumerable<UserFound> found = SearchByCondition(searchQuery,
                        model =>
                            model.FirstName.Contains(searchQuery) ||
                            model.LastName.Contains(searchQuery), GetRelevanceByName,
                        unitOfWork);

                    result.AddRange(found);
                }

                if (searchArguments.IsSearchUserByDepartment)
                {
                    IEnumerable<UserFound> found = SearchByCondition(searchQuery,
                        model => model.Department.Contains(searchQuery),
                        GetRelevanceByDepartment,
                        unitOfWork);

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

        private IEnumerable<UserFound> SearchByCondition(string searchQuery, Expression<Func<UserModel, bool>> predicate,
            Func<string, UserModel, int> getRelevance, IUnitOfWork unitOfWork)
        {
            IEnumerable<UserFound> found = unitOfWork.Users.Filter(predicate).ToList().Select(model => new UserFound
            {
                Id = model.Id,
                Department = model.Department,
                IsOnline = usersMonitorTask.IsOnline(model.Email),
                Name = model.FirstName + " " + model.LastName,
                Relevance = getRelevance(searchQuery, model)
            });

            return found;
        }

        private int GetRelevanceByName(string searchQuery, UserModel userModel)
        {
            string name = userModel.FirstName + " " + userModel.LastName;

            return Regex.Matches(name.ToLower(), searchQuery.ToLower()).Count;
        }

        private int GetRelevanceByDepartment(string searchQuery, UserModel userModel)
        {
            return Regex.Matches(userModel.Department.ToLower(), searchQuery.ToLower()).Count;
        }
    }
}