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

            string text = "C";

            int chiffre = (char)Convert.ToChar(text) - 65;

            Console.Write(chiffre);

            Console.ReadKey();

        }
    }
}
