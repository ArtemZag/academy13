using System;
using System.Collections.Generic;
using System.Linq;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    internal class SearchService : DbService, ISearchService
    {
        public SearchService(IUnitOfWorkFactory workFactory) : base(workFactory)
        {
        }

        public IEnumerable<UserModel> SearchUsers(string searchQuery, int begin, int end)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                return
                    unitOfWork.Users.Filter(
                        model => model.FirstName.Contains(searchQuery) || model.LastName.Contains(searchQuery))
                              .OrderByDescending(model => model.Id)
                              .Skip(end)
                              .Take(end - begin)
                              .ToList();
            }
        }

        public IEnumerable<AlbumModel> SearchAlbums(string searchQuery, int begin, int end)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PhotoModel> SearchPhotos(string searchQuery, int begin, int end)
        {
            throw new NotImplementedException();
        }
    }
}