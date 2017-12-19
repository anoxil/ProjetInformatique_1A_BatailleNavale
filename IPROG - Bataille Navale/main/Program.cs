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

            Console.WriteLine("\n################");
            Console.WriteLine("# Début du jeu #");
            Console.WriteLine("################");

            bool victoire = false;

            while (!victoire)
            {
                /*ENSEIGNANT : décommenter la ligne suivante pour avoir accès à la grille
                 *  de l'adversaire avec les bateaux affichés */
                AfficherGrille(tabAdversaire); 
                //AfficherGrilleAdversaire(tabAdversaire);
                victoire = TourJoueur(tabAdversaire);
            }

            Console.WriteLine("\nVICTOIRE !");

            Console.ReadKey();

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

        public static void AfficherGrille(string[,] tab)
        {
            Console.Write("\n  A  B  C  D  E  F  G  H  I  J\n");
            for (int i = 0; i < tab.GetLength(0); i++)
            {
                Console.WriteLine("+--+--+--+--+--+--+--+--+--+--+");
                for (int j = 0; j < tab.GetLength(1); j++)
                {
                    Console.Write("|" + tab[i, j]);
                    if (j == 9)
                    {
                        Console.Write("|");
                    }
                }
                Console.WriteLine(" " + (i + 1));

            }
            Console.WriteLine("+--+--+--+--+--+--+--+--+--+--+\n\n");
        }

        public static void AfficherGrilleAdversaire(string[,] tab)
        {
            Console.Write("  A  B  C  D  E  F  G  H  I  J\n");
            for (int i = 0; i < tab.GetLength(0); i++)
            {
                Console.WriteLine("+--+--+--+--+--+--+--+--+--+--+");
                for (int j = 0; j < tab.GetLength(1); j++)
                {
                    if ((tab[i, j] == "  ") || (tab[i, j] == "><")) //affiche morceaux des bateaux touchés, mais pas les morceaux non touchés
                    {
                        Console.Write("|" + tab[i, j]);
                    }
                    else //donc si "<>" qui est un morceau de bâteau non touché
                    {
                        Console.Write("|  ");//on affiche des espaces à la place
                    }

                    if (j == 9)
                    {
                        Console.Write("|");
                    }
                }
                Console.WriteLine(" " + (i + 1));

            }
            Console.WriteLine("+--+--+--+--+--+--+--+--+--+--+\n\n");
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

                //on cherche soit nord, soit sud, soit est, soit ouest pour un emplacement vide de la taille du bateau
                choixOrientation = r.Next(0, 4);
                cheminUtilisable = EmplacementEstVide(ligne, colonne, choixOrientation, taille, tab);


            } while (!cheminUtilisable);

            //on remplit les cases du tableau pour marquer le bateau
            RemplirEmplacement(ligne, colonne, choixOrientation, taille, tab);

        }

        public static bool EmplacementEstVide(int ligne, int colonne, int choix, int taille, string[,] tab)
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

        public static void RemplirEmplacement(int ligne, int colonne, int choix, int taille, string[,] tab)
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

        


        public static bool TourJoueur(string[,] tabOpposant)
        {
            int nbBateauxAdversairesRestants = 0; //pour calculer le nb de salves restantes
            int nbSalves = 5 - nbBateauxAdversairesRestants; //nb de salves qu'un joueur a au début de son tour
            string ligneVisee = ""; string colonneVisee = "";
            int coupsRestants;
            int[] caseVisee = new int[2]; //colonne et ligne choisies

            //BateauxAdversairesRestants();

            for (int coups = 0; coups < nbSalves; coups++)
            {
                coupsRestants = nbSalves - coups;
                caseVisee = CaseVisee(ligneVisee, colonneVisee, coupsRestants);


                //ATTAQUE VERS ADVERSAIRE
                //caractère de croix à mettre quand on touche un morceau de bâteau.
                if ((tabOpposant[caseVisee[0], caseVisee[1]] == "22")
                    || (tabOpposant[caseVisee[0], caseVisee[1]] == "33")
                    || (tabOpposant[caseVisee[0], caseVisee[1]] == "44")
                    || (tabOpposant[caseVisee[0], caseVisee[1]] == "55"))
                {
                    tabOpposant[caseVisee[0], caseVisee[1]] = "><"; 
                }
            }

            return Gagner(tabOpposant);

        }

        public static int[] CaseVisee (string colonneVisee, string ligneVisee, int coupsRestants)
        {
            //Demande la colonne puis la ligne visées par le joueur en vérifiant que les entrées sont correctes.
            Console.Write("\nIl vous reste {0} coup(s). Écrivez la colonne que vous visez : ", coupsRestants);
            colonneVisee = Console.ReadLine();
            while (!colonneCorrectementNommee(colonneVisee))
            {
                Console.Write("Erreur dans la lecture de la case ciblée, veuillez écrire une lettre entre A et J : ");
                colonneVisee = Console.ReadLine();
            }

            Console.Write("Écrivez la ligne que vous visez : ");
            ligneVisee = Console.ReadLine();
            while (!ligneCorrectementNommee(ligneVisee))
            {
                Console.Write("Erreur dans la lecture de la case ciblée, veuillez écrire un nombre entre 1 et 10 : ");
                ligneVisee = Console.ReadLine();
            }

            //Conversion des entrées en valeurs utilisables pour interagir avec le tableau de l'opposant
            //Premier élément = ligne (décrémentation), Deuxième élément = colonne (conversion ASCII)
            int[] choix = { (Convert.ToInt32(ligneVisee) - 1), ((char)Convert.ToChar(colonneVisee) - 65) };

            return choix;
        }

        public static bool colonneCorrectementNommee(string colonne)
        {
            string[] lettres = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };

            for (int i = 0; i < lettres.Length; i++)
            {
                if (colonne == lettres[i])
                    return true;
            }

            return false;
        }

        public static bool ligneCorrectementNommee(string ligne)
        {
            int ligneVisee;

            try
            {
                ligneVisee = Convert.ToInt32(ligne);
            }
            catch(FormatException) { return false; }

            for (int i = 0; i < 10; i++)
            {
                if (ligneVisee == (i+1))
                    return true;
            }

            return false;
        }

        public static bool Gagner(string[,] tab)
        {

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if ((tab[i, j] != "  ") && (tab[i, j] != "><"))
                    {
                        //si en balayant le tableau on trouve une case bateau active, ne gagne pas
                        return false;
                    }
                }
            }

            //si rien trouvé dans le bateau, gagne
            return true;
        }

    }
}