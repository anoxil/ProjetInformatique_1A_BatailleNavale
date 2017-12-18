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

            int[] tab = new int[4];

            for (int i = 0; i < 4; i++)
            {
                if (tab[i] == 1) Console.WriteLine(tab[i]);
            }

            Console.ReadKey();

        }
    }
}
