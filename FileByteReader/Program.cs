using FileByteReader.models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileByteReader
{
    class Program
    {
        static void Main(string[] args)
        {
            DBPF file = new DBPF(@"D:\Program Files (x86)\Maxis\SimCity 4 Deluxe\SimPE\Sound.dat");
            Console.ReadLine();
        }

        static void Test1()
        {
            string SC4 = @"D:\Program Files (x86)\Maxis\SimCity 4 Deluxe\SimCity_";
            long o = 115553131;
            long l_o = 115594603;
            long l = 512;

            long c = l_o - o + l;

            Console.WriteLine("Reading file");
            byte[] arr = Reader.Read(SC4 + "2.dat", o, l_o, l);
            Console.WriteLine("Read " + arr.Length + " bytes (" + c + ")");
            Console.WriteLine("Validation : " + arr.Length % l);
            File.WriteAllBytes(@"D:\Prog\sc4_test.txt", arr);
            Console.WriteLine("Process completed");
            Console.ReadLine();
        }
    }
}
