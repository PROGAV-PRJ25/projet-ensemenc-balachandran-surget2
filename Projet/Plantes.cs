public abstract class Plantes
{
    public string Nom { get; set; } = default!;
    public bool Vivacite { get; set; }
    public int CyclesRestants { get; private set; }
    public bool Comestible { get; set; }
    public string[] SaisonsDeSemis { get; set; } = default!;
    public string TerrainPrefere { get; set; } = default!;
    public abstract List<(int dx, int dy)> Occupation { get; }
    public int NiveauHydratation { get; set; }
    public int JoursPourMaturité { get; set; }
    public int EauHebdomadaire { get; set; }

    public (int min, int max) TemperaturePreferee { get; set; }
    public int EsperanceDeVie { get; set; }
    public ConsoleColor Couleur { get; set; }

    // Attributs de croissance
    public string Phase { get; set; } = "Graine";
    public int NombrePhases { get; set; } // nombre total de phases
    public int JoursDepuisSemis { get; set; } = 0;

    // Pour savoir si la plante est considérée comme arrosée cette période (par exemple si hydratation >= 50)
    public bool EstArrosee => NiveauHydratation >= EauHebdomadaire;

    // Pour savoir si la plante est mature
    public bool EstMature => Phase == "Mature";

    protected void InitCycles()
    {
        if (JoursPourMaturité > 0)
        CyclesRestants = Math.Max(1, EsperanceDeVie / JoursPourMaturité);
        else
        CyclesRestants = 1;
    }

    protected Plantes()
    {
        JoursDepuisSemis = 0;
        Phase = "Graine";
        NiveauHydratation = 0;
    }

    public virtual string Croissance
    {
        get
        {
            return Phase switch
            {
                "Graine" => ".",
                "Jeune pousse" => "🌱",
                "En croissance" => "🎋",
                "Mature" => "🌿",
                "Morte" => "x",
                _ => "?"
            };
        }
    }

    // Croissance gérée ici pour toutes les plantes
public void Grandir(Meteo meteo, string saison, TypeTerrain terrainCourant, int jours)
{
    if (Phase == "Morte") 
    {
        return;
    }

    // 1) Température extrême → pas de croissance
    if (meteo.Temperature < TemperaturePreferee.min - 5 || meteo.Temperature > TemperaturePreferee.max + 5)
    {
        return;
    }
        
    // 2) Hydratation hors-limits → pas de croissance
    if (NiveauHydratation < EauHebdomadaire || NiveauHydratation > EauHebdomadaire * 2)
    {
        return;
    }

    //  saison
        float facteurSaison = SaisonsDeSemis
        .Contains(saison, StringComparer.OrdinalIgnoreCase)
        ? 1.2f    // +20 % si bon moment de semis
        : 0.8f;   // –20 % sinon

    // terrain
    float facteurTerrain = string.Equals(
            TerrainPrefere, 
            terrainCourant.ToString(), 
            StringComparison.OrdinalIgnoreCase)
        ? 1.2f    // +20 % sur sol préféré
        : 0.9f;   // –10 % sinon

    // croissance
    float croissanceBrute = jours * facteurSaison * facteurTerrain;
    int croissanceTotale = Math.Max(0, (int)Math.Round(croissanceBrute));
    JoursDepuisSemis += croissanceTotale;

    // Phases
    if      (JoursDepuisSemis >= EsperanceDeVie)                 Phase = "Morte";
    else if (JoursDepuisSemis >= JoursPourMaturité)               Phase = "Mature";
    else if (JoursDepuisSemis >= JoursPourMaturité * 2 / 3)     Phase = "En croissance";
    else if (JoursDepuisSemis >= JoursPourMaturité / 3)           Phase = "Jeune pousse";
    else                                                          Phase = "Graine";
}

    public void Arroser()
    {
        NiveauHydratation += 20;
    }
    
        public bool Recolter()
    {
        if (!Vivacite)
            return false;

        // vivace :
        CyclesRestants--;
        if (CyclesRestants <= 0)
            return false; 
        Phase = "En croissance";
        JoursDepuisSemis = JoursPourMaturité * 2 / 3;
        return true;
    }

}


public class Tomate : Plantes
{
    public Tomate()
    {
        Nom = "Tomate";
        Couleur = ConsoleColor.Red;
        Vivacite = false;
        Comestible = true;
        SaisonsDeSemis = new string[] { "Automne" };
        TerrainPrefere = "Terre";
        JoursPourMaturité = 60;
        EauHebdomadaire = 25;
        TemperaturePreferee = (20, 30);
        EsperanceDeVie = 120;
        NombrePhases = 4; // Graine, Jeune pousse, En croissance, Mature
        NiveauHydratation = 0;
        InitCycles();
    }


