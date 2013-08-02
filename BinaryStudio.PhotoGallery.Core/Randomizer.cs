using System;
using System.Text;

namespace BinaryStudio.PhotoGallery.Core
{
    // this class can implement other functions for generate something
    // that's why it was renamed to Randomizer
    public static class Randomizer
    {
        private static readonly Random _random = new Random();
        private const string CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";

        public static string GetString(int size)
        {
            var stringBuilder = new StringBuilder(size);

            for (var i = 0; i < size; i++)
            {
                stringBuilder.Append(CHARS[_random.Next(CHARS.Length)]);
            }
            return stringBuilder.ToString();
        }

        public static int GetNumber(int maxNumber)
        {
            return _random.Next(maxNumber);
        }
    }
}
