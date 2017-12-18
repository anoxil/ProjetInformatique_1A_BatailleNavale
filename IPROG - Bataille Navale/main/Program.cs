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

            string[,] tabAdversaire = new string[10, 10];
            string[,] tabJoueur = new string[10, 10];
            InitialiserJeu(tabJoueur, tabAdversaire);



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
            Console.Write("\n  A  B  C  D  E  F  G  H  I  J\n");
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

            InitialiserGrilleVide(tab);
            PlacerBateau(5, tab);
            PlacerBateau(4, tab);
            PlacerBateau(3, tab); PlacerBateau(3, tab);
            PlacerBateau(2, tab);

        }

        public static void PlacerBateau(int taille, string[,] tab)
        {
            Random r = new Random();
            int ligne = 0;
            int colonne = 0;
            int choixOrientation;
            bool cheminUtilisable = false;

            //on cherche un emplacement disponible pour le bateau
            do
            {
                //on trouve une case vide
                do
                {
                    ligne = r.Next(0, 10);
                    colonne = r.Next(0, 10);
                } while (tab[ligne, colonne] != "  ");

                //on cherche soit nord, soit sud, soit est, soit ouest pour un chemin vide de la taille du bateau
                choixOrientation = r.Next(0, 4);
                cheminUtilisable = CheminVide(ligne, colonne, choixOrientation, taille, tab);


            } while (!cheminUtilisable);

            //on remplit les cases du tableau pour marquer le bateau
            RemplirChemin(ligne, colonne, choixOrientation, taille, tab);

        }

        public static bool CheminVide(int ligne, int colonne, int choix, int taille, string[,] tab)
        {
            //nord
            if (choix == 0)
            {
                for (int k = 0; k < taille; k++)
                {
                    //if outofrangeexception, return false
                    try
                    {
                        if (tab[ligne - k, colonne] != "  ")
                            return false;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        return false;
                    }
                }
            }

            //sud
            else if (choix == 1)
            {
                for (int k = 0; k < taille; k++)
                {
                    try
                    {
                        if (tab[ligne + k, colonne] != "  ")
                            return false;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        return false;
                    }
                }
            }

            //ouest
            else if (choix == 2)
            {
                for (int k = 0; k < taille; k++)
                {
                    try
                    {
                        if (tab[ligne, colonne - k] != "  ")
                            return false;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        return false;
                    }
                }
            }

            //est
            else if (choix == 3)
            {
                for (int k = 0; k < taille; k++)
                {
                    try
                    {
                        if (tab[ligne, colonne + k] != "  ")
                            return false;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        return false;
                    }
                }
            }


            return true;
        }

        public static void RemplirChemin(int ligne, int colonne, int choix, int taille, string[,] tab)
        {
            //nord
            if (choix == 0)
            {
                for (int k = 0; k < taille; k++)
                {
                    tab[ligne - k, colonne] = String.Format("{0}{0}", taille);
                }
            }

            //sud
            else if (choix == 1)
            {
                for (int k = 0; k < taille; k++)
                {
                    tab[ligne + k, colonne] = String.Format("{0}{0}", taille);
                }
            }

            //ouest
            else if (choix == 2)
            {
                for (int k = 0; k < taille; k++)
                {
                    tab[ligne, colonne - k] = String.Format("{0}{0}", taille);
                }
            }

            //est
            else if (choix == 3)
            {
                for (int k = 0; k < taille; k++)
                {
                    tab[ligne, colonne + k] = String.Format("{0}{0}", taille);
                }
            }

        }

        public static void InitialiserJeu(string[,] tabJoueur, string[,] tabAdversaire)
        {
            //générer une grille pour l'adversaire
            InitialiserGrilleRemplie(tabAdversaire);

            //générer une grille pour le joueur tant qu'il n'est pas satisfait
            char happyCustomer = 'n';
            do
            {
                InitialiserGrilleRemplie(tabJoueur);
                AfficherGrille(tabJoueur);

                Console.Write("Souhaitez-vous garder ce placement (o/n) ? ");
                happyCustomer = Convert.ToChar(Console.ReadLine());

                while ((happyCustomer != 'o') && (happyCustomer != 'n'))
                {
                    Console.Write("Mauvais caractère, veuillez recommencer (o/n) : ");
                    happyCustomer = Convert.ToChar(Console.ReadLine());
                }
            } while (happyCustomer == 'n');

        }

    }
}