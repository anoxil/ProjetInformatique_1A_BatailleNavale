using System;
using System.IO;
using System.Text;

namespace main
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.SetWindowSize(100, 40); //peut poser problème selon l'OS. à commenter/modifier manuellement la taille de l'écran selon besoin.
            int choix = 3;
            while (choix == 3)
                choix = MenuJeu(); //présentation du menu principal

            string[,] tabJoueur = new string[10, 10]; //la matrice en 10 par 10 de la grille du joueur
            string[,] tabAdversaire = new string[10, 10];

            int[,] sauvegardeEmplacementJoueur = new int[5, 4]; //chaque ligne = un bateau, colonne1 = ligne, colonne2 = colonne, colonne3 = orientation, colonne4 = taille, 
            int[,] sauvegardeEmplacementAdversaire = new int[5, 4];

            int difficulte = -1;

            difficulte = InitialisationGenerale(tabJoueur, tabAdversaire, sauvegardeEmplacementJoueur, sauvegardeEmplacementAdversaire, choix);

            Console.Clear();
            Console.WriteLine("\n################");
            Console.WriteLine("# Début du jeu #");
            Console.WriteLine("################");

            Console.WriteLine("\nN'oubliez pas que vous pouvez sauvegarder et quitter à tout moment avec la commande P9 !");

            //joueur = 0, ia = 1
            int victoireJoueur = -1, victoireAdversaire = -1;

            while (victoireJoueur != 1 && victoireAdversaire != 1)
            {
                /*CHER(E) ENSEIGNANT(E) : décommentez la ligne suivante pour avoir accès à chaque tour
                 *  à la grille de l'adversaire avec les bateaux affichés et pouvoir tricher :) */
                //AfficherGrille(tabAdversaire);

                AfficherPlateauDeJeu(tabJoueur, tabAdversaire);

                victoireJoueur = TourDeJeu(tabAdversaire, sauvegardeEmplacementAdversaire, 0, difficulte);
                if (victoireJoueur == 2)
                {
                    SauvegarderJeu(tabJoueur, tabAdversaire, sauvegardeEmplacementJoueur, sauvegardeEmplacementAdversaire, difficulte);
                    break;
                }
                if (victoireJoueur == 1)
                {
                    break;
                }

                if (difficulte == 0)
                    victoireAdversaire = TourDeJeu(tabJoueur, sauvegardeEmplacementJoueur, 1, 0);
                else if (difficulte == 1)
                    victoireAdversaire = TourDeJeu(tabJoueur, sauvegardeEmplacementJoueur, 1, 1);
            }

            if (victoireJoueur == 1) Console.WriteLine("\nLe joueur a gagné.....");
            if (victoireAdversaire == 1) Console.WriteLine("\nWOOOOOOOW l'ordinateur a vaincu le joueur !!!! Félicitation à ce prodige !");

            Console.ReadKey();

        }

        //PARTIE INITIALISATION DU JEU ET DES DONNEES//
        public static int MenuJeu()
        //une simple présentation graphique du menu principal
        {
            Console.WriteLine("\n ##################################################################################################");
            Console.WriteLine(" # ______       _        _ _ _        _   _                  _ _        _____       _             #");
            Console.WriteLine(" # | ___ \\     | |      (_) | |      | \\ | |                | | |      / ___ |     | |            #");
            Console.WriteLine(" # | |_/ / __ _| |_ __ _ _| | | ___  |  \\| | __ ___   ____ _| | | ___  \\ `--.  __ _| |_   _____   #");
            Console.WriteLine(" # | ___ \\/ _` | __/ _` | | | |/ _ \\ | . ` |/ _` \\ \\ / / _` | | |/ _ \\  `--. \\/ _` | \\ \\ / / _ \\  #");
            Console.WriteLine(" # | |_/ / (_| | || (_| | | | |  __/ | |\\  | (_| |\\ V / (_| | | |  __/ /\\__/ / (_| | |\\ V / (_) | #");
            Console.WriteLine(" # \\____/ \\__,_|\\__\\__,_|_|_|_|\\___| \\_| \\_/\\__,_| \\_/ \\__,_|_|_|\\___| \\____/ \\__,_|_| \\_/ \\___/  #");
            Console.WriteLine(" #                                                                                                #");
            Console.WriteLine(" ##################################################################################################");

            Console.Write("\n\n");

            Console.WriteLine("Bienvenue dans le jeu de Bataille Navale Salvo !\n\nMenu du jeu :\n");

            Console.WriteLine("1. Nouvelle Partie");
            Console.WriteLine("2. Continuer Partie");
            Console.WriteLine("3. Instructions");

            int choix = -1;

            do
            {
                Console.Write("\n:");
                try
                {
                    choix = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("Apparemment les try-catch vont devenir mes amis... Il faut écrire des chiffres !");
                }
                if ((choix != 1) && (choix != 2) && (choix != 3))
                {
                    choix = -1;
                    Console.WriteLine("Seuls les chiffres du menu sont acceptés.");
                }
            } while (choix == -1);


            if (choix == 3)
            {
                Console.Clear();
                StreamReader file = new StreamReader("instructions.txt", Encoding.GetEncoding("iso-8859-1"));
                //norme iso-8859-1 pour faire apparaître les accents. pas de solution pour les apostrophes...

                string ligne = "";
                while ((ligne = file.ReadLine()) != null)//afficher chaque ligne tant qu'il en existe
                    Console.WriteLine(ligne);
                Console.ReadKey();
                Console.Clear();
            }

            return choix;

        }

        public static int InitialisationGenerale(string[,] tabJoueur, string[,] tabAdversaire, int[,] sauvegardeEmplacementJoueur, int[,] sauvegardeEmplacementAdversaire, int initPartie)
        {

            int difficulte = -1;

            if (initPartie == 2) //si le choix au menu du jeu était 2, donc "Continuer Partie"
            {
                difficulte = RecupererJeu(tabJoueur, tabAdversaire, sauvegardeEmplacementJoueur, sauvegardeEmplacementAdversaire);
                return difficulte;
            }

            else if (initPartie == 1) //si le choix au menu du jeu était 1, donc "Nouvelle Partie"
            {
                //générer une grille pour l'adversaire et la sauvegarder
                InitialiserGrilleRemplie(tabAdversaire, sauvegardeEmplacementAdversaire);
                //générer une grille pour le joueur tant qu'il n'est pas satisfait et la sauvegarder
                InitialiserJoueur(tabJoueur, sauvegardeEmplacementJoueur);

                //ask for difficulté
                Console.Write("Souhaitez-vous jouer en difficulté facile (0) ou difficile (1) ? ");
                difficulte = Convert.ToInt32(Console.ReadLine());
                while ((difficulte != 0) && (difficulte != 1))
                {
                    Console.Write("Mauvais choix, veuillez recommencer (0/1) : ");
                    difficulte = Convert.ToInt32(Console.ReadLine());
                }

                return difficulte;
            }

            return -1;
        }

        public static void InitialiserJoueur(string[,] tabJoueur, int[,] sauvegardeEmplacement)
        //Le joueur choisit la grille qui lui convient
        {

            char happyCustomer = 'N';
            do //tant que la grille ne lui convient pas (happyCustomer vaut n), on propose une nouvelle grille
            {
                InitialiserGrilleRemplie(tabJoueur, sauvegardeEmplacement); //la grille tabJoueur est enregistrée. Si le joueur ne souhaite pas la conserver, on enregistre alors la nouvelle grille, jusqu’à conservation de la grille
                AfficherGrille(tabJoueur);

                Console.Write("Souhaitez-vous garder ce placement (O/N) ? ");
                happyCustomer = Convert.ToChar(Console.ReadLine());

                while ((happyCustomer != 'O') && (happyCustomer != 'N')) //le joueur ne doit entrer que les caractères O ou N, les autres sont refusés
                {
                    Console.Write("Mauvais caractère, veuillez recommencer (O/N) : ");
                    happyCustomer = Convert.ToChar(Console.ReadLine());
                }
            } while (happyCustomer == 'N'); //quand la grille convient au joueur (happyCustomer vaut O), on sauvegarde la grille et on ne la change plus

        }

        public static void AfficherGrille(string[,] tab)
        //on affiche la grille du joueur, avec les placements des bateaux
        {
            Console.Write("\n  A  B  C  D  E  F  G  H  I  J\n");
            for (int i = 0; i < tab.GetLength(0); i++)
            {
                Console.WriteLine("+--+--+--+--+--+--+--+--+--+--+");
                for (int j = 0; j < tab.GetLength(1); j++) //pour chaque case du tableau, on affiche sa valeur (case vide, portion de bateau non touchée, portion de bateau touchée)
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
        //on affiche la grille du joueur adversaire, sans donner l’emplacement des bateaux
        {
            Console.Write("\n  A  B  C  D  E  F  G  H  I  J\n");
            for (int i = 0; i < tab.GetLength(0); i++)
            {
                Console.WriteLine("+--+--+--+--+--+--+--+--+--+--+");
                for (int j = 0; j < tab.GetLength(1); j++)
                {
                    if ((tab[i, j] == "  ") || (tab[i, j] == "><") || (tab[i, j] == "bc")) //affiche morceaux des bateaux touchés, mais pas les morceaux non touchés
                    {
                        //bc est pour “bon coup” (cf fonction TourDeJeu, lignes 488 et 520)
                        if (tab[i, j] == "bc") { Console.Write("|><"); continue; } //on affiche “><” au lieu de “bc”, pour signaler au joueur que le bateau a été touché (“bc” sert à la programmation mais n’est pas présenté au joueur)
                        Console.Write("|" + tab[i, j]);
                    }
                    else //donc si "11",”22”,”33”,”44” ou “55” qui correspondent à des morceaux de bâteau non touché
                    {
                        Console.Write("|  "); //on affiche des espaces à la place pour ne pas donner l’emplacement
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
            Console.WriteLine("\n\tVotre grille\t\t\t\t     Grille de l'adversaire"); //Affiche la grille du joueur et la grille cachée de l'adversaire à l'horizontale
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
                    if ((tabOpposant[i, j] == "  ") || (tabOpposant[i, j] == "><") || (tabOpposant[i, j] == "bc"))
                    {
                        if (tabOpposant[i, j] == "bc") { Console.Write("|><"); continue; }
                        Console.Write("|" + tabOpposant[i, j]);
                    }
                    else
                    {
                        Console.Write("|  ");
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
                    tab[j, i] = "  "; //Chaque case de la grille est initialisée comme vide
                }
            }
        }

        public static void InitialiserGrilleRemplie(string[,] tab, int[,] sauvegardeEmplacement)
        {

            InitialiserGrilleVide(tab); //on crée une grille vide
            for (int i = 0; i < 4; i++)
            {
                //on place le bateau sur la grille et enregistre ses coordonnées;
                sauvegardeEmplacement = SauvegarderEmplacement(PlacerBateau((i + 2), tab), sauvegardeEmplacement, i); //on place un bateau de chaque taille (taille 2, 3, 4 et 5)
                //PlacerBateau() renvoie les 4 informations de placement des bateaux (ligne, colonne, orientation et taille). On les enregistre dans le tableau sauvegardeEmplacement, à la ligne i pour le bateau i.
            }
            sauvegardeEmplacement = SauvegarderEmplacement(PlacerBateau(3, tab), sauvegardeEmplacement, 4); //on place deux fois le bateau à 3 cases donc on le rappelle ici

        }

        public static int[] PlacerBateau(int taille, string[,] tab)
        {
            Random r = new Random();
            int ligne = 0;
            int colonne = 0;
            int choixOrientation;
            bool cheminUtilisable = false;

            //on cherche un emplacement disponible pour le bateau. On continue tant que ce n’est pas le cas
            do
            {
                //on trouve une case vide. On continue tant que ce n’est pas le cas
                do
                {
                    ligne = r.Next(0, 10); //on choisit une case au hasard dans la grille
                    colonne = r.Next(0, 10);
                } while (tab[ligne, colonne] != "  ");

                //on cherche soit nord, soit sud, soit est, soit ouest pour un emplacement vide de la taille du bateau, 4 directions
                choixOrientation = r.Next(0, 4);
                cheminUtilisable = EmplacementEstVide(ligne, colonne, choixOrientation, taille, tab);


            } while (!cheminUtilisable);

            //quand on a trouvé un chemin utilisable, on remplit les cases du tableau pour marquer le bateau
            RemplirEmplacement(ligne, colonne, choixOrientation, taille, tab);

            //on garde les coordonnées d'emplacement pour pouvoir vérifier si les bateaux sont toujours à flot plus tard
            int[] emplacement = { ligne, colonne, choixOrientation, taille };
            return emplacement;

        }

        public static bool EmplacementEstVide(int ligne, int colonne, int choix, int taille, string[,] tab)
        //retourne True si l’emplacement est vide, False sinon
        {
            //Vers le Nord (si le choix d’orientation vaut 0)
            if (choix == 0)
            {
                for (int k = 0; k < taille; k++)
                {
                    //if outofrangeexception, return false
                    try
                    {
                        if (tab[ligne - k, colonne] != "  ") //on regarde si il y a assez de cases libres au dessus pour placer le bateau
                            return false; //si la case n’est pas vide, on retourne false
                    }
                    catch (IndexOutOfRangeException) //la case n’existe pas (IndexOutOfRange), donc n’est pas considérée comme vide, on retourne false
                    {
                        return false;
                    }
                }
            }

            //Vers le Sud (choix d’orientation vaut 1)
            else if (choix == 1)
            {
                for (int k = 0; k < taille; k++)
                {
                    try
                    {
                        if (tab[ligne + k, colonne] != "  ") //on regarde si il y a assez de cases libres en dessous pour placer le bateau
                            return false;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        return false;
                    }
                }
            }

            //Vers l’Ouest (choix d’orientation vaut 2)
            else if (choix == 2)
            {
                for (int k = 0; k < taille; k++)
                {
                    try
                    {
                        if (tab[ligne, colonne - k] != "  ") //on regarde si il y a assez de cases libres à gauche pour placer le bateau
                            return false;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        return false;
                    }
                }
            }

            //Vers l’Est (choix d’orientation vaut 3)
            else if (choix == 3)
            {
                for (int k = 0; k < taille; k++)
                {
                    try
                    {
                        if (tab[ligne, colonne + k] != "  ") //on regarde si il y a assez de cases libres à droite pour placer le bateau
                            return false;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        return false;
                    }
                }
            }

            return true; //si les cases sont vides, on retourne true
        }

        public static void RemplirEmplacement(int ligne, int colonne, int choix, int taille, string[,] tab)
        {
            //nord
            if (choix == 0)
            {
                for (int k = 0; k < taille; k++) //on place les bateaux en fonction de leur taille, dans la direction choisie (ici vers le haut)
                {
                    tab[ligne - k, colonne] = String.Format("{0}{0}", taille); //les portions de bateaux sont du format “taille taille”. Pour un bateau de taille 3, on remplit trois cases alignées avec les caractères “33”
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
        //rang correspond aux 5 bateaux. Il augmente de 1 dès qu’un bateau est placé. Correspond au i de la fonction initialiserGrilleRemplie(cf ligne 292)
        {
            for (int j = 0; j < 4; j++) //j correspond aux 4 informations contenues dans le tableau emplacement (ligne, colonne, orientation et taille)
            {
                sauvegarde[rang, j] = emplacement[j]; //on entre dans le tableau sauvegarde les données de chaque bateau
            }

            return sauvegarde;
        }



        //PARTIE JEU//
        public static int TourDeJeu(string[,] tabOpposant, int[,] sauvegardeEmplacement, int joueur, int difficulte)
        //joueur = 0, ia = 1
        {

            int nbSalves = BateauxRestants(tabOpposant, sauvegardeEmplacement);  //Donne le nombre de salves restantes, donc nombre de coups à chaque tour
            int coupsRestants;
            string contenuCaseVisee;

            int[] bonCoupTourActuel = { -1, -1 };
            for (int m = 0; m < 10; m++) //on parcourt la grille
            {
                for (int n = 0; n < 10; n++)
                {
                    if (tabOpposant[m, n] == "bc")
                    { //on récupère les coordonnées du bon coup précédent si on tombe sur la case bon coup ("bc")
                        bonCoupTourActuel[0] = m;
                        bonCoupTourActuel[1] = n;
                        tabOpposant[m, n] = "><"; //on marque l'ancienne case bon coup avec la croix
                    }
                }
            }

            int[] bonCoupTourSuivant = { -1, -1 }; //permet de retenir la case correctement visée pour l'IA difficile

            for (int coups = 0; coups < nbSalves; coups++)
            {
                coupsRestants = nbSalves - coups;
                //colonne et ligne choisies
                int[] caseVisee = ViserCase(coupsRestants, joueur, difficulte, bonCoupTourActuel);
                bonCoupTourActuel[0] = -1; //on l'a utilisé une fois, on change sa valeur pour ne plus utiliser la variable

                if (caseVisee[0] == -1) //si le joueur a entré la commande "P9" pendant la fonction ViserCase
                {
                    return 2;
                }

                //Attaque vers adversaire
                //caractère de croix '><' à mettre quand on touche un morceau de bâteau.
                contenuCaseVisee = tabOpposant[caseVisee[0], caseVisee[1]];
                if ((contenuCaseVisee == "22")
                    || (contenuCaseVisee == "33")
                    || (contenuCaseVisee == "44")
                    || (contenuCaseVisee == "55"))
                {
                    tabOpposant[caseVisee[0], caseVisee[1]] = "><"; //si la case visée correspond à un morceau de bateau, on indique que le bateau est touché par “><”
                    if ((joueur == 1) && (difficulte == 1)) //si l’IA est difficile, on retient les coordonnées de la case pour pouvoir attaquer autour au tour suivant
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
                if (bonCoup[0] == -1) //s'il n'y a pas eu de bon coup au tour précédent
                { int[] choixIA = { r.Next(0, 10), r.Next(0, 10) }; return choixIA; }
                else //s'il y a eu un bon coup au tour précédent
                {
                    int ligne = 0; int colonne = 0;
                    do
                    {
                        int orientation = r.Next(0, 4);
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


            if (colonneVisee == "P" && Convert.ToInt32(ligneVisee) == 9) //si le joueur a rentrer la commande "P9" et veut donc sauvegarder
            {
                int[] save = { -1, -1 };
                return save; //on retourne un tableau identifiable dans la fonction parente
            }


            //Conversion des entrées en valeurs utilisables pour interagir avec le tableau de l'opposant
            //Premier élément = ligne (décrémentation), Deuxième élément = colonne (conversion ASCII)
            int[] choix = { (Convert.ToInt32(ligneVisee) - 1), ((char)Convert.ToChar(colonneVisee) - 65) };
            return choix;
        }

        public static bool ColonneCorrectementNommee(string colonne)
        {
            string[] lettres = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "P" }; //liste des lettres acceptées

            for (int i = 0; i < lettres.Length; i++)
            {
                if (colonne == lettres[i])
                    return true; //si la colonne saisie est comprise dans cette liste, elle est correcte
            }

            return false; //sinon, elle n’est pas correcte
        }

        public static bool LigneCorrectementNommee(string ligne)
        {
            int ligneVisee;

            try
            {
                ligneVisee = Convert.ToInt32(ligne);  //on regarde si la valeur de ligne saisie est bien un entier
            }
            catch (FormatException) { return false; } //ce n’est pas un entier (FormatException) : la ligne saisie est incorrecte

            for (int i = 0; i < 10; i++) //on regarde si la ligne visée est bien comprise entre 0 et 9 (et donc existe bien dans le tableau)
            {
                if (ligneVisee == (i + 1))
                    return true; //la ligne saisie est correcte
            }

            return false; //sinon elle est incorrecte
        }

        public static int BateauxRestants(string[,] tabBateaux, int[,] sauvegarde)
        //retourne le nombre de bateaux encore actifs
        {
            //ici, sauvegarde = {ligne, colonne, choixOrientation, taille}
            int compteurBateauxRestants = 5;
            int compteurEtatBateau;

            for (int i = 0; i < 5; i++) //on parcourt les 5 bateaux enregistrés, les lignes du tableau sauvegarde
            {
                compteurEtatBateau = sauvegarde[i, 3]; //le bateau a [taille] points de vie. Les vies sont donc données en index [i, 3]

                for (int k = 0; k < sauvegarde[i, 3]; k++) //on parcourt la taille (sauvegarde[i, 3]) du bateau i
                {
                    //on explore différemment selon l'orientation (sauvegarde[i, 2])
                    if (((sauvegarde[i, 2]) == 0) //nord
                                                  //on compare la (position d'origine + déplacement de k cases dans la bonne orientation) à une case qui serait attaquée ou enregistrée comme "bon coup"
                        && ((tabBateaux[sauvegarde[i, 0] - k, sauvegarde[i, 1]] == "><") || (tabBateaux[sauvegarde[i, 0] - k, sauvegarde[i, 1]] == "><"))) //on parcourt l’emplacement du bateau, donc vers le haut. Si une case ne vaut plus taille*11, la portion de bateau correspondante est touchée
                    {
                        compteurEtatBateau--; //donc on enlève un point de vie au bateau si on trouve une case attaquée
                    }
                    else if (((sauvegarde[i, 2]) == 1) //sud
                        && (tabBateaux[sauvegarde[i, 0] + k, sauvegarde[i, 1]] != Convert.ToString(sauvegarde[i, 3] * 11)))
                    {
                        compteurEtatBateau--;
                    }
                    else if (((sauvegarde[i, 2]) == 2) //ouest
                        && (tabBateaux[sauvegarde[i, 0], sauvegarde[i, 1] - k] != Convert.ToString(sauvegarde[i, 3] * 11)))
                    {
                        compteurEtatBateau--;
                    }
                    else if (((sauvegarde[i, 2]) == 3) //est
                        && (tabBateaux[sauvegarde[i, 0], sauvegarde[i, 1] + k] != Convert.ToString(sauvegarde[i, 3] * 11)))
                    {
                        compteurEtatBateau--;
                    }

                }

                if (compteurEtatBateau == 0) //si le bateau que l'on vient d'étudier n'a plus de case en vie, on le supprime des bateaux restants
                    compteurBateauxRestants--;

            }

            return compteurBateauxRestants;
        }

        public static int Gagner(string[,] tab)
        {

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if ((tab[i, j] != "  ") && (tab[i, j] != "><") && (tab[i, j] != "bc"))
                    {
                        //si en balayant le tableau on trouve une case bateau active (de type taille*11), ne gagne pas
                        return 0;
                    }
                }
            }

            //si rien trouvé dans le bateau, gagne
            return 1;
        }

        public static void SauvegarderJeu(string[,] tabJoueur, string[,] tabAdversaire, int[,] sauvegardeEmplacementJoueur, int[,] sauvegardeEmplacementAdversaire, int difficulte)
        {
            StreamWriter sw = new StreamWriter("data_save.txt"); //on crée un document texte .txt pour sauvegarder les données

            //écrire tabJoueur
            for (int ligneJ = 0; ligneJ < 10; ligneJ++)
            {
                for (int colonneJ = 0; colonneJ < 10; colonneJ++)
                {
                    sw.WriteLine(tabJoueur[ligneJ, colonneJ]); //on y écrit la grille actuelle du joueur (emplacements des bateaux), qui est alors sauvegardé dans le document .txt
                }
            }

            //écrire tabAdversaire
            for (int ligneA = 0; ligneA < 10; ligneA++)
            {
                for (int colonneA = 0; colonneA < 10; colonneA++)
                {
                    sw.WriteLine(tabAdversaire[ligneA, colonneA]); //celle de l’adversaire
                }
            }

            //écrire emplacements joueur
            for (int bateauJ = 0; bateauJ < 5; bateauJ++)
            {
                for (int donneeJ = 0; donneeJ < 4; donneeJ++)
                {
                    sw.WriteLine(sauvegardeEmplacementJoueur[bateauJ, donneeJ]); //les données sur les bateaux (ligne, colonne, orientation, taille)
                }
            }

            //écrire emplacements adversaire
            for (int bateauA = 0; bateauA < 5; bateauA++)
            {
                for (int donneeA = 0; donneeA < 4; donneeA++)
                {

                    sw.WriteLine(sauvegardeEmplacementAdversaire[bateauA, donneeA]); //l’emplacement des bateaux de l’adversaire
                }
            }

            //écrire difficulté
            sw.WriteLine(difficulte); //on y sauvegarde le niveau de difficulté

            sw.Close();
        }

        public static int RecupererJeu(string[,] tabJoueur, string[,] tabAdversaire, int[,] sauvegardeEmplacementJoueur, int[,] sauvegardeEmplacementAdversaire)
        //pour récupérer les données sauvegardées
        {
            StreamReader sr = new StreamReader("data_save.txt"); //on lit le document .txt créé et rempli dans sauvegarderJeu. Il faut les récupérer dans l’ordre dans lequel elles ont été entrées

            //récup tabJoueur
            for (int ligneJ = 0; ligneJ < 10; ligneJ++)
            {
                for (int colonneJ = 0; colonneJ < 10; colonneJ++)
                {
                    tabJoueur[ligneJ, colonneJ] = sr.ReadLine(); //on lit la grille contenant les emplacements dans le document .txt et on la replace dans le tableau tabJoueur
                }
            }

            //récup tabAdversaire
            for (int ligneA = 0; ligneA < 10; ligneA++)
            {
                for (int colonneA = 0; colonneA < 10; colonneA++)
                {
                    tabAdversaire[ligneA, colonneA] = sr.ReadLine(); //idem pour Adversaire
                }
            }

            //récup emplacements joueur
            for (int bateauJ = 0; bateauJ < 5; bateauJ++)
            {
                for (int donneeJ = 0; donneeJ < 4; donneeJ++)
                {
                    sauvegardeEmplacementJoueur[bateauJ, donneeJ] = Convert.ToInt32(sr.ReadLine()); //on récupère et replace les données sur l’emplacement des bateaux (ligne, colonne, orientation, taille) dans sauvegardeEmplacementJoueur
                }
            }

            //récup emplacements adversaire
            for (int bateauA = 0; bateauA < 5; bateauA++)
            {
                for (int donneeA = 0; donneeA < 4; donneeA++)
                {
                    sauvegardeEmplacementAdversaire[bateauA, donneeA] = Convert.ToInt32(sr.ReadLine()); //idem pour Adversaire
                }
            }

            //récup difficulté
            int difficulte = Convert.ToInt32(sr.ReadLine()); //on récupère la difficulté

            sr.Close();

            return difficulte;
        }


    }
}