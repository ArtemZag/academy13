using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BinaryStudio.PhotoGallery.Core.PhotoUtils
{
    public class AsyncPhotoProcessor : IAsyncPhotoProcessor
    {
        private const int MIN_HEIGHT = 1;
        private const int MAX_HEIGHT = 1024;
        //
        private IEnumerable<string> array;
        //
        private string pathToUsers;
        // путь к папке с альбомом
        private readonly string relativePath;
        // максимальная высота фотографии
        private readonly int maxHeight;
        // путь к папке с превью
        private readonly string pathToThumbnail;
        // путь к папке с collages
        private readonly string pathToCollages;
        //
        private readonly int userId;
        //
        private readonly int albumId ;
        //генератор случайных чисел для класса, ибо он часто используется
        private readonly Random rnd;
        public AsyncPhotoProcessor(string pathToUsersFolder,int userId ,int albumId, int maxHeight)
        {
            this.maxHeight = maxHeight;

            if (maxHeight < MIN_HEIGHT)
                throw new ArgumentException(string.Format("Invalid height. Parameter must be greater then {0}", MIN_HEIGHT));

            if (maxHeight > MAX_HEIGHT)
                throw new ArgumentException(string.Format("Invalid height. Parameter must be less or equal then {0}", MAX_HEIGHT));

            this.albumId = albumId;
            this.userId = userId;
            if (albumId < 0 || userId < 0)
                throw new ArgumentException("Invalid userId and albumId");

            pathToUsers = pathToUsersFolder;

            if (pathToUsers == null)
                throw new NullReferenceException("Path to users folder is null");

            relativePath = Path.Combine(pathToUsers, userId.ToString(), albumId.ToString());

            if (relativePath == null)
                throw new NullReferenceException("Relative path is null");

            if (!Directory.Exists(relativePath))
                Directory.CreateDirectory(relativePath);

            pathToThumbnail = string.Format(@"{0}\thumbnail\{1}", relativePath, maxHeight);
            pathToCollages = string.Format(@"{0}\collages", relativePath);
            rnd = new Random((int)DateTime.Now.Ticks);
        }

        private void CreateDirectoryIfNotExists(string path)
        {
            Directory.CreateDirectory(path);
        }

        // создаёт превью, заданного в конструкторе размера, для файлов превью которых отсутствуют
        public bool CreateThumbnailsIfNotExist()
        {
            CreateDirectoryIfNotExists(pathToThumbnail);
            bool created = false;
            object syncRoot = new object();
            IEnumerable<string> files = Directory.GetFiles(relativePath);

            Parallel.ForEach(files, new ParallelOptions {MaxDegreeOfParallelism = Environment.ProcessorCount},
                             path =>
                                 {
                                     // получаем имя изображения
                                     string imageName = Path.GetFileNameWithoutExtension(path);
                                     // получаем путь к thumbnail изображения
                                     string pathToThumbnailOfImage = Path.Combine(pathToThumbnail,
                                                                                  string.Format("{0}.jpg", imageName));
                                     // если на диске нет thumbnail для текущего изображения то создадим его и сохраним на диске
                                     if (!File.Exists(pathToThumbnailOfImage))
                                     {
                                         Image image = Image.FromFile(path);
                                         Size size = CalculateThumbSize(image.Size);
                                         image = image.GetThumbnailImage(size.Width, size.Height,
                                                                         () => false, IntPtr.Zero);
                                         image.Save(pathToThumbnailOfImage, ImageFormat.Jpeg);
                                         lock (syncRoot)
                                             created = true;
                                     }
                                 });
            return created;
        }

        // удаляет миниатюру заданного размера для отсутствующих исходных файлов
        public bool DeleteThumbnailsIfOriginalNotExist()
        {
            CreateDirectoryIfNotExists(pathToThumbnail);
            bool deleted = false;
            object syncRoot = new object();
            IEnumerable<string> files = Directory.GetFiles(pathToThumbnail);

            Parallel.ForEach(files, new ParallelOptions {MaxDegreeOfParallelism = Environment.ProcessorCount},
                             path =>
                                 {
                                     string srcName = Path.GetFileNameWithoutExtension(path);
                                     string pathToSrcOfImage = Path.Combine(relativePath,
                                                                            string.Format("{0}.jpg", srcName));
                                     if (!File.Exists(pathToSrcOfImage))
                                     {
                                         File.Delete(path);
                                         lock (syncRoot)
                                             deleted = true;
                                     }
                                 });
            return deleted;
        }

        private void DeleteCollages()
        {
            if (Directory.Exists(pathToCollages))
                Directory.Delete(pathToCollages, true);
        }

        public void SyncOriginalAndThumbnailImages()
        {
            bool anyDeleted = DeleteThumbnailsIfOriginalNotExist();
            bool anyCreated = CreateThumbnailsIfNotExist();

            if (anyDeleted || anyCreated)
                DeleteCollages();
        }

        public string CreateCollageIfNotExist(int width,int rows)
        {
            if (Directory.Exists(pathToCollages))
            {
                string[] file = Directory.GetFiles(pathToCollages);
                return file[0];
            }
            return MakeCollage(width, rows);
        }
       
        public static IEnumerable<string> GetThumbnails(string pathToUsers,int userId, int albumId, int maxHeight)
        {
            string fullPath = Path.Combine(pathToUsers, userId.ToString(), albumId.ToString(),"thumbnail" ,maxHeight.ToString());
            if (Directory.Exists(fullPath))
            {
                string[] files = Directory.GetFiles(fullPath);
                if (files == null || files.Length == 0)
                    throw new FileNotFoundException("Файлы не найдены!");

                return files;
            }
            return null;
        }

        public IEnumerable<string> GetThumbnails()
        {
            return GetThumbnails(pathToUsers, userId, albumId, maxHeight);
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

        public void SetUpForRandomEnumerable(IEnumerable<string> arr)
        {
            array = arr;
        }

        private void SetUpGraphics(Graphics grfx)
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
            SetUpGraphics(grfx);

            SetUpForRandomEnumerable(GetThumbnails());
            TileTheImage(grfx, GetEnumerator(), width, height);

            string collageName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            string result = string.Format(@"{0}\{1}.jpg", pathToCollages, collageName);

            CreateDirectoryIfNotExists(pathToCollages);
            img.Save(result, ImageFormat.Jpeg);

            return result;
        }

        private Size CalculateThumbSize(Size size)
        {
            // делает высоту постоянной, а ширина изменяется в соответствии с пропорцией
            return new Size((int) ((size.Width/(double) size.Height)*maxHeight), maxHeight);
        }
        public IEnumerable<string> GetEnumerator()
        {
            string[] arr = array.ToArray();
            int length = arr.Length;

            List<int> indexes =
                Enumerable.Range(0, length).ToList();

            for (int iter = 0; iter < length; iter++)
            {
                int index = rnd.Next(0, length - iter);
                yield return arr[indexes[index]];
                indexes.RemoveAt(index);
            }
        }
    }
}