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
    }
}