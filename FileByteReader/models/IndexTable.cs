using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileByteReader.models
{
    class IndexTable
    {
        public class Index
        {
            public static UInt32 DIR_GROUP_ID = 3899334383;

            public UInt32 TypeID;
            public UInt32 GroupID;
            public UInt32 InstanceID;
            public UInt32 SecondInstanceID;
            public UInt32 Offset;
            public UInt32 Size;

            public Index(byte[] d, bool Ver71)
            {

                TypeID = Extensions.ExtractUInt32(d, 0);
                GroupID = Extensions.ExtractUInt32(d, 4);
                InstanceID = Extensions.ExtractUInt32(d, 8);

                if (!Ver71)
                {
                    Offset = Extensions.ExtractUInt32(d, 12);
                    Size = Extensions.ExtractUInt32(d, 16);
                }
                else
                {
                    SecondInstanceID = Extensions.ExtractUInt32(d, 12);
                    Offset = Extensions.ExtractUInt32(d, 16);
                    Size = Extensions.ExtractUInt32(d, 20);
                }
            }
        }

        public UInt32 Major;
        public UInt32 Minor;
        public UInt32 Count;
        public bool Ver71;

        public Index[] Indexes;
        public Index IndexDir;

        public IndexTable(UInt32 Major, UInt32 Minor, UInt32 Count, byte[] d)
        {
            this.Major = Major;
            this.Minor = Minor;
            this.Count = Count;

            Ver71 = (Major == 7 && Minor == 1);
            int l = Ver71 ? 24 : 20;

            Indexes = new Index[Count];

            Index DirIndex;

            for (int i=0;i< Count;i++)
            {
                Indexes[i] = new Index(d.SubArray(l * i, l), Ver71);

                if (Indexes[i].GroupID == Index.DIR_GROUP_ID)
                    IndexDir = Indexes[i];
            }
        }
    }
}
