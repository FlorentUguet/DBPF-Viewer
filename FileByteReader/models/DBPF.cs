using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileByteReader.models
{
    class DBPF
    {
        //http://wiki.xentax.com/index.php/Maxis_DBPF
        //http://www.wiki.sc4devotion.com/index.php?title=DBPF

        public string Header;
        public UInt32 MajorVersion;
        public UInt32 MinorVersion;

        public UInt32 DateCreated;
        public UInt32 DateModified;
        public UInt32 IndexMajorVersion;
        public UInt32 IndexEntryCount;
        public UInt32 IndexOffset;
        public UInt32 IndexSize;
        public UInt32 HoleEntryCount;
        public UInt32 HoleOffset;
        public UInt32 HoleSize;
        public UInt32 IndexMinorVersion;

        public byte[] Reserved;

        public byte[] FileData;

        IndexTable Indexes;
        Directory DIR;

        public DBPF(string file)
        {

            byte[] d = File.ReadAllBytes(file);

            byte[] buf;


            buf = d.SubArray(0, 4);
            Header = Encoding.Default.GetString(buf);

            MajorVersion = Extensions.ExtractUInt32(d, 4);
            MinorVersion = Extensions.ExtractUInt32(d, 8);

            IndexEntryCount = Extensions.ExtractUInt32(d, 36);
            IndexSize = Extensions.ExtractUInt32(d, 44);

            if (MajorVersion < 2)
            {
                DateCreated = Extensions.ExtractUInt32(d, 24);
                DateModified = Extensions.ExtractUInt32(d, 28);
                IndexMajorVersion = Extensions.ExtractUInt32(d, 32);
                IndexOffset = Extensions.ExtractUInt32(d, 40);
                HoleEntryCount = Extensions.ExtractUInt32(d, 48);
                HoleOffset = Extensions.ExtractUInt32(d, 52);
                HoleSize = Extensions.ExtractUInt32(d, 56);
                IndexMinorVersion = Extensions.ExtractUInt32(d, 60);
            }
            else
            {
                IndexOffset = Extensions.ExtractUInt32(d, 64);
            }

            Indexes = new IndexTable(IndexMajorVersion, IndexMinorVersion, IndexEntryCount, d.SubArray((int)IndexOffset, (int)IndexSize));
            DIR = new Directory(Indexes.IndexDir, d);

            //Console.WriteLine("Test Insta is " + Indexes.Indexes[953].InstanceID.ToString("X"));
            //Console.WriteLine("Test Group is " + Indexes.Indexes[953].GroupID.ToString("X"));
            Console.WriteLine("File loaded");

        }
    }
}
