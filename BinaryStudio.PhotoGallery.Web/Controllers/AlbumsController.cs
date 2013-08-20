using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Http;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Web.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [Authorize]
    [RoutePrefix("albums")]
    public class AlbumsController : Controller
    {
        private readonly IAlbumService _albumService;
        private readonly IUserService _userService;

        public AlbumsController(IAlbumService albumService, IUserService userService)
        {
            _albumService = albumService;
            _userService = userService;
        }

        [GET("")]
        public ActionResult Index()
        {
            /*string email = User.Identity.Name;
            IEnumerable<AlbumModel> albums = _albumService.GetAllAlbums(email);*/
            return View(new AlbumsViewModel {UserEmail = User.Identity.Name});
        }

        /*[HttpPost]
        public ActionResult GetAlbums(int start, int end)
        {
            var model = new AlbumsViewModel
            {
                UserEmail = User.Identity.Name,
                Models = new Collection<AlbumViewModel>()
            };

            string[] descriptions =
            {
                "Pictures", "Cars",
                "Muscle cars",
                "Power cars",
                "Elite cars",
                "Mega cars",
                "Import cars"
            };

            for (int i = 0; i < 7; i++)
            {
                model.Models.Add(
                    new AlbumViewModel
                    {
                        AlbumName = "Pictures",
                        AlbumTags = new Collection<AlbumTagModel>
                        {
                            new AlbumTagModel
                            {
                                ID = 0,
                                TagName = "fun images",
                                Albums = null
                            }
                        },
                        DateOfCreation = DateTime.Now,
                        Description = descriptions[i],
                        Id = i,
                        IsDeleted = false,
                        UserModelId = 0,
                    });
            }

            List<AlbumViewModel> models = model.Models.Select(p => p).Skip(start).Take(end - start + 1).ToList();
            foreach (AlbumViewModel mod in models)
            {
                var processor = new AsyncPhotoProcessor(UsersFolder, mod.UserModelId, mod.Id, 64);
                processor.SyncOriginalAndThumbnailImages();
                string s = processor.CreateCollageIfNotExist(256, 3);
                s = s.Remove(0, s.IndexOf("data") - 1).Replace(@"\", "/");
                mod.collageSource = s;
            }
            return Json(models);
        }*/

        /*[HttpGet]
        public ActionResult GetUserInfo()
        {
            UserModel user = _userService.GetUser(User.Identity.Name);
            return Json(new
            {
                nickName = "Superman",
                albumCount = 7,
                photoCount = 90,
                firstName = User.Identity.Name,
                lastName = User.Identity.Name,
                lastPhotoAdded =
                    "Date: " + DateTime.Now.ToShortDateString() + " time: " + DateTime.Now.ToLongTimeString(),
                isAdmin = user.IsAdmin ? "admin" : "simple user",
                department = ".Net development",
                userAvatar = "/data/photos/0/avatar.jpg"
            });
        }*/

        /*private static string GetUsersImagesFolder()
        {
            string webProjectPath = HttpRuntime.AppDomainAppPath;
            // TODO by Mikhail: this operation must be executed with PathUtil!!!
            return Path.Combine(webProjectPath, DataFolderName, PhotosFolderName);
        }*/
    }
}