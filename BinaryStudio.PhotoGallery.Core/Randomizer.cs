using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinaryStudio.PhotoGallery.Core
{
    // this class can implement other functions for generate something
    // that's why it was renamed to Randomizer
    public static class Randomizer
    {
        private static readonly Random Random = new Random((int) DateTime.Now.Ticks);
        private const string CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";

        public static string GetString(int size)
        {
            var stringBuilder = new StringBuilder(size);

            for (var i = 0; i < size; i++)
            {
                stringBuilder.Append(CHARS[Random.Next(CHARS.Length)]);
            }
            return stringBuilder.ToString();
        }

        public static IEnumerable<string> GetEnumerator(IEnumerable<string> enumerable)
        {
            var arr = enumerable.ToArray();
            var length = arr.Length;

            var indexes =
                Enumerable.Range(0, length).ToList();

            for (int iter = 0; iter < length; iter++)
            {
                int index = Random.Next(0, length - iter);
                yield return arr[indexes[index]];
                indexes.RemoveAt(index);
            }
        }

        public static int GetNumber(int maxNumber)
        {
            return Random.Next(maxNumber);
        }
    }
}
