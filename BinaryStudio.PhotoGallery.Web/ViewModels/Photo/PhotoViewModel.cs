using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Web.ViewModels.Photo
{
    public class PhotoViewModel : BaseViewModel
    {
        public string PhotoSource { get; set; }
        public string PhotoThumbSource { get; set; }
        public string PhotoViewPageUrl { get; set; }
        public int PhotoId { get; set; }
        public int AlbumId { get; set; }
	    public IEnumerable<PhotoTagModel> Tags { get; set; }
	    public string Description { get; set; }
	    public int OwnerId { get; set; }

        public static PhotoModel ToModel(int albumId, int userId, string realFileFormat)
        {
            var model = new PhotoModel
            {
                OwnerId = userId,
                AlbumModelId = albumId,
                Format = realFileFormat
            };

            return model;
        }
    }
}