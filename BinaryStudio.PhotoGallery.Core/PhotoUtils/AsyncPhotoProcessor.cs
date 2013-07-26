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
        private string relativePath;
        private int maxHeight;
        private string pathToThumbnail;
        //relativepath - путь к папке с альбомом
        //maxHeight - максимальная высота фотографии
        public AsyncPhotoProcessor(string relativePathToAlbumIdFolder, int maxHeight)
        {
            this.maxHeight = maxHeight;
            if (maxHeight < 1)
                throw new ArgumentException("Invalid height. Parameter must be greater then zero");

            if (maxHeight > 1024)
                throw new ArgumentException("Invalid height. Parameter must be less or equal then 1024");

            relativePath = relativePathToAlbumIdFolder;

            if (relativePath == null)
                throw new NullReferenceException("Relative path must be not null reference");

            if (!Directory.Exists(relativePath))
                throw new DirectoryNotFoundException(string.Format("Directory {0} must exist", relativePath));

            pathToThumbnail = string.Format(@"{0}\thumbnail\{1}", relativePath, maxHeight);
        }
        //Создаёт миниатюры, заданного в конструкторе размера, для файлов миниатюры которых ещё не были созданы или же удалены
        public string CreateThumbnailsIfNotExist()
        {
            try
            {
                CreateDirectoryIfNotExists(pathToThumbnail);
            }
            catch (Exception exception)
            {
                return exception.Message;
            }

            string[] files = Directory.GetFiles(relativePath);
            if (files == null || files.Length < 1)
                return string.Format("Target folder {0} not contains any file", relativePath);

            Parallel.ForEach(files, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
                             (path) =>
                             {
                                 //получаем имя изображения и пути к папке с исходной фотографией и prewiew
                                 string srcName = Path.GetFileNameWithoutExtension(path);
                                 //получаем путь к thumbnail изображения
                                 string pathToThumbnailOfImage = Path.Combine(pathToThumbnail, string.Format("{0}.jpg", srcName));
                                 //если на диске есть thumbnail для текущего изображения то читаем его,
                                 try
                                 {
                                     //если на диске нет thumbnail для текущего изображения то создадим его и сохраним на диске
                                     if (!File.Exists(pathToThumbnailOfImage))
                                     {
                                         Image image = Image.FromFile(path);
                                         Size size = CalculateThumbSize(image.Size, maxHeight);
                                         image = image.GetThumbnailImage(size.Width, size.Height,
                                                                         () => { return false; }, IntPtr.Zero);
                                         image.Save(pathToThumbnailOfImage, ImageFormat.Jpeg);
                                     }
                                 }
                                 catch
                                 { }
                             });
            return "Ok";
        }
        //Удаляет все миниатюры для данного размера миниатюры
        public string DeleteThumbnails()
        {
            try
            {
                Directory.Delete(Path.GetDirectoryName(pathToThumbnail), true);
                return "Ok";
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }
        //Удаляет миниатюру заданного размера для исходного файла
        public string DeleteThumbnailForSpecifiedOriginalFile(string name)
        {
            string path = string.Format(@"{0}\{1}.jpg", pathToThumbnail, name);
            if (File.Exists(path))
                File.Delete(path);
            else
                return string.Format("Specified thumbnail for file {0} does not exists", name);

            return "Ok";
        }
        //Удаляет миниатюру заданного размера для отсутствующих исходных файлов
        public string DeleteThumbnailsIfOriginalNotExist()
        {
            if (!Directory.Exists(pathToThumbnail))
                return "Directory doesnt not exist";

            string[] files = Directory.GetFiles(pathToThumbnail);
            if (files == null || files.Length < 1)
                return string.Format("Target folder {0} not contains any file", pathToThumbnail);

            Parallel.ForEach(files, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
                             (path) =>
                             {
                                 string srcName = Path.GetFileNameWithoutExtension(path);
                                 string pathToSrcOfImage = Path.Combine(relativePath, string.Format("{0}.jpg", srcName));
                                 try
                                 {
                                     if (!File.Exists(pathToSrcOfImage))
                                         File.Delete(path);
                                 }
                                 catch
                                 { }
                             });
            return "Ok";
        }
        //Синхронизирует миниатюры заданного размера и исходные файлы
        //Удаляет ненужные миниатюры(исходные файлы для них не существуют) и
        //создаёт для тех для которых ещё не были созданы
        public string SyncOriginalAndThumbnailImages()
        {
            string s1 = DeleteThumbnailsIfOriginalNotExist();
            string s2 = CreateThumbnailsIfNotExist();
            return string.Format("{0}\n{1}", s1, s2);
        }
        private void CreateDirectoryIfNotExists(string path)
        {
            if (!Directory.Exists(path))
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
            if (files == null || files.Length < 1)
                throw new FileNotFoundException("Файлы не найдены!");

            return files;
        }

        public string MakePrewiew(int width, int rows)
        {
            string result = "";

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
            //имя для prewiew;
            result = string.Format(@"{0}\@_@_@{1}.jpg", pathToThumbnail, Path.GetRandomFileName());
            img.Save(result, ImageFormat.Jpeg);
            return result;
        }
        public string[] GetPrewiews()
        {
            return GetThumbnails("@_@_@????????????.jpg");
        }

        private string[] GetRandomPaths(string[] files, int howMany)
        {
            //Если количество нужных нам, случайных картинок больше либо равно количеству имеющихся, то
            //возвращаем все что есть
            int length = files.Length;
            if (howMany >= length)
                return files;

            List<int> indexes =
                Enumerable.Range(0, length).ToList();

            //иначе формируем список и удаляем из него пока не останется то количество картинок которое нам нужно
            var rnd = new Random((int)DateTime.Now.Ticks);
            int index;
            for (int iter = 0; iter < length - howMany; iter++)
            {
                index = rnd.Next(0, length - iter);
                indexes.RemoveAt(index);
            }

            string[] result = new string[howMany];
            for (int iter = 0; iter < howMany; iter++)
                result[iter] = files[indexes[iter]];

            return result;
        }
        private static Size CalculateThumbSize(Size size, int maxSize)
        {
            //Делает высоту постоянной, а ширина изменяется в соответствии с пропорцией
            return new Size((int)((size.Width / (double)size.Height) * maxSize), maxSize);
        }
        public IEnumerator<string> GetEnumerator()
        {
            int length = array.Length;

            List<int> indexes =
                Enumerable.Range(0, length).ToList();

            var rnd = new Random((int)DateTime.Now.Ticks);
            int index;
            for (int iter = 0; iter < length; iter++)
            {
                index = rnd.Next(0, length - iter);
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