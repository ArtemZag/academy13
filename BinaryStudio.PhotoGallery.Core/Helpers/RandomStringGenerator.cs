using System;
using System.Text;

namespace BinaryStudio.PhotoGallery.Core.Helpers
{
    internal class RandomStringGenerator
    {
        private readonly Random random = new Random();
        private const string CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";

        public string Generate(int size)
        {
            var stringBuilder = new StringBuilder(size);

            for (var i = 0; i < size; i++)
            {
                stringBuilder.Append(CHARS[random.Next(CHARS.Length)]);
            }
            return stringBuilder.ToString();
        }
    }
}
