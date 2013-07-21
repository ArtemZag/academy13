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
    }
}
