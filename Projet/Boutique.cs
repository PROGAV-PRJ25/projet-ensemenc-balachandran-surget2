public class Boutique
{
    public void Afficher(Joueur joueur)
    {
        bool quitter = false;

        while (!quitter)
        {
            Console.Clear();
            Console.WriteLine("🛒 Bienvenue à la boutique !");
            Console.WriteLine($"💰 Votre argent : {joueur.Argent} pièces\n");

            Console.WriteLine("1. Acheter");
            Console.WriteLine("2. Vendre");
            Console.WriteLine("3. Revenir au jeu");

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
                    Console.WriteLine("Choix invalide.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    public void Acheter(Joueur joueur)
    {
        var catalogue = new Dictionary<string, int>
        {
            { "graine de tomate", 5 },
            { "graine de aubergine", 6 },
            { "graine de mangue", 10 },
            { "graine de hibiscus", 7 },
            { "graine de thé", 8 }
        };

        Console.Clear();
        Console.WriteLine("🛍️ Acheter des graines :\n");
        Console.WriteLine($"💰 Votre argent : {joueur.Argent} pièces\n");

        int i = 1;
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

        Console.ResetColor();
        Console.WriteLine("\nAppuyez sur une touche pour continuer...");
        Console.ReadKey();
    }

    public void Vendre(Joueur joueur)
    {
        Console.Clear();
        Console.WriteLine("💼 Vendre des objets de votre inventaire :\n");

        var vendables = joueur.Inventaire.Objets
            .Where(o => !o.Nom.Contains("graine"))
            .ToList();

        if (vendables.Count == 0)
        {
            Console.WriteLine("Vous n'avez rien à vendre.");
            Console.ReadKey();
            return;
        }

        for (int i = 0; i < vendables.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {vendables[i].Nom} x{vendables[i].Quantite}");
        }
        Console.WriteLine($"{vendables.Count + 1}. Retour");

        Console.Write("\nChoisissez un article à vendre (numéro) : ");
        if (int.TryParse(Console.ReadLine(), out int choix) && choix > 0 && choix <= vendables.Count)
        {
            var item = vendables[choix - 1];
            int prix = 4;
            if (joueur.Inventaire.UtiliserObjet(item.Nom))
            {
                joueur.AjouterArgent(prix);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✅ Vous avez vendu 1 {item.Nom} pour {prix} pièces.");
            }
        }
        Console.ResetColor();
        Console.WriteLine("\nAppuyez sur une touche pour continuer...");
        Console.ReadKey();
    }
}