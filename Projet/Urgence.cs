using System.Threading;

public class Urgence
{
    private Jardin _jardin;
    private JardinCurseur _curseur;

    public Urgence(Jardin jardin, JardinCurseur curseur)
    {
        _jardin = jardin;
        _curseur = curseur;
    }


    public void AfficherPageUrgence()
    {
        // Mode URGENCE qui clignote
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;
        for (int i = 1; i < 6; i++)
        {
            Console.WriteLine(TextHelper.CenterText(@",__  __  ___  ____  _____   _   _ ____   ____ _____ _   _  ____ _____   _ "));
            Console.WriteLine(TextHelper.CenterText(@"|  \/  |/ _ \|  _ \| ____| | | | |  _ \ / ___| ____| \ | |/ ___| ____| | |"));
            Console.WriteLine(TextHelper.CenterText(@"| |\/| | | | | | | |  _|   | | | | |_) | |  _|  _| |  \| | |   |  _|   | |"));
            Console.WriteLine(TextHelper.CenterText(@"| |  | | |_| | |_| | |___  | |_| |  _ <| |_| | |___| |\  | |___| |___  |_|"));
            Console.WriteLine(TextHelper.CenterText(@"|_|  |_|\___/|____/|_____|  \___/|_| \_\\____|_____|_| \_|\____|_____| (_)"));

            // Attendre 5 secondes avant de nettoyer l'écran
            Thread.Sleep(300); // Pause de 300ms pour voir le texte
            Console.Clear(); // Efface le texte
            Thread.Sleep(300); // Pause de 300ms avant de le réafficher

        }
        Console.Clear(); // Efface le texte
        Console.ResetColor();

        Console.WriteLine("Appuyez sur une touche pour gérer la situation !");
        Console.ReadKey();
    }

    public void Elephant()
    {
        Console.Clear();

        _curseur.Afficher();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Des éléphants ont réussi à entrer sur votre propriété");
        Console.WriteLine("Ils vont écraser certains de vos plants !! Voulez vous lui faire peur et ainsi sauvez vos plants ??");
        Console.WriteLine("(Si oui vous prenez le risque qu'ils s'énervent et écrasent TOUTES vos plantations !)\n");
        Console.ResetColor();
        Console.Write("Choix (oui/non): ");

        string reponse = Console.ReadLine()!.Trim().ToLower();

        Random rnd = new Random();
        int nombreAleatoire = rnd.Next(1, 3);

        if (reponse == "oui")
        {
            if (nombreAleatoire == 1)
            {
                Console.Clear();
                _curseur.Afficher();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nLes éléphants ont pris peur et se sont enfuis ! Toutes vos plantes sont sauves 🐘💨");
                Console.ResetColor();

            }
            else
            {
                Console.Clear();
                AnimationElephant(_jardin);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nLes éléphant se sont énervés ! Ils ont tout piétiné ");
                Console.ResetColor();

            }

        }
        else if (reponse == "non")
        {
            Console.Clear();
            // Supprimer aléatoirement quelques plantes (par ex. 25 à 50 % des plantes existantes)
            List<(int tx, int ty, int i, int j)> plantesExistantes = new();

            for (int tx = 0; tx < 2; tx++)
            {
                for (int ty = 0; ty < 3; ty++)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (_jardin.Terrains[tx, ty].Cases[i, j].Plante != null)
                            {
                                plantesExistantes.Add((tx, ty, i, j));
                            }
                        }
                    }
                }
            }
            int plantesASupprimer = rnd.Next(plantesExistantes.Count / 4, plantesExistantes.Count / 2 + 1);
            for (int k = 0; k < plantesASupprimer; k++)
            {
                var (tx, ty, i, j) = plantesExistantes[k];
                _jardin.Terrains[tx, ty].Cases[i, j].Plante = null;
            }

            _curseur.Afficher();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\nLes éléphants se sont baladés... certaines plantes ont été écrasées 🐘💥");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Réponse invalide. Veuillez taper 'oui' ou 'non'.");
            Console.ResetColor();
        }

    }



    public void AnimationElephant(Jardin jardin)
    {
        int largeur = 3; // Colonnes
        int hauteur = 3; // Lignes
        int terrainsX = 2;
        int terrainsY = 3;

        for (int tx = 0; tx < terrainsX; tx++)
        {
            for (int col = 0; col < largeur; col++)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(" Les éléphants foncent à travers votre potager !");
                Console.ResetColor();

                for (int row = 0; row < hauteur; row++)
                {
                    for (int ty = 0; ty < terrainsY; ty++)
                    {
                        for (int j = 0; j < largeur; j++)
                        {
                            if (ty == col && tx < terrainsX && row < hauteur && j == col)
                            {
                                Console.ForegroundColor = ConsoleColor.Gray;
                                Console.Write("🐘 ");
                                Console.ResetColor();

                                // Récupérer la plante présente à cet emplacement
                                var plante = jardin.Terrains[tx, ty].Cases[row, j].Plante;

                                // Supprimer toutes les cases occupées par cette plante
                                if (plante != null)
                                {
                                    foreach (var (dx, dy) in plante.Occupation)
                                    {
                                        int cx = j + dx;
                                        int cy = row + dy;

                                        if (cx >= 0 && cx < 3 && cy >= 0 && cy < 3)
                                        {
                                            jardin.Terrains[tx, ty].Cases[cy, cx].Plante = null;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var plante = jardin.Terrains[tx, ty].Cases[row, j].Plante;
                                if (plante == null)
                                    Console.Write("□ ");
                                else
                                {
                                    Console.ForegroundColor = plante.Couleur;
                                    Console.Write(plante.Croissance + " ");
                                    Console.ResetColor();
                                }
                            }
                        }
                        Console.Write("   ");
                    }
                    Console.WriteLine();
                }

                Thread.Sleep(400); // Pause pour l'effet d'animation
            }
        }

        Console.ForegroundColor = ConsoleColor.DarkGray;
    }

}

