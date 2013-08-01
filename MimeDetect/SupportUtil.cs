// ***************************************************************
//  SupportUtil   version:  1.0   Date: 03/23/2006
//  -------------------------------------------------------------
//  
//  -------------------------------------------------------------
//  Copyright (C) 2006 - All Rights Reserved
// ***************************************************************
// 
// ***************************************************************

using System;
using System.Text;

namespace Winista.Mime
{
    public class SupportUtil
    {
        /// <summary>
        /// Receives a byte array and returns it transformed in an sbyte array
        /// </summary>
        /// <param name="byteArray">Byte array to process</param>
        /// <returns>The transformed array</returns>
        public static sbyte[] ToSByteArray(byte[] byteArray)
        {
            if (byteArray == null) return null;

            var sbyteArray = new sbyte[byteArray.Length];

            for (var index = 0; index < byteArray.Length; index++)
            {
                sbyteArray[index] = (sbyte)byteArray[index];
            }

            return sbyteArray;
        }

        /// <summary>
        /// Converts a string to an array of bytes
        /// </summary>
        /// <param name="sourceString">The string to be converted</param>
        /// <returns>The new array of bytes</returns>
        public static byte[] ToByteArray(string sourceString)
        {
            return Encoding.UTF8.GetBytes(sourceString);
        }

        /// <summary>
        /// Converts a array of object-type instances to a byte-type array.
        /// </summary>
        /// <param name="tempObjectArray">Array to convert.</param>
        /// <returns>An array of byte type elements.</returns>
        public static byte[] ToByteArray(object[] tempObjectArray)
        {
            if (tempObjectArray == null) return null;

            var byteArray = new byte[tempObjectArray.Length];

            for (var index = 0; index < tempObjectArray.Length; index++)
                byteArray[index] = (byte)tempObjectArray[index];

            return byteArray;
        }

        /// <summary>
        /// Obtains an array containing all the elements of the collection.
        /// </summary>
        /// <param name="objects">The array into which the elements of the collection will be stored.</param>
        /// <returns>The array containing all the elements of the collection.</returns>
        public static Object[] ToArray(System.Collections.ICollection c, Object[] objects)
        {
            var index = 0;

            var type = objects.GetType().GetElementType();
            var objs = (Object[])Array.CreateInstance(type, c.Count);

            var e = c.GetEnumerator();

            while (e.MoveNext())
                objs[index++] = e.Current;

            //If objects is smaller than c then do not return the new array in the parameter
            if (objects.Length >= c.Count)
                objs.CopyTo(objects, 0);

            return objs;
        }

        /// <summary>
        /// Converts a array of object-type instances to a byte-type array.
        /// </summary>
        /// <param name="tempObjectArray">Array to convert.</param>
        /// <returns>An array of byte type elements.</returns>
        public static byte[] ToByteArray(sbyte[] tempObjectArray)
        {
            if (tempObjectArray == null) return null;

            var byteArray = new byte[tempObjectArray.Length];

            for (var index = 0; index < tempObjectArray.Length; index++)
                byteArray[index] = (byte)tempObjectArray[index];

            return byteArray;
        }
    }
}
