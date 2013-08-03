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

        // путь к папке с collages
        private readonly string pathToCollages;

        //путь к текстурам collages;
        private readonly string texturesPath;

        //генератор случайных чисел для класса, ибо он часто используется
        private readonly Random rnd;

        public AsyncPhotoProcessor(string relativePathToAlbumIdFolder, string texturesPath, int maxHeight)
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

            this.texturesPath = texturesPath;
            if (texturesPath == null)
                throw new NullReferenceException("Textures path is null");

            if (!Directory.Exists(texturesPath))
                throw new DirectoryNotFoundException(string.Format("Directory {0} must exist", texturesPath));

            pathToThumbnail = string.Format(@"{0}\thumbnail\{1}", relativePath, maxHeight);
            pathToCollages = string.Format(@"{0}\collages", relativePath);
            rnd = new Random((int)DateTime.Now.Ticks);
        }

        // создаёт превью, заданного в конструкторе размера, для файлов превью которых отсутствуют
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
        //текстуры для collages
        public string[] GetTextures()
        {
            return Directory.GetFiles(texturesPath);
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
            CreateDirectoryIfNotExists(pathToThumbnail);
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

        // синхронизирует превью заданного размера и исходные файлы
        // удаляет ненужные превью (исходные файлы для них не существуют) и
        // создаёт для тех исходников картинок,превью для которых ещё не были созданы
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
        //застелить текстурой фон колажа(collage)
        private void TileTheImage(Graphics grfx, string imgPath, int width, int heigth)
        {
            int iter = 0;
            int sumWidth = 0;
            Image imgTexture = Image.FromFile(imgPath);
            while (true)
            {
                grfx.DrawImageUnscaled(imgTexture, sumWidth, iter);
                sumWidth += imgTexture.Width;
                if (sumWidth >= width)
                {
                    sumWidth = 0;
                    iter += imgTexture.Height;
                    if (iter >= heigth)
                        break;
                }
            }
        }
        //застелить колаж(collage) миниатюрами
        private void TileTheImage(Graphics grfx, IEnumerator<string> rndImages, int width, int heigth, int margin)
        {
            int iter = margin;
            int sumWidth = margin;
            foreach (var file in this)
            {
                Image thumbImage = Image.FromFile(file);
                grfx.DrawImageUnscaled(thumbImage, sumWidth, iter);
                sumWidth += thumbImage.Width + margin;
                if (sumWidth >= width)
                {
                    sumWidth = margin;
                    iter += maxHeight + margin;
                    if (iter >= heigth)
                        break;
                }
            }
        }
        public string MakeCollage(int width, int rows, int margin)
        {
            SetUpForRandomEnumerable(GetThumbnails());

            Image img = new Bitmap(width, rows * maxHeight + (rows + 1) * margin);
            Graphics grfx = Graphics.FromImage(img);

            string[] textutes = GetTextures();
            TileTheImage(grfx, textutes[rnd.Next(0, textutes.Length)], img.Width, img.Height);
            TileTheImage(grfx, GetEnumerator(), img.Width, img.Height, margin);

            string collageName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());

            string result = string.Format(@"{0}\{1}.jpg", pathToCollages, collageName);
            CreateDirectoryIfNotExists(pathToCollages);
            img.Save(result, ImageFormat.Jpeg);

            return result;
        }
        public string[] GetCollages()
        {
            string[] files = Directory.GetFiles(pathToCollages);
            if (files == null || files.Length == 0)
                throw new FileNotFoundException("Файлы не найдены!");

            return files;
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