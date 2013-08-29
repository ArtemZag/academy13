using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Services.Search.Results;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services.Search
{
    internal class CommentSearchService : DbService, ICommentSearchService
    {
        private readonly ISecureService secureService;

        public CommentSearchService(IUnitOfWorkFactory workFactory, ISecureService secureService) : base(workFactory)
        {
            this.secureService = secureService;
        }

        public IEnumerable<IFound> Search(SearchArguments searchArguments)
        {
            IEnumerable<string> searchWords = searchArguments.SearchQuery.SplitSearchString();

            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                IEnumerable<AlbumModel> avialableAlbums = secureService.GetAvailableAlbums(searchArguments.UserId,
                    unitOfWork);

                return Group(SearchByText(avialableAlbums, searchWords));
            }
        }

        private IEnumerable<CommentFound> SearchByText(IEnumerable<AlbumModel> fromAlbums,
            IEnumerable<string> searchWords)
        {
            var result = new List<CommentFound>();

            foreach (AlbumModel avialableAlbum in fromAlbums)
            {
                IEnumerable<CommentFound> found =
                    avialableAlbum.Photos.SelectMany(
                        model =>
                            model.PhotoComments.Where(
                                commentModel => searchWords.Any(searchWord => commentModel.Text.ToLower().Contains(searchWord)))
                                .Select(commentModel => new CommentFound
                                {
                                    DateOfCreation = commentModel.DateOfCreating,
                                    Id = commentModel.Id,
                                    OwnerId = commentModel.UserId,
                                    PhotoId = commentModel.PhotoId,
                                    Text = commentModel.Text,
                                    Relevance = CalculateRelevanceByText(searchWords, commentModel)
                                }));

                result.AddRange(found);
            }

            return result;
        }

        private int CalculateRelevanceByText(IEnumerable<string> searchWords, PhotoCommentModel photoCommentModel)
        {
            return searchWords.Sum(searchWord => Regex.Matches(photoCommentModel.Text.ToLower(), searchWord.ShieldString()).Count);
        }

        private IEnumerable<IFound> Group(IEnumerable<CommentFound> comments)
        {
            return comments.GroupBy(item => new {item.Id, item.OwnerId, item.Text, item.Type, item.DateOfCreation, item.PhotoId})
                .Select(items => new CommentFound
                {
                    DateOfCreation = items.Key.DateOfCreation,
                    Id = items.Key.Id,
                    OwnerId = items.Key.OwnerId,
                    PhotoId = items.Key.PhotoId,
                    Text = items.Key.Text,
                    Relevance = items.Sum(item => item.Relevance)
                });
        }
    }
}