using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Core.PhotoUtils
{
    public interface ICollageProcessor
    {
        void CreateCollage(int userId, int albumId, IEnumerable<PhotoModel> models);
    }
}