    public override List<(int dx, int dy)> Occupation => new List<(int, int)>
    {
        (0, 0)
    };
    public override string Croissance
    {
        get
        {
            return Phase switch
            {
                "Graine" => ".",
                "Jeune pousse" => "🌱",
                "En croissance" => "🎋",
                "Mature" => "🍅",
                "Morte" => "x",
                _ => "?"
            };
        }
    }
}


public class Mangue : Plantes
{
    public Mangue()
    {
        Nom = "Mangue";
        Vivacite = true;
        Comestible = true;
        SaisonsDeSemis = new string[] { "Printemps" };
        TerrainPrefere = "Terre";
        JoursPourMaturité = 180;
        EauHebdomadaire = 20;
        TemperaturePreferee = (25, 35);
        EsperanceDeVie = 3650;
        Couleur = ConsoleColor.Yellow;
        NombrePhases = 4; // Graine, Jeune pousse, En croissance, Mature
        NiveauHydratation = 0;
        InitCycles();
    }

    public override List<(int dx, int dy)> Occupation => new List<(int, int)>
    {
        (-1, -1), (0, -1), (1, -1),
        (-1, 0),  (0, 0),  (1, 0),
        (-1, 1),  (0, 1),  (1, 1)
    };

    
    public override string Croissance => Phase switch
    {
        "Graine" => ".",
        "Jeune pousse" => "🌱",
        "En croissance" => "🌳",
        "Mature" => "🥭",
        "Morte" => "x",
        _ => "?"
    };
}


public class Aubergine : Plantes
{
    public Aubergine()
    {
        Nom = "Aubergine";
        Vivacite = false;
        Comestible = true;
        SaisonsDeSemis = new string[] { "Automne" };
        TerrainPrefere = "Argile";
        JoursPourMaturité = 70;
        EauHebdomadaire = 30;
        TemperaturePreferee = (22, 30);
        EsperanceDeVie = 120;
        Couleur = ConsoleColor.DarkMagenta;
        NombrePhases = 4; // Graine, Jeune pousse, En croissance, Mature
        NiveauHydratation = 0;
        InitCycles();
    }

    public override List<(int dx, int dy)> Occupation => new List<(int, int)>
    {
        (0, 0), (1, 0),
        (0, 1), (1, 1)
    };

    public override string Croissance => Phase switch
    {
        "Graine" => ".",
        "Jeune pousse" => "🌱",
        "En croissance" => "🪴",
        "Mature" => "🍆",
        "Morte" => "x",
        _ => "?"
    };
}


public class The : Plantes
{
    public The()
    {
        Nom = "Thé";
        Vivacite = true;
        Comestible = true;
        SaisonsDeSemis = new string[] { "Été" };
        TerrainPrefere = "Argile";
        JoursPourMaturité = 365;
        EauHebdomadaire = 35;
        TemperaturePreferee = (22, 28);
        EsperanceDeVie = 1825;
        Couleur = ConsoleColor.Green;
        NombrePhases = 4; // Graine, Jeune pousse, En croissance, Mature
        NiveauHydratation = 0;
        InitCycles();
    }

    public override List<(int dx, int dy)> Occupation => new List<(int, int)>
    {
        (-1, 0), // case à gauche
        (0, 0),  // case centrale
        (1, 0)   // case à droite
    };
    public override string Croissance => Phase switch
    {
        "Graine" => ".",
        "Jeune pousse" => "🌱",
        "En croissance" => "🌿",
        "Mature" => "🍃",
        "Morte" => "x",
        _ => "?"
    };
}

public class Hibiscus : Plantes
{
    public Hibiscus()
    {
        Nom = "Hibiscus";
        Vivacite = true;
        Comestible = false;
        SaisonsDeSemis = new string[] { "Printemps", "Été", "Automne", "Hiver" };
        TerrainPrefere = "Sable";
        JoursPourMaturité = 90;
        EauHebdomadaire = 40;
        TemperaturePreferee = (23, 35);
        EsperanceDeVie = 1000;
        Couleur = ConsoleColor.DarkRed;
        NombrePhases = 4; // Graine, Jeune pousse, En croissance, Mature
        NiveauHydratation = 0;
        InitCycles();
    }

    public override List<(int dx, int dy)> Occupation => new List<(int, int)>
    {
        (0, 0), (1, 0)
    };

    public override string Croissance => Phase switch
    {
        "Graine" => ".",
        "Jeune pousse" => "🌱",
        "En croissance" => "🌿",
        "Mature" => "🌺",
        "Morte" => "x",
        _ => "?"
    };
}