using System.Threading; // en haut de ton fichier si pas déjà fait

public class Urgence
{
    public void AfficherPageUrgence()
    {
        Jardin jardin = new Jardin();
        JardinCurseur curseur = new JardinCurseur(jardin);

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
    }

    public void Elephant()
    {
        Jardin jardin = new Jardin();
        JardinCurseur curseur = new JardinCurseur(jardin);
        Console.Clear();

        curseur.Afficher();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Un éléphant a réussi à rentrer sur votre propriété");
        Console.WriteLine("Il va écraser certains de vos plants !! Voulez vous lui faire peur et ainsi sauvez vos plants ??");
        Console.WriteLine("(Si oui vous prenez le risque qu'il s'énerve et écrase TOUTES vos plantations !)\n");
        Console.ResetColor();
        Console.Write("Choix (oui/non): ");

        string reponse = Console.ReadLine()?.Trim().ToLower();

        Random rnd = new Random();
        int nombreAleatoire = rnd.Next(1, 4);

        if (reponse == "oui")
        {
            if (nombreAleatoire == 1 || nombreAleatoire == 2)
            {
                Console.Clear();
                curseur.Afficher();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nL'éléphant a pris peur et s'est enfui ! Toutes vos plantes sont sauves 🐘💨");
                Console.ResetColor();

            }
            else
            {
                Console.Clear();
                // Supprimer toutes les plantes
                for (int tx = 0; tx < 2; tx++)
                {
                    for (int ty = 0; ty < 3; ty++)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                jardin.Terrains[tx, ty].Cases[i, j].Plante = null;
                            }
                        }
                    }
                }
                curseur.Afficher();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nL'éléphant s'est énervé ! Il a tout piétiné ");
                Console.ResetColor();
                AnimationElephant(jardin);
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
                            if (jardin.Terrains[tx, ty].Cases[i, j].Plante != null)
                            {
                                plantesExistantes.Add((tx, ty, i, j));
                            }
                        }
                    }
                }
            }
            curseur.Afficher();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\nL'éléphant s'est baladé... certaines plantes ont été écrasées 🐘💥");
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
                Console.WriteLine("💥 L'éléphant fonce à travers votre potager !");

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
                                // Il écrase la plante ici :
                                jardin.Terrains[tx, ty].Cases[row, j].Plante = null;
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
        Console.WriteLine("\n🐘 L'éléphant a terminé sa traversée !");
        Console.ResetColor();
        Console.WriteLine("Appuyez sur une touche pour continuer...");
        Console.ReadKey();
    }

}

