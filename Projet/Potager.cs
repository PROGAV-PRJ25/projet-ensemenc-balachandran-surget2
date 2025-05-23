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
    private Meteo meteo;

    public Jardin(Meteo meteo)
    {
        this.meteo = meteo;
        Terrains = new Terrain[2, 3];
        for (int i = 0; i < 2; i++)
            for (int j = 0; j < 3; j++)
                Terrains[i, j] = new Terrain();
    }

    public void ToutPousser(int jours = 1)
    {
            foreach (var terrain in Terrains)
            {
                foreach (var c in terrain.Cases)
                {
                    if (c.Plante != null)
                    {
                        c.Plante.Grandir(meteo, jours);
                    }
                }
            }
    }

    public List<Plantes> ObtenirToutesLesPlantes()
    {
        var plantes = new List<Plantes>();

        foreach (var terrain in Terrains)
        {
            foreach (var c in terrain.Cases)
            {
                if (c.Plante != null && !plantes.Contains(c.Plante))
                {
                    plantes.Add(c.Plante);
                }
            }
        }

        return plantes;
    }


    public Dictionary<string, int> InventaireRecoltes(Inventaire inventaire)
    {
        var recoltes = new Dictionary<string, int>();
        var plantesDejaRecoltees = new HashSet<Plantes>();

        foreach (var terrain in Terrains)
        {
            int tailleX = terrain.Cases.GetLength(1);
            int tailleY = terrain.Cases.GetLength(0);

            for (int y = 0; y < tailleY; y++)
            {
                for (int x = 0; x < tailleX; x++)
                {
                    var plante = terrain.Cases[y, x].Plante;

                    if (plante != null && plante.Phase == "Mature" && !plantesDejaRecoltees.Contains(plante))
                    {
                        // Vérifie si (x, y) est l'origine de la plante
                        bool estOrigine = true;
                        foreach (var (dx, dy) in plante.Occupation)
                        {
                            int cx = x + dx;
                            int cy = y + dy;

                            if (cx < 0 || cx >= tailleX || cy < 0 || cy >= tailleY)
                            {
                                estOrigine = false;
                                break;
                            }

                            if (terrain.Cases[cy, cx].Plante != plante)
                            {
                                estOrigine = false;
                                break;
                            }
                        }

                        if (!estOrigine) continue;

                        // Récolte
                        string nom = plante.GetType().Name.ToLower();
                        int quantite = plante.Occupation.Count;

                        inventaire.AjouterObjet(nom, quantite);

                        if (recoltes.ContainsKey(nom))
                            recoltes[nom] += quantite;
                        else
                            recoltes[nom] = quantite;

                        plantesDejaRecoltees.Add(plante);

                        // Supprimer la plante de toutes ses cases
                        foreach (var (dx, dy) in plante.Occupation)
                        {
                            int cx = x + dx;
                            int cy = y + dy;

                            terrain.Cases[cy, cx].Plante = null;
                        }
                    }
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

    public void VoirCaracteristiquesEnDeplacement()
    {
        BoucleDeplacement(() =>
        {
            Console.Clear();
            Afficher();

            var plante = ObtenirPlante();

            Console.WriteLine();
            if (plante == null)
            {
                Console.WriteLine("Pas de plante ici.");
            }
            else
            {
                Console.WriteLine($"Plante : {plante.GetType().Name}");
                Console.WriteLine($"Phase : {plante.Phase}");
                // Niveau d'hydratation
                Console.WriteLine($"Hydratation : {plante.NiveauHydratation} / {plante.EauHebdomadaire}");

                // Indiquer si elle a besoin de plus ou moins d'eau
                if (plante.NiveauHydratation < plante.EauHebdomadaire)
                    Console.WriteLine("Besoin d'eau : Plus d'eau nécessaire");
                else if (plante.NiveauHydratation > plante.EauHebdomadaire * 2)
                    Console.WriteLine("Besoin d'eau : Trop d'eau, réduire l'arrosage");
                else
                    Console.WriteLine("Besoin d'eau : Hydratation correcte");
            }

            Console.WriteLine();
            Console.WriteLine("Utilisez les flèches pour déplacer le curseur, Échap pour quitter.");
        },
        ConsoleKey.Escape);
    }

    public void Deplacer(bool instructions = false,  string nomPlante = "")
    {
        BoucleDeplacement(() =>
        {
            Console.Clear();
            Console.WriteLine("Choisissez une case avec les flèches et appuyez sur Entrée.\n");
            if (instructions == true)
            {
                var phrases = new Dictionary<string, string>
                {
                    { "Mangue", "La mangue a besoin de 9 cases autour d'elle pour grandir." },
                    { "Aubergine", "L'aubergine occupe un carré de 2x2 cases." },
                    { "Hibiscus", "L'hibiscus s'étale sur deux cases horizontalement." },
                    { "Thé", "Le thé prend une rangée de 3 cases." },
                    { "Tomate", "La tomate ne prend qu'une seule case." }
                };
                if (phrases.ContainsKey(nomPlante))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"ℹ️  {phrases[nomPlante]}\n");
                    Console.ResetColor();
                }
            }
            Afficher();
        },
        ConsoleKey.Enter);
    }

    private void BoucleDeplacement(Action afficher, ConsoleKey toucheQuitter)
    {
        bool enDeplacement = true;

        while (enDeplacement)
        {
            afficher();

            var keyInfo = Console.ReadKey(true);

            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    if (caseY == 0)
                    {
                        terrainX = (terrainX + 1) % 2;
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
                        terrainX = (terrainX + 1) % 2;
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
                        terrainY = (terrainY + 2) % 3;
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
                        terrainY = (terrainY + 1) % 3;
                        caseX = 0;
                    }
                    else
                    {
                        caseX++;
                    }
                    break;

                case var key when key == toucheQuitter:
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

    public Plantes? ObtenirPlante()
    {
        Case caseActuelle = this.ObtenirCase();
        return caseActuelle.Plante;
    }


    public void Planter(Plantes plante)
    {
        if (!PeutPlanter(plante))
        {
            Console.WriteLine("Impossible de planter ici : les cases sont occupées ou hors limites.");
            Console.ReadKey();
            return;
        }

        foreach (var (dx, dy) in plante.Occupation)
        {
            int cx = caseX + dx;
            int cy = caseY + dy;
            jardin.Terrains[terrainX, terrainY].Cases[cy, cx].Plante = plante;
        }
    }

    public bool PeutPlanter(Plantes plante)
    {
        var terrain = jardin.Terrains[terrainX, terrainY];

        foreach (var (dx, dy) in plante.Occupation)
        {
            int x = caseX + dx;
            int y = caseY + dy;

            // Vérifie que la case est dans les limites
            if (x < 0 || x >= 3 || y < 0 || y >= 3) return false;

            if (terrain.Cases[y, x].Plante != null) return false;
        }

        return true;
    }

}

