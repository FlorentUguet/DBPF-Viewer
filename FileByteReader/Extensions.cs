using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileByteReader
{
    static class Extensions
    {
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        public static UInt32 ExtractUInt32(byte[] data, int index)
        {
            return BitConverter.ToUInt32(data.SubArray(index, 4), 0);
        }

        public static UInt16 ExtractUInt16(byte[] data, int index)
        {
            return BitConverter.ToUInt16(data.SubArray(index, 2), 0);
        }
    }
}
