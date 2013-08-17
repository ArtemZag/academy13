using System;
using System.Collections.Generic;
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

            string searchQuery = searchArguments.SearchQuery;

            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                IEnumerable<AlbumModel> avialableAlbums = secureService.GetAvailableAlbums(searchArguments.UserId,
                    unitOfWork);

                if (searchArguments.IsSearchPhotosByDescription)
                {
                    IEnumerable<PhotoFound> found = SearchByCondition(avialableAlbums, searchQuery, model =>
                        model.Description.ToLower().Contains(searchQuery.ToLower()) && !model.IsDeleted,
                        GetRelevanceByDescription);

                    result.AddRange(found);
                }

                if (searchArguments.IsSearchPhotosByTags)
                {
                    IEnumerable<PhotoFound> found = SearchByTags(avialableAlbums, searchQuery);

                    result.AddRange(found);
                }
            }

            return Group(result);
        }

        private IEnumerable<IFound> Group(IEnumerable<PhotoFound> data)
        {
            return
                data.GroupBy(
                    item =>
                        new
                        {
                            item.Id,
                            UserId = item.OwnerId,
                            item.AlbumId,
                            item.Rating,
                            item.DateOfCreation,
                            item.Format
                        })
                    .Select(items => new PhotoFound
                    {
                        Id = items.Key.Id,
                        OwnerId = items.Key.UserId,
                        AlbumId = items.Key.AlbumId,
                        Format = items.Key.Format,
                        Rating = items.Key.Rating,
                        DateOfCreation = items.Key.DateOfCreation,
                        Relevance = items.Sum(item => item.Relevance)
                    });
        }

        private IEnumerable<PhotoFound> SearchByCondition(IEnumerable<AlbumModel> fromAlbums, string searchQuery,
            Func<PhotoModel, bool> predicate,
            Func<string, PhotoModel, int> getRelevance)
        {
            var result = new List<PhotoFound>();

            foreach (AlbumModel album in fromAlbums)
            {
                IEnumerable<PhotoFound> found = album.Photos.Where(predicate).Select(model => new PhotoFound
                {
                    Id = model.Id,
                    OwnerId = model.OwnerId,
                    AlbumId = model.AlbumId,
                    Format = model.Format,
                    Rating = model.Rating,
                    DateOfCreation = model.DateOfCreation,
                    Relevance = getRelevance(searchQuery, model)
                });

                result.AddRange(found);
            }

            return result;
        }

        private IEnumerable<PhotoFound> SearchByTags(IEnumerable<AlbumModel> fromAlbums, string searchQuery)
        {
            var result = new List<PhotoFound>();

            foreach (AlbumModel album in fromAlbums)
            {
                IEnumerable<PhotoFound> found =
                    album.Photos.Where(
                        model => model.PhotoTags.Any(
                                tagModel => tagModel.TagName.ToLower().Contains(searchQuery.ToLower())))
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

        private int GetRelevanceByName(string searchQuery, PhotoModel photoModel)
        {
            return Regex.Matches(photoModel.Name.ToLower(), searchQuery.ToLower()).Count;
        }

        private int GetRelevanceByDescription(string searchQuery, PhotoModel photoModel)
        {
            return Regex.Matches(photoModel.Description.ToLower(), searchQuery.ToLower()).Count;
        }
    }
}