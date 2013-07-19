using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryStudio.PhotoGallery.Database.Model_Interfaces;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Database.Model_Repositories
{
    class AvailableGroupRepository : BaseRepository<AvailableGroupModel>, IAvailableGroupRepository
    {
        public AvailableGroupRepository(DatabaseContext dataBaseContext) : base(dataBaseContext)
        {
        }
    }
}
