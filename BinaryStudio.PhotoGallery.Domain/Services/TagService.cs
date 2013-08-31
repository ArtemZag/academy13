using System.Collections.Generic;
using System.Linq;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    internal class TagService : DbService, ITagService
    {
        public TagService(IUnitOfWorkFactory workFactory) : base(workFactory)
        {
        }

		public IEnumerable<string> GetTagsByPhoto(int photoId)
		{
			using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
			{
				return unitOfWork.Photos.Find(g => g.Id == photoId).Tags.Select(g=>g.TagName).ToList();
			}
		}

		public void AddPhotoTag(int photoId, string tagName)
		{
			using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
			{

				unitOfWork.Photos.Find(g => g.Id == photoId).Tags.Add(new PhotoTagModel(tagName));

				unitOfWork.SaveChanges();

			}
		}

		public void RemoveAllTagsByPhoto(int photoId)
		{
			using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
			{

				unitOfWork.Photos.Find(g => g.Id == photoId).Tags.Clear();

				unitOfWork.SaveChanges();

			}
		}
    }
}