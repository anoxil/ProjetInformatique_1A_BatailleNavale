using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tmp
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            StreamWriter sw = new StreamWriter("coucou.txt");

            sw.WriteLine("hello world.");*/

            StreamReader sr = new StreamReader("coucou.txt");

            Console.WriteLine(sr.ReadLine());

            sr.Close();

            Console.ReadKey();

        }
    }
}
