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

        /// <summary>
        /// Remove tag from photo
        /// </summary>
        /// <param name="tagName">tag name</param>
        /// <param name="photoID">photo ID</param>
        /// <returns></returns>
        bool RemoveTag(string tagName, int photoID);

        /// <summary>
        /// Gets all photo with tag
        /// </summary>
        /// <param name="tagName">tag name</param>
        /// <returns></returns>
        IQueryable<PhotoModel> FindPhotosByTag(string tagName);

        /// <summary>
        /// Gets all tags for photo
        /// </summary>
        /// <param name="photoID">photo ID</param>
        /// <returns></returns>
        IQueryable<string> GetTagsFromPhoto(int photoID);
    }
}
