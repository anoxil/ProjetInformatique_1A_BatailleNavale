using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main
{
    class Program
    {
        static void Main(string[] args)
        {

            string[,] tabJoueur = new string[10, 10]; InitialiserGrilleVide(tabJoueur);

            AfficherGrille(tabJoueur);

            InitialiserGrilleRemplie(tabJoueur);


            Console.ReadKey();

        }

        public static void InitialiserGrilleVide(string[,] tab)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    tab[j, i] = "  ";
                }
            }
        }

        public static void AfficherGrille(string[,] tab)
        {
            Console.Write("  A  B  C  D  E  F  G  H  I  J\n");
            for (int i = 0; i < tab.GetLength(0); i++)
            {
                Console.WriteLine("+--+--+--+--+--+--+--+--+--+--+");
                for (int j = 0; j < tab.GetLength(1); j++)
                {
                    Console.Write("|" + tab[i, j]);
                    if (j==9)
                    {
                        Console.Write("|");
                    }
                }
                Console.WriteLine(" " + (i + 1));

            }
            Console.WriteLine("+--+--+--+--+--+--+--+--+--+--+\n\n");
        }

        public static void InitialiserGrilleRemplie(string[,] tab)
        {

            PlacerBateau(5, tab); //PlacerBateau(4, tab); PlacerBateau(3, tab); PlacerBateau(3, tab); PlacerBateau(2, tab);

        }

        public static void PlacerBateau(int taille, string[,] tab)
        {
            Random r = new Random();
            int ligne = 0;
            int colonne = 0;

            //on cherche un emplacement disponible pour le bateau
            do
            {
                //on trouve une case vide
                do
                {
                    ligne = r.Next(0, 10);
                    colonne = r.Next(0, 10);
                } while (tab[ligne, colonne] == "<>");

                //on cherche soit nord, soit sud, soit est, soit ouest pour un chemin vide de la taille du bateau
                int[] nsoe = new int[4];
                for (int i = 0; i < 4; i++)
                {

                }

            } while (true);


            //on remplie les cases du tableau pour marquer le bateau
            //...

        }

        public static bool CheminVide()
        {
            return true;
        }

    }
}