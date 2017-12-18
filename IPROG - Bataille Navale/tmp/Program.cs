using System;
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

            int i = 2;

            string text = String.Format("{0}{0}", i);

            Console.Write(text);

            Console.ReadKey();

        }
    }
}
