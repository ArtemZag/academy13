using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    public interface ITagService
    {
		/// <summary>
		///     Gets tags by photo's ID.
		/// </summary>
	    IEnumerable<string> GetTagsByPhoto(int photoId);

		/// <summary>
		///     Adds new tags to photo.
		/// </summary>
	    void AddPhotoTag(int photoId, string tagName);

	    /// <summary>
	    ///    Remove all tags associated with photo by photo's ID.
	    /// </summary>
	    void RemoveAllTagsByPhoto(int photoId);
    }
}
