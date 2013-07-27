using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BinaryStudio.PhotoGallery.Core.PhotoUtils
{
    internal class AsyncPhotoProcessor : IEnumerable<string>, IAsyncPhotoProcessor
    {
        private const int MIN_HEIGHT = 1;

        private const int MAX_HEIGHT = 1024;

        // путь к папке с альбомом
        private readonly string relativePath;

        // максимальная высота фотографии
        private readonly int maxHeight;

        // путь к папке с превью
        private readonly string pathToThumbnail;

        public AsyncPhotoProcessor(string relativePathToAlbumIdFolder, int maxHeight)
        {
            this.maxHeight = maxHeight;

            if (maxHeight < MIN_HEIGHT)
                throw new ArgumentException(string.Format("Invalid height. Parameter must be greater then {0}", MIN_HEIGHT));

            if (maxHeight > MAX_HEIGHT)
                throw new ArgumentException(string.Format("Invalid height. Parameter must be less or equal then {0}", MAX_HEIGHT));

            relativePath = relativePathToAlbumIdFolder;

            if (relativePath == null)
                throw new NullReferenceException("Relative path is null");

            if (!Directory.Exists(relativePath))
                throw new DirectoryNotFoundException(string.Format("Directory {0} must exist", relativePath));

            pathToThumbnail = string.Format(@"{0}\thumbnail\{1}", relativePath, maxHeight);
        }

        // создаёт миниатюры, заданного в конструкторе размера, для файлов миниатюры которых отсутствуют
        public void CreateThumbnailsIfNotExist()
        {
            CreateDirectoryIfNotExists(pathToThumbnail);

            string[] files = Directory.GetFiles(relativePath);

            Parallel.ForEach(files, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
                             path =>
                             {
                                 // получаем имя изображения
                                 string imageName = Path.GetFileNameWithoutExtension(path);

                                 // получаем путь к thumbnail изображения
                                 string pathToThumbnailOfImage = Path.Combine(pathToThumbnail, string.Format("{0}.jpg", imageName));
                                 
                                 // если на диске есть thumbnail для текущего изображения то читаем его
                                 
                                 // если на диске нет thumbnail для текущего изображения то создадим его и сохраним на диске
                                 if (!File.Exists(pathToThumbnailOfImage))
                                 {
                                     Image image = Image.FromFile(path);
                                     Size size = CalculateThumbSize(image.Size, maxHeight);

                                     image = image.GetThumbnailImage(size.Width, size.Height,
                                         () => false, IntPtr.Zero);
                                     image.Save(pathToThumbnailOfImage, ImageFormat.Jpeg);
                                 }
                             });
        }

        // удаляет все миниатюры для данного размера миниатюры
        public void DeleteThumbnails()
        {
            Directory.Delete(Path.GetDirectoryName(pathToThumbnail), true);
        }

        // удаляет миниатюру заданного размера для исходного файла
        public void DeleteThumbnailForSpecifiedOriginalFile(string name)
        {
            string path = string.Format(@"{0}\{1}.jpg", pathToThumbnail, name);

            File.Delete(path);
        }

        // удаляет миниатюру заданного размера для отсутствующих исходных файлов
        public void DeleteThumbnailsIfOriginalNotExist()
        {
            string[] files = Directory.GetFiles(pathToThumbnail);

            Parallel.ForEach(files, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
                             path =>
                             {
                                 string srcName = Path.GetFileNameWithoutExtension(path);

                                 string pathToSrcOfImage = Path.Combine(relativePath, string.Format("{0}.jpg", srcName));

                                 if (!File.Exists(pathToSrcOfImage))
                                 {
                                     File.Delete(path);
                                 }

                             });
        }

        // синхронизирует миниатюры заданного размера и исходные файлы
        // удаляет ненужные миниатюры (исходные файлы для них не существуют) и
        // создаёт для тех для которых ещё не были созданы
        public void SyncOriginalAndThumbnailImages()
        {
            DeleteThumbnailsIfOriginalNotExist();

            CreateThumbnailsIfNotExist();
        }

        private void CreateDirectoryIfNotExists(string path)
        {
            Directory.CreateDirectory(path);
        }

        public string[] GetRandomThumbnail(int howMany)
        {
            string[] files = GetThumbnails();

            return GetRandomPaths(files, howMany);
        }

        public string[] GetThumbnails(string searchPattern = "*.*")
        {
            string[] files = Directory.GetFiles(pathToThumbnail, searchPattern);
            if (files == null || files.Length == 0)
                throw new FileNotFoundException("Файлы не найдены!");

            return files;
        }

        public string MakePrewiew(int width, int rows)
        {
            SetUpForRandomEnumerable(GetThumbnails());
            int iter = 0;
            int sumWidth = 0;
            Image img = new Bitmap(width, rows * maxHeight);
            Graphics grfx = Graphics.FromImage(img);
            foreach (var file in this)
            {
                Image thumbImage = Image.FromFile(file);
                grfx.DrawImageUnscaled(thumbImage, sumWidth, iter);
                sumWidth += thumbImage.Width;
                if (sumWidth >= width)
                {
                    sumWidth = 0;
                    iter += maxHeight;
                    if (iter >= rows * maxHeight)
                        break;
                }
            }
            //имя для prewiew; ???? 
            string result = string.Format(@"{0}\@_@_@{1}.jpg", pathToThumbnail, Path.GetRandomFileName());
            img.Save(result, ImageFormat.Jpeg);
            return result;
        }
        public string[] GetPrewiews()
        {
            // ????? 
            return GetThumbnails("@_@_@????????????.jpg");
        }

        private string[] GetRandomPaths(string[] files, int howMany)
        {
            // Если количество нужных нам, случайных картинок больше либо равно количеству имеющихся, то
            // возвращаем все что есть
            int length = files.Length;
            if (howMany >= length)
                return files;

            List<int> indexes =
                Enumerable.Range(0, length).ToList();

            //иначе формируем список и удаляем из него пока не останется то количество картинок которое нам нужно
            var rnd = new Random((int)DateTime.Now.Ticks);
            for (int iter = 0; iter < length - howMany; iter++)
            {
                int index = rnd.Next(0, length - iter);
                indexes.RemoveAt(index);
            }

            var result = new string[howMany];
            for (int iter = 0; iter < howMany; iter++)
                result[iter] = files[indexes[iter]];

            return result;
        }
        private static Size CalculateThumbSize(Size size, int maxSize)
        {
            // делает высоту постоянной, а ширина изменяется в соответствии с пропорцией
            return new Size((int)((size.Width / (double)size.Height) * maxSize), maxSize);
        }
        public IEnumerator<string> GetEnumerator()
        {
            int length = array.Length;

            List<int> indexes =
                Enumerable.Range(0, length).ToList();

            var rnd = new Random((int)DateTime.Now.Ticks);
            for (int iter = 0; iter < length; iter++)
            {
                int index = rnd.Next(0, length - iter);
                yield return array[indexes[index]];
                indexes.RemoveAt(index);
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private string[] array;
        public void SetUpForRandomEnumerable(string[] arr)
        {
            array = arr;
        }
    }
}