using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Services.Search.Results;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services.Search
{
    internal class AlbumSearchService : DbService, IAlbumSearchService
    {
        private readonly IAlbumService albumService;

        public AlbumSearchService(IUnitOfWorkFactory workFactory, IAlbumService albumService) : base(workFactory)
        {
            this.albumService = albumService;
        }

        public IEnumerable<IFound> Search(SearchArguments searchArguments)
        {
            var result = new List<AlbumFound>();

            IEnumerable<string> searchWords = searchArguments.SearchQuery.SplitSearchString();

            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                IEnumerable<AlbumModel> avialableAlbums = albumService.GetAvialableAlbums(searchArguments.UserId, unitOfWork);

                if (searchArguments.IsSearchAlbumsByName)
                {
                    IEnumerable<AlbumFound> found = SearchByCondition(searchWords, avialableAlbums,
                        model => searchWords.Any(searchWord => model.Name.ToLower().Contains(searchWord)),
                        CalculateRelevanceByName);

                    result.AddRange(found);
                }

                if (searchArguments.IsSearchAlbumsByDescription)
                {
                    IEnumerable<AlbumFound> found = SearchByCondition(searchWords, avialableAlbums,
                        model => searchWords.Any(searchWord => model.Description.ToLower().Contains(searchWord)),
                        CalculateRelevaceByDescription);

                    result.AddRange(found);
                }
            }

            return Group(result);
        }

        private IEnumerable<AlbumFound> SearchByCondition(IEnumerable<string> searchWords,
            IEnumerable<AlbumModel> fromAlbums,
            Func<AlbumModel, bool> predicate,
            Func<IEnumerable<string>, AlbumModel, int> getRelevance)
        {
            return fromAlbums.Where(predicate).Select(model => new AlbumFound
            {
                Id = model.Id,
                DateOfCreation = model.DateOfCreation,
                Name = model.Name,
                OwnerId = model.OwnerId,
                Relevance = getRelevance(searchWords, model)
            });
        }

        private int CalculateRelevanceByName(IEnumerable<string> searchWords, AlbumModel albumModel)
        {
            return searchWords.Sum(searchWord => Regex.Matches(albumModel.Name.ToLower(), searchWord.ShieldString()).Count);
        }

        private int CalculateRelevaceByDescription(IEnumerable<string> searchWords, AlbumModel albumModel)
        {
            return searchWords.Sum(searchWord => Regex.Matches(albumModel.Description.ToLower(), searchWord.ShieldString()).Count);
        }

        private IEnumerable<IFound> Group(IEnumerable<AlbumFound> albums)
        {
            return
                albums.GroupBy(item => new {item.Id, item.DateOfCreation, item.Name, item.OwnerId, item.Type})
                    .Select(items => new AlbumFound
                    {
                        Id = items.Key.Id,
                        DateOfCreation = items.Key.DateOfCreation,
                        Name = items.Key.Name,
                        OwnerId = items.Key.OwnerId,
                        Relevance = items.Sum(found => found.Relevance)
                    });
        }
    }
}