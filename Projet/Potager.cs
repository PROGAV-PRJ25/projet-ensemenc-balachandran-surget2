public class Case
{
    public Plantes Plante { get; set; }

    public string Afficher()
    {
        return Plante == null ? "□" : Plante.Croissance;
    }
}

public class Terrain
{
    public Case[,] Cases { get; }

    public Terrain()
    {
        Cases = new Case[3, 3];
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                Cases[i, j] = new Case();
    }
}

public class Jardin
{
    public Terrain[,] Terrains { get; }

    public Jardin()
    {
        Terrains = new Terrain[2, 3];
        for (int i = 0; i < 2; i++)
            for (int j = 0; j < 3; j++)
                Terrains[i, j] = new Terrain();
    }

    public void ToutPousser(int nbJours = 1)
    {
        for (int jour = 0; jour < nbJours; jour++)
        {
            foreach (var terrain in Terrains)
            {
                foreach (var c in terrain.Cases)
                {
                    c.Plante?.Grandir();
                }
            }
        }
    }


    public Dictionary<string, int> InventaireRecolte(Inventaire inventaire)
    {
        var recoltes = new Dictionary<string, int>();

        foreach (var terrain in Terrains)
        {
            foreach (var c in terrain.Cases)
            {
                if (c.Plante != null && c.Plante.Phase == "Mature")
                {
                    string nom = c.Plante.GetType().Name.ToLower(); // ex : "tomate"

                    // Ajout à l'inventaire
                    inventaire.AjouterObjet(nom, 1);

                    // Comptage pour affichage
                    if (recoltes.ContainsKey(nom))
                        recoltes[nom]++;
                    else
                        recoltes[nom] = 1;

                    // Suppression de la plante après récolte
                    c.Plante = null;
                }
            }
        }

        return recoltes;
    }



}

public class JardinCurseur
{
    public Jardin jardin;
    public int terrainX;
    public int terrainY;
    public int caseX;
    public int caseY;

    public JardinCurseur(Jardin jardin)
    {
        this.jardin = jardin;
        terrainX = 0;
        terrainY = 0;
        caseX = 0;
        caseY = 0;
    }

    public void Deplacer()
    {
        bool enDeplacement = true; // Variable pour quitter la boucle
        while (enDeplacement)
        {
            Console.Clear();
            Console.WriteLine("Choisissez une case avec les flèches et appuyez sur Entrée.");
            Afficher();
            var keyInfo = Console.ReadKey(true);
            var key = keyInfo.Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    if (caseY == 0)
                    {
                        terrainX = (terrainX + 1) % 2; // terrain précédent verticalement
                        caseY = 2;
                    }
                    else
                    {
                        caseY--;
                    }
                    break;

                case ConsoleKey.DownArrow:
                    if (caseY == 2)
                    {
                        terrainX = (terrainX + 1) % 2; // terrain suivant verticalement
                        caseY = 0;
                    }
                    else
                    {
                        caseY++;
                    }
                    break;

                case ConsoleKey.LeftArrow:
                    if (caseX == 0)
                    {
                        terrainY = (terrainY + 2) % 3; // terrain précédent horizontalement
                        caseX = 2;
                    }
                    else
                    {
                        caseX--;
                    }
                    break;

                case ConsoleKey.RightArrow:
                    if (caseX == 2)
                    {
                        terrainY = (terrainY + 1) % 3; // terrain suivant horizontalement
                        caseX = 0;
                    }
                    else
                    {
                        caseX++;
                    }
                    break;

                case ConsoleKey.Enter: // Quitter le mode déplacement
                    enDeplacement = false;
                    break;
            }
        }
    }

    public void Afficher()
    {
        int nbTerrainsX = jardin.Terrains.GetLength(0);
        int nbTerrainsY = jardin.Terrains.GetLength(1);

        for (int tx = 0; tx < nbTerrainsX; tx++)
        {
            for (int row = 0; row < 3; row++)
            {
                for (int ty = 0; ty < nbTerrainsY; ty++)
                {
                    for (int col = 0; col < 3; col++)
                    {
                        bool estCurseur = (tx == terrainX && ty == terrainY && row == caseY && col == caseX);
                        var caseActuelle = jardin.Terrains[tx, ty].Cases[row, col];
                        string symbole = caseActuelle.Plante?.Croissance ?? "□";

                        if (estCurseur)
                            Console.BackgroundColor = ConsoleColor.DarkGray;

                        if (caseActuelle.Plante != null)
                            Console.ForegroundColor = caseActuelle.Plante.Couleur;

                        Console.Write(FormatCellule(symbole));
                        Console.ResetColor();
                    }

                    Console.Write("   "); // Espace entre terrains
                }

                Console.WriteLine();
            }

            Console.WriteLine(); // Espace entre rangées de terrains
        }
    }

    private string FormatCellule(string contenu)
    {
        if (contenu.Length == 1)
            return $" {contenu}  ";
        else if (contenu.Length == 2)
            return $" {contenu} ";
        else
            return contenu.PadRight(4); // Sécurité pour les croissances personnalisées
    }

    public Case ObtenirCase()
    {
        return jardin.Terrains[terrainX, terrainY].Cases[caseY, caseX];
    }

    public void Planter(Plantes plante)
    {
        ObtenirCase().Plante = plante;
    }
}

