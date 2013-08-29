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
        private readonly IAlbumService albumService;

        public PhotoSearchService(IUnitOfWorkFactory workFactory, IAlbumService albumService) : base(workFactory)
        {
            this.albumService = albumService;
        }

        public IEnumerable<IFound> Search(SearchArguments searchArguments)
        {
            var result = new List<PhotoFound>();

            IEnumerable<string> searchWords = searchArguments.SearchQuery.SplitSearchString();

            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                IEnumerable<AlbumModel> avialableAlbums = albumService.GetAvialableAlbums(searchArguments.UserId, unitOfWork);

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
                        model => searchWords.Any((model.Description ?? "").ToLower().Contains))
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
                    model => model.Tags.Any(tagModel => searchWords.Any((tagModel.TagName ?? "").ToLower().Contains)))
                    .Select(model => new PhotoFound
                    {
                        Id = model.Id,
                        OwnerId = model.OwnerId,
                        AlbumId = model.AlbumId,
                        Format = model.Format,
                        Rating = model.Rating,
                        DateOfCreation = model.DateOfCreation,
                        Relevance = CalculateRelevanceByTags(searchWords, model)
                    });

                result.AddRange(found);
            }

            return result;
        }

        private int CalculateRelevanceByTags(IEnumerable<string> searchWords, PhotoModel photoModel)
        {
            return
                photoModel.Tags.Sum(
                    photoTagModel =>
                        searchWords.Sum(searchWord => Regex.Matches((photoTagModel.TagName ?? ""), searchWord.ShieldString()).Count));
        }

        private int CalculateRelevanceByDescription(IEnumerable<string> searchWords, PhotoModel photoModel)
        {
            return searchWords.Sum(searchWord => Regex.Matches((photoModel.Description ?? "").ToLower(), searchWord.ShieldString()).Count);
        }

        private IEnumerable<IFound> Group(IEnumerable<PhotoFound> photos)
        {
            return
                photos.GroupBy(
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