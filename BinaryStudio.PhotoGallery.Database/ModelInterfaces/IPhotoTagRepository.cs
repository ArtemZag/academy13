using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Database.ModelInterfaces
{
    public interface IPhotoTagRepository : IBaseRepository<PhotoTagModel>
    {
        /// <summary>
        /// Add tag to photo
        /// </summary>
        /// <param name="tagName">tag name</param>
        /// <returns></returns>
        void Add(string tagName);
    }
}
