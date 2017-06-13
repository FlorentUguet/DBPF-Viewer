using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileByteReader.models
{
    class Directory
    {
        public class ControlCharacter
        {
            public int CCLength;
            public UInt16 NumPlaintText;
            public UInt16 NumCopy;
            public UInt16 CopyOffset;
            public byte byte0;

            /*
            - Read the next control character. 
	        - Depending on the control character, read 0-3 more bytes that are a part of the control character.
	        - Inspect the control character.  From this, find out how many characters should be read and where from.
	        - Read 0-n characters from source and append them to the output.
                 (n being the "how many" data from above)
	        - Copy 0-n characters from somewhere in the output to the end of the output.
                 (n in this case is the "where from" from above)
            */

            public ControlCharacter(byte b)
            {
                BitArray arr = new BitArray(b);

                if (arr[0] == false)
                {
                    CCLength = 2;

                }
                else if (arr[1] == false)
                {
                    CCLength = 3;
                }
                else if (arr[2] == false)
                {
                    CCLength = 4;
                }
                else if (arr[0] == arr[1] == arr[2] == true)
                {
                    CCLength = 1;
                }
                else
                {
                    CCLength = 1;
                }
            }

        }

        public UInt32 CompressedSize;
        public UInt16 CompressionID;
        public UInt32 Size;

        public Directory(IndexTable.Index index, byte[] d)
        {
            byte[] h = d.SubArray((int)index.Offset, (int)index.Size);

            CompressedSize = Extensions.ExtractUInt32(h, 0);
            CompressionID = Extensions.ExtractUInt16(h, 4);

            byte[] arr = new byte[4];
            byte[] buf = h.SubArray(6, 3);

            for(int i=0;i<3;i++)
            {
                arr[i] = buf[i];
            }

            Size = BitConverter.ToUInt32(arr, 0);
        }
    }
}
