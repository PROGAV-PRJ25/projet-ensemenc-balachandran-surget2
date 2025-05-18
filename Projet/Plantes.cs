public abstract class Plantes
{
    public string Nom { get; set; }
    public bool Vivacite { get; set; }
    public bool Comestible { get; set; }
    public string[] SaisonsDeSemis { get; set; }
    public string TerrainPrefere { get; set; }
    public abstract List<(int dx, int dy)> Occupation { get; }
    public int NiveauHydratation { get; set; }
    public int JoursPourMaturité { get; set; }
    public int EauHebdomadaire { get; set; }
    public string Lumiere { get; set; }
    public (int min, int max) TemperaturePreferee { get; set; }
    public int EsperanceDeVie { get; set; }
    public ConsoleColor Couleur { get; set; }

    // Attributs de croissance
    public string Phase { get; set; } = "Graine";
    public int PhaseActuelle { get; set; }  // index de la phase courante
    public int NombrePhases { get; set; } // nombre total de phases
    public int JoursDepuisSemis { get; set; } = 0;

    // Pour savoir si la plante est considérée comme arrosée cette période (par exemple si hydratation >= 50)
    public bool EstArrosee => NiveauHydratation >= EauHebdomadaire;

    // Pour savoir si la plante est mature
    public bool EstMature => Phase == "Mature";

    public Plantes()
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
    public void Grandir(Meteo meteo)
    {
        if (Phase == "Morte") return;

        // Croissance normale + boost si pluie
        int joursACroissance = 1;

        // Vérification température : si en dehors des bornes préférées, pas de croissance
        if (meteo.Temperature < TemperaturePreferee.min - 5 || meteo.Temperature > TemperaturePreferee.max + 5)
        {
            // Pas de croissance car température vraiment trop extrême
            return;
        }
        else if (meteo.Temperature < TemperaturePreferee.min || meteo.Temperature > TemperaturePreferee.max)
        {
            joursACroissance = 1; // croissance lente mais possible
        }

        if (meteo.Condition.ToLower().Contains("pluie"))
        {
            joursACroissance = 2; // double croissance par jour de pluie
            NiveauHydratation += 20;
        }



        if (NiveauHydratation < EauHebdomadaire) // On peut considérer EauHebdomadaire comme seuil minimal ici
        {
            // Pas assez d'eau, pas de croissance
            return;
        }
        if (NiveauHydratation > EauHebdomadaire * 2) // trop d'eau
        {
            // Trop d'eau = stress = pas de croissance
            return;
        }


        for (int i = 0; i < joursACroissance; i++)
        {
            JoursDepuisSemis++;
            if (JoursDepuisSemis >= EsperanceDeVie)
            {
                Phase = "Morte";
            }
            else if (JoursDepuisSemis >= JoursPourMaturité)
            {
                Phase = "Mature";
                PhaseActuelle = 4;
            }
            else if (JoursDepuisSemis >= JoursPourMaturité * 2 / 3)
            {
                Phase = "En croissance";
                PhaseActuelle = 3;
            }
            else if (JoursDepuisSemis >= JoursPourMaturité / 3)
            {
                Phase = "Jeune pousse";
                PhaseActuelle = 2;
            }
            else
            {
                Phase = "Graine";
                PhaseActuelle = 1;
            }
        }

    }

    public void Arroser()
    {
        NiveauHydratation += 20;
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
        Lumiere = "Plein Soleil";
        TemperaturePreferee = (20, 30);
        EsperanceDeVie = 120;
        NombrePhases = 4; // Graine, Jeune pousse, En croissance, Mature
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
        Lumiere = "Plein Soleil";
        TemperaturePreferee = (25, 35);
        EsperanceDeVie = 3650;
        Couleur = ConsoleColor.Yellow;
        NombrePhases = 4; // Graine, Jeune pousse, En croissance, Mature
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
        Lumiere = "Plein Soleil";
        TemperaturePreferee = (22, 30);
        EsperanceDeVie = 120;
        Couleur = ConsoleColor.DarkMagenta;
        NombrePhases = 4; // Graine, Jeune pousse, En croissance, Mature
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
        Lumiere = "Mi-ombre";
        TemperaturePreferee = (22, 28);
        EsperanceDeVie = 1825;
        Couleur = ConsoleColor.Green;
        NombrePhases = 4; // Graine, Jeune pousse, En croissance, Mature
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
        Lumiere = "Plein Soleil";
        TemperaturePreferee = (23, 35);
        EsperanceDeVie = 1000;
        Couleur = ConsoleColor.DarkRed;
        NombrePhases = 4; // Graine, Jeune pousse, En croissance, Mature
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




// PLANTES

public class Comestibles {

}
public class PlantesOrnementales {

}
public class PlantesIndustrielles {

}
public class MauvaisesHerbes {

}
