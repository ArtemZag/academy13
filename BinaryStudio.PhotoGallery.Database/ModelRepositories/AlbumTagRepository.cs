using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryStudio.PhotoGallery.Database.ModelInterfaces;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Database.ModelRepositories
{
    class AlbumTagRepository : BaseRepository<AlbumTagModel>, IAlbumTagRepository
    {
        public AlbumTagRepository(DatabaseContext dataBaseContext) : base(dataBaseContext)
        {
        }
    }
}
