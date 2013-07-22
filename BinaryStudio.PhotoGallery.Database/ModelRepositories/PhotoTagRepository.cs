using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryStudio.PhotoGallery.Database.ModelInterfaces;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Database.ModelRepositories
{
    class PhotoTagRepository : BaseRepository<PhotoTagModel>, IPhotoTagRepository
    {
        public PhotoTagRepository(DatabaseContext dataBaseContext) : base(dataBaseContext)
        {
        }

        public void Add(string tagName)
        {
            var tag = new PhotoTagModel(){ TagName = tagName };
            base.Add(tag);
        }

        public bool RemoveTag(string tagName, int photoID)
        {
            throw new NotImplementedException();
        }

        public IQueryable<PhotoModel> FindPhotosByTag(string tagName)
        {
            throw new NotImplementedException();
        }

        public IQueryable<string> GetTagsFromPhoto(int photoID)
        {
            throw new NotImplementedException();
        }
    }
}
