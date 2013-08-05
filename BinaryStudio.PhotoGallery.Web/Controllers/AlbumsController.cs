using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels;
using BinaryStudio.PhotoGallery.Core.PhotoUtils;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
	[RoutePrefix("Albums")]
    public class AlbumsController : Controller
	{
	    private static string usersFolder;
        static AlbumsController()
        {
            usersFolder = GetUsersImagesFolder();
        }

	    [GET("")]
	    public ActionResult Index()
	    {
	        return View(new AlbumsViewModel() {UserEmail = User.Identity.Name});
	    }

	    [HttpPost]
        public ActionResult GetAlbums(int start, int end)
        {
            AlbumsViewModel model = new AlbumsViewModel();
            model.UserEmail = User.Identity.Name;
            model.Models = new Collection<AlbumViewModel>();
            string[] descriptions = new string[] { "Pictures", "Cars", "Muscle cars", "Power cars", "Elite cars", "Mega cars", "Import cars" };
            
            for (int i = 0; i < 7; i++)
            {
                model.Models.Add(
                    new AlbumViewModel()
                    {
                        AlbumName = "Pictures",
                        AlbumTags = new Collection<AlbumTagModel>()
		                        {
		                            new AlbumTagModel()
		                                {
		                                    ID = 0,
		                                    TagName = "fun images",
		                                    AlbumModels = null
		                                }
		                        },
                        DateOfCreation = DateTime.Now,
                        Description = descriptions[i],
                        Id = i,
                        IsDeleted = false,
                        UserModelId = 0,
                    });
            }

            var models = model.Models.Select(p => p).Skip(start).Take(end - start + 1).ToList();
            foreach (var mod in models)
            {
                AsyncPhotoProcessor processor = new AsyncPhotoProcessor(usersFolder, mod.UserModelId, mod.Id, 100);
                processor.SyncOriginalAndThumbnailImages();
                string s = processor.CreateCollageIfNotExist(400, 3);
                s = s.Remove(0, s.IndexOf("Content")-1).Replace(@"\","/");
                mod.collageSource = s;
            }
            return Json(models);
        }
        private static string GetUsersImagesFolder()
        {
            string webProjectPath = HttpRuntime.AppDomainAppPath;
            return Path.Combine(webProjectPath, @"Content\Users");
        }
    }
}
