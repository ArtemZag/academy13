using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryStudio.PhotoGallery.Core.Helpers
{
    public class MaskHelper : IMaskHelper
    {
        public bool IsSet<T>(T flags, T flag) where T : struct
        {
            var flagsValue = (int)(object)flags;
            var flagValue = (int)(object)flag;

            return (flagsValue & flagValue) != 0;
        }

        public void Set<T>(ref T flags, T flag) where T : struct
        {
            var flagsValue = (int)(object)flags;
            var flagValue = (int)(object)flag;

            flags = (T)(object)(flagsValue | flagValue);
        }

        public void Unset<T>(ref T flags, T flag) where T : struct
        {
            var flagsValue = (int)(object)flags;
            var flagValue = (int)(object)flag;

            flags = (T)(object)(flagsValue & (~flagValue));
        }
    }
}
