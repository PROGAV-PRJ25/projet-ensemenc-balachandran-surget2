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

    // Affichage du texte en clignotant
    public void AfficherPageUrgence()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;
        for (int i = 1; i < 6; i++) // Afficher 5 fois
        {
            Console.WriteLine(TextHelper.CenterText(@",__  __  ___  ____  _____   _   _ ____   ____ _____ _   _  ____ _____   _ "));
            Console.WriteLine(TextHelper.CenterText(@"|  \/  |/ _ \|  _ \| ____| | | | |  _ \ / ___| ____| \ | |/ ___| ____| | |"));
            Console.WriteLine(TextHelper.CenterText(@"| |\/| | | | | | | |  _|   | | | | |_) | |  _|  _| |  \| | |   |  _|   | |"));
            Console.WriteLine(TextHelper.CenterText(@"| |  | | |_| | |_| | |___  | |_| |  _ <| |_| | |___| |\  | |___| |___  |_|"));
            Console.WriteLine(TextHelper.CenterText(@"|_|  |_|\___/|____/|_____|  \___/|_| \_\\____|_____|_| \_|\____|_____| (_)"));

            Thread.Sleep(300); // Pause de 300ms pour voir le texte
            Console.Clear(); // Efface le texte
            Thread.Sleep(300); // Pause de 300ms avant de le réafficher

        }
        Console.Clear();
        Console.ResetColor();

        Console.WriteLine("Appuyez sur une touche pour gérer la situation !");
        Console.ReadKey();
    }

    // URGENCE : Invasion d'éléphants
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
            if (nombreAleatoire == 1) // 1 chance sur 3 que les éléphants prennent peur et ne fassent pas de dégâts
            {
                Console.Clear();
                _curseur.Afficher();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nLes éléphants ont pris peur et se sont enfuis ! Toutes vos plantes sont sauves 🐘💨");
                Console.ResetColor();

            }
            else // ils écrasent tout sur leur passage
            {
                Console.Clear();
                AnimationElephant(_jardin);
                ClearAllPlants();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n🐘💥 Les éléphant se sont énervés ! Ils ont tout piétiné ");
                Console.ResetColor();

            }

        }
        else if (reponse == "non")
        {
            Console.Clear();
            // Supprimer aléatoirement quelques plantes (par ex. 25 à 50 % des plantes existantes)
            var plantesMap = new Dictionary<Plantes, List<(int tx, int ty, int i, int j)>>();
            for (int tx = 0; tx < 2; tx++)
                for (int ty = 0; ty < 3; ty++)
                    for (int i = 0; i < 3; i++)
                        for (int j = 0; j < 3; j++)
                        {
                            var plante = _jardin.Terrains[tx, ty].Cases[i, j].Plante;
                            if (plante != null)
                            {
                                if (!plantesMap.TryGetValue(plante, out var list))
                                {
                                    list = new List<(int, int, int, int)>();
                                    plantesMap[plante] = list;
                                }
                                list.Add((tx, ty, i, j));
                            }
                        }

            var toutesPlantes = plantesMap.Keys.ToList();
            int totalInstances = toutesPlantes.Count;
            int aSupprimer = rnd.Next(totalInstances / 4, totalInstances / 2 + 1);

            var plantesPiétinées = toutesPlantes.OrderBy(_ => rnd.Next()).Take(aSupprimer).ToList();

            foreach (var plante in plantesPiétinées)
            {
                foreach (var (tx, ty, i, j) in plantesMap[plante])
                {
                    _jardin.Terrains[tx, ty].Cases[i, j].Plante = null!;
                }
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


    // Méthode qui gère l'animation des éléphants sur la console
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
                                            jardin.Terrains[tx, ty].Cases[cy, cx].Plante = null!;
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
    
    // Méthode qui supprime toutes les plantes
    private void ClearAllPlants()
    {
        for (int tx = 0; tx < _jardin.Terrains.GetLength(0); tx++)
        {
            for (int ty = 0; ty < _jardin.Terrains.GetLength(1); ty++)
            {
                var terrain = _jardin.Terrains[tx, ty];
                for (int i = 0; i < terrain.Cases.GetLength(0); i++)
                {
                    for (int j = 0; j < terrain.Cases.GetLength(1); j++)
                    {
                        terrain.Cases[i, j].Plante = null!;
                    }
                }
            }
        }
    }


}

