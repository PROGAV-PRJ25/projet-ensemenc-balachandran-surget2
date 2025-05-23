
// Représente une cellule du jardin
public class Case
{
    public Plantes Plante { get; set; } = default!;
    public string Afficher()
    {
        return Plante == null ? "□" : Plante.Croissance;
    }
}


// Énumération des types de terrain
public enum TypeTerrain
{
    Sable,
    Terre,
    Argile
}


// Représente une grille 3x3 de cases
public class Terrain
{
    public TypeTerrain Type { get; }
    public Case[,] Cases { get; }
    public Terrain(TypeTerrain type)
    {
        Type = type;
        Cases = new Case[3, 3];
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                Cases[i, j] = new Case();
    }
}



// Représente une grille 2x3 de terrains
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
        // Mélange aléatoirement pour varier la disposition des terrains
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

    // Permet de faire pousser toutes les plantes
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

    // Retourne un dictionnaire : nom de plante → quantité récoltée
    public Dictionary<string, int> InventaireRecoltes(Inventaire inventaire)
    {
        var recoltes = new Dictionary<string, int>();
        var plantesDejaRecoltees = new HashSet<Plantes>(); // Pour éviter de récolter plusieurs fois la même plante (grandes plantes)

        // Parcourt tous les terrains du jardin
        foreach (var terrain in Terrains)
        {
            int largeur = terrain.Cases.GetLength(1);
            int hauteur = terrain.Cases.GetLength(0);

            for (int y = 0; y < hauteur; y++)
                for (int x = 0; x < largeur; x++)
                {
                    var plante = terrain.Cases[y, x].Plante;
                    // Ignore si : pas de plante, pas mature, ou déjà récoltée
                    if (plante == null || !plante.EstMature || plantesDejaRecoltees.Contains(plante))
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
                        recoltes[key] = qte;

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

    // Supprime les plantes mortes du potager
    public void NettoyerPlantesMortes()
    {
        foreach (var terrain in Terrains)
        {
            int h = terrain.Cases.GetLength(0), w = terrain.Cases.GetLength(1);
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    var c = terrain.Cases[y, x];
                    if (c.Plante != null && c.Plante.Phase == "Morte")
                    {
                        c.Plante = null!;
                    }
                }
            }
        }
    }

}


