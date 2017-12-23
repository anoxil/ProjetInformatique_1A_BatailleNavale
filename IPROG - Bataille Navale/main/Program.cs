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

            //joueur = 0, ia = 1
            bool victoireJoueur = false, victoireAdversaire = false;
            int difficulte = 1;
            int[] bonCoup = new int[2];

            while (!victoireJoueur && !victoireAdversaire)
            {
                /*ENSEIGNANT : décommentez la ligne suivante pour avoir accès à la grille
                 *  de l'adversaire avec les bateaux affichés */
                //AfficherGrille(tabAdversaire);

                AfficherPlateauDeJeu(tabJoueur, tabAdversaire);
                victoireJoueur = TourDeJeu(tabAdversaire, sauvegardeEmplacementAdversaire, 0, difficulte);
                if (victoireJoueur) break;

                if (difficulte == 0)
                    victoireAdversaire = TourDeJeu(tabJoueur, sauvegardeEmplacementJoueur, 1, 0);
                else if (difficulte == 1)
                    victoireAdversaire = TourDeJeu(tabJoueur, sauvegardeEmplacementJoueur, 1, 1);
            }

            if (victoireJoueur) Console.WriteLine("\nVictoire du joueur.");
            if (victoireAdversaire) Console.WriteLine("\nVictoire de l'adversaire.");

            Console.ReadKey();

        }

        /* TO DO:
         *  - fonction de sauvegarde en cours de partie
         *  - optionnel : rajouter un mode 1vs1 avec deux joueurs irl
         *  - optionnel : modifier le système des cases de string[,] à int[,] avec "XX" à X --> modifier affichage + fonctions
         *  - optionnel : améliorer l'ia difficile
         *  - optionnel : fonction qui lorsqu'appelée énumère les cases déjà visées ou mieux, les affiche sur une nouvelle grille
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
            Console.Write("\n  A  B  C  D  E  F  G  H  I  J\n");
            for (int i = 0; i < tab.GetLength(0); i++)
            {
                Console.WriteLine("+--+--+--+--+--+--+--+--+--+--+");
                for (int j = 0; j < tab.GetLength(1); j++)
                {
                    if ((tab[i, j] == "  ") || (tab[i, j] == "><") || (tab[i, j] == "bc")) //affiche morceaux des bateaux touchés, mais pas les morceaux non touchés
                    {
                        if (tab[i, j] == "bc") { Console.Write("|><"); continue; }
                        Console.Write("|" + tab[i, j]);
                    }
                    else //donc si "<>" qui est un morceau de bâteau non touché
                    {
                        Console.Write("|  "); //on affiche des espaces à la place
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

        public static void AfficherPlateauDeJeu(string[,] tabJoueur, string[,] tabOpposant)
        //Fonction bonus: combinaison plus esthétique et pratique (double lecture d'informations) des deux précédentes fonctions
        {
            Console.WriteLine("\n\tVotre grille\t\t\t\t     Grille de l'adversaire");
            Console.Write("\n  A  B  C  D  E  F  G  H  I  J\t\t\t  A  B  C  D  E  F  G  H  I  J\n");
            for (int i = 0; i < tabJoueur.GetLength(0); i++)
            {
                Console.Write("+--+--+--+--+--+--+--+--+--+--+"); Console.Write("\t\t|\t"); Console.WriteLine("+--+--+--+--+--+--+--+--+--+--+");
                for (int j = 0; j < tabJoueur.GetLength(1); j++)
                {
                    if (tabJoueur[i, j] == "bc") { Console.Write("|><"); continue; }
                    Console.Write("|" + tabJoueur[i, j]);
                    if (j == 9) Console.Write("|");
                }
                Console.Write(" " + (i + 1));

                Console.Write("\t|\t");

                for (int j = 0; j < tabOpposant.GetLength(1); j++)
                {
                    if ((tabOpposant[i, j] == "  ") || (tabOpposant[i, j] == "><") || (tabOpposant[i, j] == "bc")) //affiche morceaux des bateaux touchés, mais pas les morceaux non touchés
                    {
                        if (tabOpposant[i, j] == "bc") { Console.Write("|><"); continue; }
                        Console.Write("|" + tabOpposant[i, j]);
                    }
                    else //donc si "<>" qui est un morceau de bâteau non touché
                    {
                        Console.Write("|  "); //on affiche des espaces à la place
                    }

                    if (j == 9)
                    {
                        Console.Write("|");
                    }
                }
                Console.WriteLine(" " + (i + 1));

            }
            Console.Write("+--+--+--+--+--+--+--+--+--+--+"); Console.Write("\t\t|\t"); Console.WriteLine("+--+--+--+--+--+--+--+--+--+--+\n");
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
        public static bool TourDeJeu(string[,] tabOpposant, int[,] sauvegardeEmplacement, int joueur, int difficulte)
        {

            int nbSalves = BateauxRestants(tabOpposant, sauvegardeEmplacement); //pour calculer le nb de salves restantes
            int coupsRestants;
            string contenuCaseVisee;

            int[] bonCoupTourActuel = { -1, -1 };
            for (int m = 0; m < 10; m++) //on parcourt la grille
            {
                for (int n = 0; n < 10; n++)
                {
                    if (tabOpposant[m, n] == "bc") { //on récupère les coordonnées si on tombe sur la case bon coup
                        bonCoupTourActuel[0] = m;
                        bonCoupTourActuel[1] = n;
                        tabOpposant[m, n] = "><"; //on marque l'ancienne case bon coup avec la croix
                    }
                }
            }

            int[] bonCoupTourSuivant = { -1, -1 }; //permet de retenir la case correctement visée pour l'IA difficile
            //peut-être ne pas réinitialiser à -1 mais plutôt en même temps que -----Actuel (maintien info sur plusieurs tours)

            for (int coups = 0; coups < nbSalves; coups++)
            {
                coupsRestants = nbSalves - coups;
                //colonne et ligne choisies
                int[] caseVisee = ViserCase(coupsRestants, joueur, difficulte, bonCoupTourActuel);
                bonCoupTourActuel[0] = -1; //on l'a utilisé une fois, on change sa valeur pour ne plus utiliser la variable

                //Attaque vers adversaire
                //caractère de croix '><' à mettre quand on touche un morceau de bâteau.
                contenuCaseVisee = tabOpposant[caseVisee[0], caseVisee[1]];
                if ((contenuCaseVisee == "22")
                    || (contenuCaseVisee == "33")
                    || (contenuCaseVisee == "44")
                    || (contenuCaseVisee == "55"))
                {
                    tabOpposant[caseVisee[0], caseVisee[1]] = "><";
                    if ((joueur == 1) && (difficulte == 1)) //si tour IA difficile, on retient les coordonnées de la case pour attaquer proche
                    {
                        bonCoupTourSuivant[0] = caseVisee[0];
                        bonCoupTourSuivant[1] = caseVisee[1];
                    }

                }

            }

            if ((joueur == 1) && (difficulte == 1) && (bonCoupTourSuivant[0] != -1)) //tour IA difficile, si une case bateau a été attaquée
            {
                tabOpposant[bonCoupTourSuivant[0], bonCoupTourSuivant[1]] = "bc";
                //on marque la dernière case bien visée avec "bc" pour la détecter au tour suivant
            }

            return Gagner(tabOpposant);
        }
        
        public static int[] ViserCase(int coupsRestants, int joueur, int difficulte, int[] bonCoup)
        {
            Random r = new Random();

            //SI c'est le tour de l'IA au niveau facile
            if ((joueur == 1) && (difficulte == 0)) 
            {
                int[] choixIA = { r.Next(0, 10), r.Next(0, 10) };
                return choixIA;
            }


            //SI c'est le tour de l'IA au niveau difficile
            else if ((joueur == 1) && (difficulte == 1)) 
            {
                if (bonCoup[0] == -1)
                { int[] choixIA = { r.Next(0, 10), r.Next(0, 10) }; return choixIA; }
                else
                {
                    int ligne = 0; int colonne = 0;
                    do
                    {
                        int orientation = r.Next(0, 4);
                        //Console.WriteLine(bonCoup[0] + " - " + bonCoup[1] + " - " + orientation);
                        if (orientation == 0) //nord
                        { ligne = bonCoup[0] - 1; colonne = bonCoup[1]; }
                        else if (orientation == 1) //sud
                        { ligne = bonCoup[0] + 1; colonne = bonCoup[1]; }
                        else if (orientation == 2) //ouest
                        { ligne = bonCoup[0]; colonne = bonCoup[1] - 1; }
                        else if (orientation == 3) //est
                        { ligne = bonCoup[0]; colonne = bonCoup[1] + 1; }
                    } while ((ligne > 9) || (ligne < 0) || (colonne > 9) || (colonne < 0));

                    int[] choixIA = { ligne, colonne };
                    return choixIA;
                }
            }


            //SI c'est le tour du joueur
            Console.Write("Il vous reste {0} coup(s). Écrivez la case que vous visez (ColonneLigne) : ", coupsRestants);
            string caseVisee = Console.ReadLine();
            //on récupère le premier caractère pour la colonne, le restant des caractères pour le nombre (simple ou double chiffre)
            string colonneVisee = caseVisee.Substring(0, 1), ligneVisee = caseVisee.Substring(1);

            //on gère les erreurs de frappe et valeurs hors champs
            while (!ColonneCorrectementNommee(colonneVisee) || !LigneCorrectementNommee(ligneVisee))
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

        public static bool ColonneCorrectementNommee(string colonne)
        {
            string[] lettres = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };

            for (int i = 0; i < lettres.Length; i++)
            {
                if (colonne == lettres[i])
                    return true;
            }

            return false;
        }

        public static bool LigneCorrectementNommee(string ligne)
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
                        //on compare la (position d'origine + déplacement de k cases dans la bonne orientation) à une case qui serait attaquée ou enregistrée comme "bon coup"
                        && ( (tabBateaux[sauvegarde[i, 0] - k, sauvegarde[i, 1]] == "><" ) || (tabBateaux[sauvegarde[i, 0] - k, sauvegarde[i, 1]] == "><") ) )
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