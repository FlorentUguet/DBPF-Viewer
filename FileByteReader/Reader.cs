using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileByteReader
{
    class Reader
    {
        public Reader()
        {

        }

        public static byte[] Read(string file, long offset, long last_offset, long length)
        {
            byte[] data = File.ReadAllBytes(file);
            byte[] sub_data = new byte[last_offset - offset + length];

            Array.Copy(data, last_offset - offset + length, sub_data, 0, last_offset - offset + length);

            return sub_data;
        }
    }
}
