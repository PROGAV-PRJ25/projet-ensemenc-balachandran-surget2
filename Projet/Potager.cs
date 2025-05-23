public class Case
{
    public Plantes Plante { get; set; } = default!;

    public string Afficher()
    {
        return Plante == null ? "□" : Plante.Croissance;
    }
}
public enum TypeTerrain
{
    Sable,
    Terre,
    Argile
}

public class Terrain
{
    public TypeTerrain Type { get; }
    public Case[,] Cases { get; }

    public Terrain(TypeTerrain type)
    {
        Type  = type;
        Cases = new Case[3, 3];
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                Cases[i, j] = new Case();
    }
}


public class Jardin
{
    public Terrain[,] Terrains { get; }
    public Meteo meteo;

    public Jardin(Meteo meteo)
    {
        this.meteo = meteo;
        // Prépare la liste 2× de chaque type
        var types = new List<TypeTerrain>
        {
            TypeTerrain.Sable, TypeTerrain.Sable,
            TypeTerrain.Terre, TypeTerrain.Terre,
            TypeTerrain.Argile, TypeTerrain.Argile
        };
        // Mélange aléatoirement pour varier la disposition
        var rnd = new Random();
        types = types.OrderBy(x => rnd.Next()).ToList();

        Terrains = new Terrain[2, 3];
        int idx = 0;
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Terrains[i, j] = new Terrain(types[idx++]);
            }
        }
    }

    public void ToutPousser(Meteo meteo, string saison, int jours = 1)
    {
            foreach (var terrain in Terrains)
            {
                foreach (var c in terrain.Cases)
                {
                    if (c.Plante != null)
                    {
                        c.Plante.Grandir(meteo, saison, terrain.Type, jours);
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
            int largeur = terrain.Cases.GetLength(1);
            int hauteur = terrain.Cases.GetLength(0);

            for (int y = 0; y < hauteur; y++)
            for (int x = 0; x < largeur; x++)
            {
                var plante = terrain.Cases[y, x].Plante;
                if (plante == null  || !plante.EstMature || plantesDejaRecoltees.Contains(plante))
                    continue;

                // On ne récolte qu'à partir de l'origine de la plante
                bool estOrigine = true;
                foreach (var (dx, dy) in plante.Occupation)
                {
                    int cx = x + dx, cy = y + dy;
                    if (cx < 0 || cx >= largeur 
                    || cy < 0 || cy >= hauteur
                    || terrain.Cases[cy, cx].Plante != plante)
                    {
                        estOrigine = false;
                        break;
                    }
                }
                if (!estOrigine) 
                    continue;

                // -- Récolte --
                string key = plante.GetType().Name.ToLower();
                int qte = plante.Occupation.Count;

                inventaire.AjouterObjet(key, qte);
                if (recoltes.ContainsKey(key)) 
                    recoltes[key] += qte;
                else 
                    recoltes[key]  = qte;

                plantesDejaRecoltees.Add(plante);

                // -- Gestion de la repousse / disparition --
                bool repousse = plante.Recolter();
                if (!repousse)
                {
                    // Supprimer définitivement: on efface toutes les cases occupées
                    foreach (var (dx, dy) in plante.Occupation)
                    {
                        int cx = x + dx, cy = y + dy;
                        terrain.Cases[cy, cx].Plante = null!;
                    }
                }
                // sinon la plante reste, dans sa nouvelle phase « En croissance »
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

    public void AfficherInfosSousCurseur()
        {
            var plante = ObtenirPlante();
            Console.WriteLine(); // ligne vide pour séparer du potager

            if (plante == null)
            {
                Console.WriteLine("Aucune plante ici. Déplacez le curseur pour en voir les infos.");
                return;
            }

            // Nom et phase
            Console.WriteLine($"📋 Plante : {plante.Nom} — Phase : {plante.Phase}");

            // Hydratation
            Console.Write($"    Hydratation : {plante.NiveauHydratation} / {plante.EauHebdomadaire}  ");
            if (plante.NiveauHydratation < plante.EauHebdomadaire)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("➤ Plus d'eau nécessaire");
            }
            else if (plante.NiveauHydratation > plante.EauHebdomadaire * 2)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("➤ Trop d'eau, attention !");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("➤ Hydratation correcte");
            }
            Console.ResetColor();

            // Température idéale vs actuelle
            int tMin = plante.TemperaturePreferee.min;
            int tMax = plante.TemperaturePreferee.max;
            Console.WriteLine($"    Température idéale : {tMin}°C – {tMax}°C");

            int tempAct = jardin.meteo.Temperature;
            Console.Write($"    Température actuelle : {tempAct}°C  ");
            if (tempAct < tMin)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("➤ Trop froid !");
            }
            else if (tempAct > tMax)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("➤ Trop chaud !");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("➤ Parfait");
            }
            Console.ResetColor();
        }

    public void Deplacer(bool instructions = false,  Plantes? plante = null)
    {
        BoucleDeplacement(() =>
        {
            Console.Clear();
            Console.WriteLine("Choisissez une case avec les flèches et appuyez sur Entrée.\n");
            if (instructions && plante != null)
            {
                string message = plante.Nom switch
                {
                    "Mangue" => $"Un plant de mangue occupe 9 cases. Les mangues poussent mieux sur {plante.TerrainPrefere}.",
                    "Aubergine" => $"Un plant d'aubergine occupe un carré de 2x2 cases. Les aubergines poussent mieux sur {plante.TerrainPrefere}",
                    "Hibiscus" => $"L'hibiscus s'étale sur 2 cases horizontalement. L'hibiscus pousse mieux sur {plante.TerrainPrefere}",
                    "Thé" => $"Un plant de thé prend une rangée de 3 cases. Le thé pousse mieux sur {plante.TerrainPrefere}",
                    "Tomate" => $"La tomate ne prend qu'une seule case. La tomate pousse mieux sur {plante.TerrainPrefere}",
                    _ => ""
                };

                if (!string.IsNullOrWhiteSpace(message))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"ℹ️  {message}\n");
                    Console.ResetColor();
                }
            }
            Afficher();
            AfficherInfosSousCurseur();
            Console.WriteLine("\n(Entrée pour confirmer.)");
        },
        ConsoleKey.Enter);
    }

        public void DeplacerUneFois(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.UpArrow:
                if (caseY == 0) { terrainX = (terrainX + 1) % 2; caseY = 2; }
                else caseY--;
                break;
            case ConsoleKey.DownArrow:
                if (caseY == 2) { terrainX = (terrainX + 1) % 2; caseY = 0; }
                else caseY++;
                break;
            case ConsoleKey.LeftArrow:
                if (caseX == 0) { terrainY = (terrainY + 2) % 3; caseX = 2; }
                else caseX--;
                break;
            case ConsoleKey.RightArrow:
                if (caseX == 2) { terrainY = (terrainY + 1) % 3; caseX = 0; }
                else caseX++;
                break;
        }
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
                    var terrain = jardin.Terrains[tx, ty];
                    ConsoleColor bg = terrain.Type switch
                {
                    TypeTerrain.Sable  => ConsoleColor.DarkYellow,
                    TypeTerrain.Terre  => ConsoleColor.DarkGreen,
                    TypeTerrain.Argile => ConsoleColor.DarkGray,
                    _                   => ConsoleColor.Black
                };
                    for (int col = 0; col < 3; col++)
                    {
                        bool estCurseur = tx == terrainX && ty == terrainY && row == caseY && col == caseX;
                        var caseActuelle = jardin.Terrains[tx, ty].Cases[row, col];
                        string symbole = caseActuelle.Plante?.Croissance ?? "□";

                        if (estCurseur)
                            Console.BackgroundColor = ConsoleColor.Gray;
                        else
                            Console.BackgroundColor = bg;

                        if (caseActuelle.Plante != null)
                            Console.ForegroundColor = caseActuelle.Plante.Couleur;
                        else
                            Console.ForegroundColor = ConsoleColor.Black;

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

