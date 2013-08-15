using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using System.Configuration;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Core.PhotoUtils;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    class PhotoSynchronizingService: SynchronizingProvider, IPhotoSynchronizingService
    {
        private static readonly int MinHeight;
        private static readonly int MaxHeight;
        private static readonly Random rnd;
        private static readonly string physicalPath;
        private static readonly string virtualPathToDataFolder;
        private static readonly string photosFolderName;
        private static readonly string thumbnailsFolderName;
        private static readonly string collagesFolderName;
        private static readonly string avatarName;
        private static readonly string thumbnailExtension;
        private static readonly ParallelOptions options;

        private object syncRoot;
        private string pathToUsersFolder;
        private string pathToUserFolder;
        private string pathToAlbumFolder;
        private string pathToThumbnail;
        private string pathToCollages;
        private int maxHeight;
        private int userId;
        private int albumId;
        static PhotoSynchronizingService()
        {
            MinHeight = 1;
            MaxHeight = 1024;
            rnd = new Random((int) DateTime.Now.Ticks);
            options = new ParallelOptions();
            options.MaxDegreeOfParallelism = Environment.ProcessorCount;

            physicalPath = HttpRuntime.AppDomainAppPath;
            virtualPathToDataFolder = ConfigurationManager.AppSettings["DataDirectory"];
            photosFolderName = ConfigurationManager.AppSettings["UsersDirectory"];
            thumbnailsFolderName = ConfigurationManager.AppSettings["AlbumThumbnailsFolderName"];
            collagesFolderName = ConfigurationManager.AppSettings["AlbumCollagesFolder"];
            avatarName = ConfigurationManager.AppSettings["UserAvatarFileName"];
            thumbnailExtension = ConfigurationManager.AppSettings["ThumbnailExtension"];
        }

        public PhotoSynchronizingService(IUnitOfWorkFactory workFactory) : base(workFactory)
        {
            
        }
        public void Initialize(int albumId, int userId, int maxHeight)
        {
            this.maxHeight = maxHeight;
            if (maxHeight < MinHeight)
                throw new ArgumentException(string.Format("Invalid height. Parameter must be greater then {0}", MinHeight));

            if (maxHeight > MaxHeight)
                throw new ArgumentException(string.Format("Invalid height. Parameter must be less or equal then {0}", MaxHeight));

            this.albumId = albumId;
            this.userId = userId;

            if (albumId < 0 || userId < 0)
                throw new ArgumentException("Invalid userId and albumId");

            pathToUsersFolder = Path.Combine(physicalPath, virtualPathToDataFolder, photosFolderName);
            pathToUserFolder = Path.Combine(pathToUsersFolder, userId.ToString());
            pathToAlbumFolder = Path.Combine(pathToUserFolder, albumId.ToString());
            pathToThumbnail = Path.Combine(pathToAlbumFolder, thumbnailsFolderName, maxHeight.ToString());
            pathToCollages = Path.Combine(pathToAlbumFolder, collagesFolderName);

            syncRoot = new object();
        }

        public void SyncOriginalAndThumbnailImages()
        {
            //List<PhotoModel> photos = GetPhotosForSyncronizeByModelAndUserId(userId, albumId);
            List<string> photos = new List<string>(Directory.GetFiles(pathToAlbumFolder));

            bool anyDeleted = DeleteThumbnailsIfOriginalNotExist(photos);
            bool anyCreated = CreateThumbnailsIfNotExist(photos);

            //Если сихронизация сделала изменения то в коллаже могут быть неактуальные фотки, удаляем
            if (anyDeleted || anyCreated)
                DeleteCollages();
        }

        // удаляет миниатюру заданного размера для отсутствующих исходных файлов
        public bool DeleteThumbnailsIfOriginalNotExist(List<string> models)
        {
            bool deleted = false;
            if (!Directory.Exists(pathToThumbnail))
            {
                Parallel.ForEach(models, options,
                                 model =>
                                     {
                                         string originalPhotoPath = Path.Combine(pathToAlbumFolder, Path.GetFileName(model));
                                         string nameOfOriginal = Path.GetFileNameWithoutExtension(model);
                                         string pathToAppropriateThumbnail = Path.Combine(pathToThumbnail,
                                                                                          string.Format("{0}.jpg",
                                                                                                        nameOfOriginal));

                                         if (DeleteFileIfAnotherDoesNotExist(pathToAppropriateThumbnail,
                                                                             originalPhotoPath))
                                         {
                                             lock (syncRoot)
                                                 deleted = true;
                                         }
                                     });
            }
            else
                deleted = true;

            return deleted;
        }

        // создаёт превью, заданного в конструкторе размера, для файлов превью которых отсутствуют
        public bool CreateThumbnailsIfNotExist(List<string> models)
        {
            CreateDirectoriesIfNotExist(pathToThumbnail);
            bool created = false;

            Parallel.ForEach(models, options,
                             model =>
                             {
                                 string pathToOriginal = Path.Combine(pathToAlbumFolder, Path.GetFileName(model));
                                 string originalName = Path.GetFileNameWithoutExtension(model);
                                 string pathToThumbnailOfOriginal = Path.Combine(pathToThumbnail,
                                                                                 string.Format("{0}.jpg",
                                                                                               originalName));
                                 // если на диске нет thumbnail для текущего изображения то создадим его и сохраним на диске
                                 if (CreateThumbnailIfNotExist(pathToThumbnailOfOriginal, pathToOriginal))
                                 {
                                     lock (syncRoot)
                                         created = true;
                                 }
                             });
            return created;
        }

        private void DeleteCollages()
        {
            if (Directory.Exists(pathToCollages))
                Directory.Delete(pathToCollages, true);
        }

        private bool DeleteFileIfAnotherDoesNotExist(string aFileWhichCanBeRemoved, string fileToVerifyTheExistenceOf)
        {
            if (!File.Exists(fileToVerifyTheExistenceOf))
            {
                File.Delete(aFileWhichCanBeRemoved);
                return true;
            }
            return false;
        }

        private void CreateDirectoriesIfNotExist(params string[] paths)
        {
            foreach (var path in paths)
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
        }

        private bool CreateThumbnailIfNotExist(string thumbnailPath, string originalPhotoPath)
        {
            if (!File.Exists(thumbnailPath))
            {
                reduceAndConvertTheImageToJpg(thumbnailPath, originalPhotoPath);
                return true;
            }
            return false;
        }

        private void reduceAndConvertTheImageToJpg(string dest, string source)
        {
            Image image = Image.FromFile(source);
            Size size = CalculateThumbSize(image.Size);
            image = image.GetThumbnailImage(size.Width, size.Height,
                                            () => false, IntPtr.Zero);
            image.Save(dest, ImageFormat.Jpeg);
        }

        public string GetUserAvatar()
        {
            return GetReference(ImageFormatHelper.GetImages(pathToUserFolder).FirstOrDefault());
        }

        public string CreateCollageIfNotExist(int width,int rows)
        {
            if (Directory.Exists(pathToCollages))
            {
                return GetReference(ImageFormatHelper.GetImages(pathToCollages).First());
            }
            return GetReference(MakeCollage(width, rows*maxHeight));
        }

        private string GetReference(string path)
        {
            return path.Remove(0, path.IndexOf(virtualPathToDataFolder) - 1).Replace(@"\", "/");
        }
        public IEnumerable<string> GetThumbnails()
        {
            if (Directory.Exists(pathToThumbnail))
            {
                IEnumerable<string> files = ImageFormatHelper.GetImages(pathToThumbnail);
                if (files == null || !files.Any())
                    throw new FileNotFoundException("Файлы не найдены!");

                return files;
            }
            return null;
        }

        //застелить collage миниатюрами
        private void TileTheImage(Graphics grfx, IEnumerable<string> enumerable, int width, int heigth)
        {
            int iter = 0;
            int sumWidth = 0;
            foreach (var file in enumerable)
            {
                Image thumbImage = Image.FromFile(file);
                grfx.DrawImageUnscaled(thumbImage, sumWidth, iter);
                sumWidth += thumbImage.Width;
                if (sumWidth >= width)
                {
                    sumWidth = 0;
                    iter += maxHeight;
                    if (iter >= heigth)
                        break;
                }
            }
        }

        private void SetUpGraphicsToHightQuality(Graphics grfx)
        {
            grfx.CompositingQuality = CompositingQuality.HighQuality;
            grfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
            grfx.SmoothingMode = SmoothingMode.HighQuality;
        }

        public string MakeCollage(int width, int rows)
        {
            int height = rows*maxHeight;
            Image img = new Bitmap(width, height);
            Graphics grfx = Graphics.FromImage(img);
            SetUpGraphicsToHightQuality(grfx);

            TileTheImage(grfx, GetEnumerator(GetThumbnails()), width, height);
            CreateDirectoriesIfNotExist(pathToCollages);

            string pathToSaveCollage = GetRandomCollageFileName();
            img.Save(pathToSaveCollage, ImageFormat.Jpeg);

            return pathToSaveCollage;
        }
        public string GetRandomCollageFileName()
        {
            string collageName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            return string.Format(@"{0}\{1}.jpg", pathToCollages, collageName);
        }
        private Size CalculateThumbSize(Size size)
        {
            // делает высоту постоянной, а ширина изменяется в соответствии с пропорцией
            return new Size((int) ((size.Width/(double) size.Height)*maxHeight), maxHeight);
        }
        public IEnumerable<string> GetEnumerator(IEnumerable<string> thumbnails)
        {
            var enumerable = thumbnails as IList<string> ?? thumbnails.ToList();
            var thumbs = enumerable.ToList();
            int length = thumbs.Count;

            var indexes =
                Enumerable.Range(0, length).ToList();

            for (int iter = 0; iter < length; iter++)
            {
                int index = rnd.Next(0, length - iter);
                yield return enumerable[indexes[index]];
                indexes.RemoveAt(index);
            }
        }
    }
}
