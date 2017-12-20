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

            //générer une grille pour l'adversaire et la sauvegarder
            int[,] sauvegardeEmplacementAdversaire = InitialiserGrilleRemplie(tabAdversaire);
            //générer une grille pour le joueur tant qu'il n'est pas satisfait et la sauvegarder
            int[,] sauvegardeEmplacementJoueur = InitialiserJoueur(tabJoueur);

            Console.WriteLine("\n################");
            Console.WriteLine("# Début du jeu #");
            Console.WriteLine("################");

            bool victoireJoueur = false, victoireAdversaire = false;
            int difficulte = 0;

            while (!victoireJoueur && !victoireAdversaire)
            {
                /*ENSEIGNANT : décommenter la ligne suivante pour avoir accès à la grille
                 *  de l'adversaire avec les bateaux affichés */
                //AfficherGrille(tabAdversaire);
                //AfficherGrilleCachee(tabAdversaire);
                //victoireJoueur = TourJoueur(tabAdversaire, sauvegardeEmplacementAdversaire);

                if (difficulte == 0)
                    victoireAdversaire = TourAdversaireFacile(tabJoueur, sauvegardeEmplacementJoueur);
                else if (difficulte == 1)
                    victoireAdversaire = TourAdversaireDifficile(tabJoueur, sauvegardeEmplacementJoueur);

            }

            Console.WriteLine("\nVICTOIRE !");

            Console.ReadKey();

        }

        /* TO DO:
         *  - fonction avancee d'attaque de proche en proche de l'adversaire
         *  - fonction de sauvegarde en cours de partie
         * */
         

        //PARTIE INITIALISATION DU JEU ET DES DONNEES//
        public static int[,] InitialiserJoueur(string[,] tabJoueur)
        {
            int[,] sauvegardeEmplacement;

            char happyCustomer = 'n';
            do
            {
                sauvegardeEmplacement = InitialiserGrilleRemplie(tabJoueur);
                AfficherGrille(tabJoueur);

                Console.Write("Souhaitez-vous garder ce placement (o/n) ? ");
                happyCustomer = Convert.ToChar(Console.ReadLine());

                while ((happyCustomer != 'o') && (happyCustomer != 'n'))
                {
                    Console.Write("Mauvais caractère, veuillez recommencer (o/n) : ");
                    happyCustomer = Convert.ToChar(Console.ReadLine());
                }
            } while (happyCustomer == 'n');

            return sauvegardeEmplacement;
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

        public static void AfficherGrilleCachee(string[,] tab)
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

        public static int[,] InitialiserGrilleRemplie(string[,] tab)
        {
            int[,] sauvegardeEmplacement = new int[5, 5]; //chaque ligne = un bateau, colonne1 = ligne, colonne2 = colonne, colonne3 = orientation, colonne4 = taille, 

            InitialiserGrilleVide(tab);
            for (int i = 0; i < 4; i++)
            {
                //on place le bateau sur la grille de l'adversaire et enregistre ses coordonnées;
                sauvegardeEmplacement = SauvegarderEmplacement( PlacerBateau((i + 2), tab), sauvegardeEmplacement, i);
            }
            sauvegardeEmplacement = SauvegarderEmplacement(PlacerBateau(3, tab), sauvegardeEmplacement, 4); //on place deux fois le bateau à 3 cases donc on le rappelle ici

            return sauvegardeEmplacement;

        }

        public static int[] PlacerBateau(int taille, string[,] tab)
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

            //on garde les coordonnées d'emplacement pour pouvoir vérifier si les bateaux sont toujours à flot plus tard
            int[] emplacement = { ligne, colonne, choixOrientation, taille };
            return emplacement;

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

        public static int[,] SauvegarderEmplacement(int[] emplacement, int[,] sauvegarde, int rang)
        {
            for (int j = 0; j < 4; j++)
            {
                sauvegarde[rang, j] = emplacement[j];
            }

            return sauvegarde;
        }



        //PARTIE JEU//
        public static bool TourJoueur(string[,] tabOpposant, int[,] sauvegardeEmplacement)
        {
            int nbSalves = BateauxRestants(tabOpposant, sauvegardeEmplacement); //pour calculer le nb de salves restantes
            int coupsRestants;

            for (int coups = 0; coups < nbSalves; coups++)
            {
                coupsRestants = nbSalves - coups;
                int[] caseVisee = CaseVisee(coupsRestants); //colonne et ligne choisies


                //Attaque vers adversaire
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

        public static bool TourAdversaireFacile(string[,] tabOpposant, int[,] sauvegardeEmplacement)
        //Tir totalement aléatoire, sans rappel des tirs précédents.
        {
            Random r = new Random();

            int nbSalves = BateauxRestants(tabOpposant, sauvegardeEmplacement); //pour calculer le nb de salves restantes
            int coupsRestants;

            Console.WriteLine(nbSalves);

            for (int coups = 0; coups < nbSalves; coups++)
            {
                coupsRestants = nbSalves - coups;
                int[] caseVisee = { r.Next(0, 10), r.Next(0, 10) };

                //Attaque vers adversaire
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

        public static bool TourAdversaireDifficile(string[,] tabOpposant, int[,] sauvegardeEmplacement)
        //Tir proche de zones correctement attaquées, avec rappel de tirs précédents.
        {
            Random r = new Random();

            int nbSalves = BateauxRestants(tabOpposant, sauvegardeEmplacement); //pour calculer le nb de salves restantes
            int coupsRestants;

            Console.WriteLine(nbSalves);

            for (int coups = 0; coups < nbSalves; coups++)
            {
                coupsRestants = nbSalves - coups;
                int[] caseVisee = { r.Next(0, 10), r.Next(0, 10) };

                //Attaque vers adversaire
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

            return Gagner(tabOpposant);
        }

        public static int[] CaseVisee(int coupsRestants)
        {

            Console.Write("\nIl vous reste {0} coup(s). Écrivez la case que vous visez (ColonneLigne) : ", coupsRestants);
            string caseVisee = Console.ReadLine();
            //on récupère le premier caractère pour la colonne, le restant des caractères pour le nombre (simple ou double chiffre)
            string colonneVisee = caseVisee.Substring(0, 1), ligneVisee = caseVisee.Substring(1);

            //on gère les erreurs de frappe et valeurs hors champs
            while (!colonneCorrectementNommee(colonneVisee) || !ligneCorrectementNommee(ligneVisee))
            {
                Console.Write("Erreur dans la lecture de la case ciblée, veuillez recommencer : ");
                caseVisee = Console.ReadLine();
                colonneVisee = caseVisee.Substring(0, 1); ligneVisee = caseVisee.Substring(1);
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
            catch (FormatException) { return false; }

            for (int i = 0; i < 10; i++)
            {
                if (ligneVisee == (i + 1))
                    return true;
            }

            return false;
        }

        public static int BateauxRestants(string[,] tabBateaux, int[,] sauvegarde)
        {
            //ici, sauvegarde = {ligne, colonne, choixOrientation, taille}
            int compteurBateauxRestants = 5;
            int compteurEtatBateau;

            for (int i = 0; i < 5; i++) //on parcourt les 5 bateaux enregistrés
            {
                compteurEtatBateau = sauvegarde[i, 3]; //le bateau a [taille] points de vie

                for (int k = 0; k < sauvegarde[i, 3]; k++) //on parcourt la taille (sauvegarde[i, 3]) du bateau i
                {
                    //on explore différemment selon orientation
                    if ( ((sauvegarde[i, 2]) == 0) //nord
                        //on compare la (position d'origine + déplacement de k cases dans la bonne orientation) à une case qui serait attaquée
                        && (tabBateaux[sauvegarde[i, 0] - k, sauvegarde[i, 1]] == "><" ) )
                    {
                        compteurEtatBateau--; //donc on enlève un point de vie au bateau si on trouve une case attaquée
                    }
                    else if ( ((sauvegarde[i, 2]) == 1) //sud
                        && (tabBateaux[sauvegarde[i, 0] + k, sauvegarde[i, 1]] != Convert.ToString(sauvegarde[i, 3] * 11)) )
                    {
                        compteurEtatBateau--;
                    }
                    else if ( ((sauvegarde[i, 2]) == 2) //ouest
                        && (tabBateaux[sauvegarde[i, 0], sauvegarde[i, 1] - k] != Convert.ToString(sauvegarde[i, 3] * 11)) )
                    {
                        compteurEtatBateau--;
                    }
                    else if ( ((sauvegarde[i, 2]) == 3) //est
                        && (tabBateaux[sauvegarde[i, 0], sauvegarde[i, 1] + k] != Convert.ToString(sauvegarde[i, 3] * 11)) )
                    {
                        compteurEtatBateau--;
                    }
                    
                }

                if (compteurEtatBateau == 0) //si le bateau que l'on vient d'étudier n'a plus de case en vie, on le supprime des bateaux restants
                    compteurBateauxRestants--;

            }

            return compteurBateauxRestants;
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