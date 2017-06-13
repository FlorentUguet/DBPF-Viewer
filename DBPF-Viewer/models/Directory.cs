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
            //http://www.wiki.sc4devotion.com/index.php?title=DBPF_Compression

            public int CCLength;
            public int NumPlaintText;
            public int NumCopy;
            public int CopyOffset;
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

            public ControlCharacter(byte[] b)
            {
                BitArray arr = new BitArray(b[0]);

                if (arr[0] == false)
                {
                    CCLength = 2;
                    byte[] C = b.SubArray(0,2);
                    arr = new BitArray(C);
                    
                    NumPlaintText = C[0] & 0x03;
                    NumCopy = ( (C[0] & 0x1C) >> 2) + 3;
                    CopyOffset = ( (C[0] & 0x60) << 3) + C[1] + 1;

                }
                else if (arr[1] == false)
                {
                    CCLength = 3;
                    byte[] C = b.SubArray(0,3);

                    NumPlaintText = ((C[1] & 0xC0) >> 6 ) & 0x03;
                    NumCopy = (C[0] & 0x3F) + 4;
                    CopyOffset = ( (C[1] & 0x3F) << 8 ) + C[2] + 1;

                }
                else if (arr[2] == false)
                {
                    CCLength = 4;
                    byte[] C = b.SubArray(0,4);

                    //Sims2 - SimCity 4 Deluxe
                    
                    NumPlaintText = C[0] & 0x03;
                    NumCopy = ((C[0] & 0x0C) << 6 )  + C[3] + 5;
                    CopyOffset =((C[0] & 0x10) << 12 ) + (C[1] << 8 ) + C[2] + 1;
                    
                    //SimCity 4
                    /*
                    NumPlaintText = C[0] & 0x03;
                    NumCopy = ( (C[0] & 0x1C) << 6 )  + C[3] + 5;
                    CopyOffset = (C[1] << 8) + C[2];
                    */
                }
                else if (arr[0] == arr[1] == arr[2] == true)
                {
                    CCLength = 1;
                    byte[] C = b.SubArray(0,1);

                    NumPlaintText =((C[0]& 0x1F) << 2 ) + 4;
                    NumCopy = 0 ;
                    CopyOffset = 0; 
                }
                else
                {
                    CCLength = 1;
                    byte[] C = b.SubArray(0,1);

                    NumPlaintText = (C[0] & 0x03);
                    NumCopy =    0 ;
                    CopyOffset =   0;
                }
            }

            public void Process(byte[] input, List<byte> output)
            {
                //Plain Text Copy
                for(int i=0;i<NumPlaintText;i++)
                {
                    output.Add(input[i]);
                }

                //Character Copy
                int count = output.Count-1;
                for(int i=0;i<NumCopy;i++)
                {
                    output.Add(output[count-CopyOffset+i]);
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

            byte[] CompressedData = d.SubArray((int)(index.Offset+index.Size),(int)CompressedSize);
            List<byte> UncompressedData = new List<byte>();

            int bId = 0;

            while(bId<CompressedSize)
            {
                ControlCharacter C = new ControlCharacter(CompressedData.SubArray(bId,4));
                bId += C.CCLength;
                C.Process(CompressedData.SubArray(bId,C.NumPlaintText),UncompressedData);
                bId += C.NumPlaintText;
            }

            Console.WriteLine("Uncompressed data: " + Size);
            Console.WriteLine("Expected Size    : " + UncompressedData.Count);
        }
    }
}
