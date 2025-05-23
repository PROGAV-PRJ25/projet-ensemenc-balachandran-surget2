public class Boutique
{
    // Méthode pour afficher la boutique
    public void Afficher(Joueur joueur)
    {
        bool quitter = false;
        while (!quitter)
        {
            Console.Clear();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("║          🛒  BOUTIQUE DU JEU       ║");
            Console.WriteLine("╚════════════════════════════════════╝");
            Console.ResetColor();

            // Afficher l'argent du joueur
            Console.WriteLine($"💰 Votre argent : {joueur.Argent} pièces\n");

            // Menu d'actions
            Console.WriteLine("Que souhaitez-vous faire ?");
            Console.WriteLine("1️⃣ Acheter");
            Console.WriteLine("2️⃣ Vendre");
            Console.WriteLine("3️⃣ ↩ Revenir au jeu");

            Console.Write("\nVotre choix : ");
            var choix = Console.ReadKey().Key;
            Console.WriteLine();

            switch (choix)
            {
                case ConsoleKey.D1:
                    Acheter(joueur);
                    break;
                case ConsoleKey.D2:
                    Vendre(joueur);
                    break;
                case ConsoleKey.D3:
                    quitter = true;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Choix invalide.");
                    Console.ResetColor();
                    Console.ReadKey();
                    break;
            }
        }
    }

    // Méthode pour acheter dans la boutique
    public void Acheter(Joueur joueur)
    {
        // Ajout des différents éléments dans le catalogue
        var catalogue = new Dictionary<string, int>
        {
            { "graine de tomate", 5 },
            { "graine de aubergine", 6 },
            { "graine de mangue", 10 },
            { "graine de hibiscus", 7 },
            { "graine de thé", 8 }
        };

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔════════════════════════════════╗");
        Console.WriteLine("║        🛍️  ACHAT DE GRAINES    ║");
        Console.WriteLine("╚════════════════════════════════╝");
        Console.ResetColor();

        Console.WriteLine($"💰 Votre argent : {joueur.Argent} pièces\n");

        int i = 1;
        // Affichage des éléments du catalogue
        foreach (var item in catalogue)
        {
            Console.WriteLine($"{i++}. {item.Key} - {item.Value} pièces");
        }
        Console.WriteLine($"{i}. Retour");


        Console.Write("\nChoisissez un article (numéro) : ");
        if (int.TryParse(Console.ReadLine(), out int choix) && choix > 0 && choix < i)
        {
            var selected = catalogue.ElementAt(choix - 1);
            if (joueur.DepenserArgent(selected.Value))
            {
                joueur.Inventaire.AjouterObjet(selected.Key, 1);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($" Vous avez acheté 1 {selected.Key} !");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" Pas assez d'argent !");
            }
        }
        else
        {
            Console.WriteLine("\n↩ Retour à la boutique...");
        }

        Console.ResetColor();
        Console.WriteLine("\nAppuyez sur une touche pour continuer...");
        Console.ReadKey();
    }

    // Méthode pour que le joueur puisse vendre ses récoltes
    public void Vendre(Joueur joueur)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔═════════════════════════════════╗");
        Console.WriteLine("║       💼  VENTE DE RÉCOLTES     ║");
        Console.WriteLine("╚═════════════════════════════════╝");
        Console.ResetColor();

        var vendables = joueur.Inventaire.Objets
            .Where(o => !o.Nom.Contains("graine")) // Le joueur ne peut pas vendre de graines
            .ToList();

        if (vendables.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Vous n'avez rien à vendre.");
            Console.ResetColor();
            Console.ReadKey();
            return;
        }

        // Affiche les éléments que le joueur peut vendre
        for (int i = 0; i < vendables.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {vendables[i].Nom} x{vendables[i].Quantite}");
        }
        Console.WriteLine($"{vendables.Count + 1}. Retour");


        Console.Write("\nChoisissez un article à vendre (numéro) : ");
        if (int.TryParse(Console.ReadLine(), out int choix) && choix > 0 && choix <= vendables.Count)
        {
            var item = vendables[choix - 1];
            // Définition des prix des différentes plantes 
            int prix = item.Nom switch
            {
                "tomate" => 4,
                "aubergine" => 2,
                "mangue" => 1,
                "hibiscus" => 5,
                "thé" => 7,
                _ => 2 // par défaut
            };

            // Choix de la quantité
            Console.Write($"📦 Combien de {item.Nom} souhaitez-vous vendre ? (Max {item.Quantite}) : ");

            if (int.TryParse(Console.ReadLine(), out int quantiteAVendre) &&
                quantiteAVendre > 0 &&
                quantiteAVendre <= item.Quantite)
            {
                for (int i = 0; i < quantiteAVendre; i++)
                {
                    joueur.Inventaire.UtiliserObjet(item.Nom);
                }

                int total = prix * quantiteAVendre;
                joueur.AjouterArgent(total);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nVous avez vendu {quantiteAVendre} {item.Nom}(s) pour {total} pièces.");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Quantité invalide.");
            }
        }
        else
        {
            Console.WriteLine("\n↩ Retour à la boutique...");
        }

        Console.ResetColor();
        Console.WriteLine("\nAppuyez sur une touche pour continuer...");
        Console.ReadKey();
    }
}