using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace Test
{
    internal static class AsyncPhotoProcessor
    {
        //функция возвращает массив сжатых фотографий
        //принимает массив путей к исходным фотографиям и максимальный размер фотографии с одной стороны
        //функция просматривает папку prewiew, которая должна находится в папке с фотографиями.
        //Если prewiew для фотографии существует,то берёт фотку с диска, иначе читает полное изображение с диска и делает prewiew
        //Всё работает в параллельном режиме.
        public static Image[] ProcessImages(string[] filesToProcess, int maxSize)
        {
            var images = new ConcurrentBag<Image>();
            var parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = Environment.ProcessorCount;
            Parallel.ForEach(filesToProcess, parallelOptions,
                             (path) =>
                                 {
                                     //получаем имя изображения и пути к папке с исходной фотографией и prewiew
                                     string srcName = Path.GetFileNameWithoutExtension(path);
                                     string pathToSrcFiles = Path.GetDirectoryName(path);
                                     string prewiewsFolder = Path.Combine(pathToSrcFiles, @"prewiews\");
                                     //получаем путь к prewiew изображения
                                     string pathToPrewiewOfImage = Path.Combine(prewiewsFolder, srcName + ".jpg");
                                     Image image = null;
                                     //если на диске есть prewiew для текущего изображения то читаем его,
                                     try
                                     {
                                         if (File.Exists(pathToPrewiewOfImage))
                                         {
                                             image = Image.FromFile(pathToPrewiewOfImage);
                                         }
                                             //если на диске нет prewiew для текущего изображения то создадим его и сохраним на диске
                                         else
                                         {
                                             image = Image.FromFile(path);
                                             Size size = CalculateThumbSize(image.Size, maxSize);
                                             image = image.GetThumbnailImage(size.Width, size.Height,
                                                                             () => { return false; }, IntPtr.Zero);
                                             if (!Directory.Exists(prewiewsFolder))
                                                 Directory.CreateDirectory(prewiewsFolder);
                                             image.Save(pathToPrewiewOfImage, ImageFormat.Jpeg);
                                         }
                                     }
                                     catch
                                     {
                                         image = new Bitmap(maxSize, maxSize);
                                     }
                                     finally
                                     {
                                         //сохраняем сжатую фотограцию в коллекцию
                                         images.Add(image);
                                     }
                                 });
            //возвращаем массив уменьшенных фотографий
            return images.ToArray();
        }

        /*private static string[] GetRandomPathsToImages(string[] files, int howMany)
        {
            int length = files.Length;
            if (howMany >= length)
                return files;
            else
            {
                var rnd = new Random((int) DateTime.Now.Ticks);
                var rndImages = new List<string>(files);
                int index;
                for (int i = 0; i < length - howMany; i++)
                {
                    index = rnd.Next(0, length - i);
                    rndImages.RemoveAt(index);
                }
                return rndImages.ToArray();
            }
        }*/
        //public static string Make 
        private static Size CalculateThumbSize(Size size, int maxSize)
        {
            if (size.Width > size.Height)
            {
                return new Size(maxSize, (int) ((size.Height/(double) size.Width)*maxSize));
            }
            else
            {
                return new Size((int) ((size.Width/(double) size.Height)*maxSize), maxSize);
            }
        }
    }
}