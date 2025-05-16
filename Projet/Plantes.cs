public abstract class Plantes
{
    public string Nom { get; set; }
    public bool Vivacite { get; set; }
    public bool Comestible { get; set; }
    public string[] SaisonsDeSemis { get; set; }
    public string TerrainPrefere { get; set; }
    public abstract List<(int dx, int dy)> Occupation { get; }

    public int JoursPourMaturité { get; set; }
    public int EauHebdomadaire { get; set; }
    public string Lumiere { get; set; }
    public (int min, int max) TemperaturePreferee { get; set; }
    public int EsperanceDeVie { get; set; }
    public ConsoleColor Couleur { get; set; }

    // Attributs de croissance
    public string Phase { get; set; } = "Graine";
    public int JoursDepuisSemis { get; set; } = 0;

    public Plantes()
{
    JoursDepuisSemis = 0;
    Phase = "Graine";
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
                "Mature" => "🌿", // Peut être redéfini dans les classes filles
                "Morte" => "x",
                _ => "?"
            };
        }
    }

    // Croissance gérée ici pour toutes les plantes
    public void Grandir()
    {
        JoursDepuisSemis++;
        if (Phase == "Morte") return;

        if (JoursDepuisSemis >= EsperanceDeVie)
        {
            Phase = "Morte";
        }
        else if (JoursDepuisSemis >= JoursPourMaturité)
        {
            Phase = "Mature";
        }
        else if (JoursDepuisSemis >= JoursPourMaturité * 2 / 3)
        {
            Phase = "En croissance";
        }
        else if (JoursDepuisSemis >= JoursPourMaturité / 3)
        {
            Phase = "Jeune pousse";
        }
        else
        {
            Phase = "Graine";
        }
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
    }

    public override List<(int dx, int dy)> Occupation => new() { (0, 0) }; // 1 case

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
    }

    public override List<(int dx, int dy)> Occupation
    {
        get
        {
            var coords = new List<(int dx, int dy)>();
            for (int dx = 0; dx < 3; dx++)
                for (int dy = 0; dy < 3; dy++)
                    coords.Add((dx, dy));
            return coords;
        }
    }
    
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
    }

    public override List<(int dx, int dy)> Occupation => new() { (0, 0), (1, 0), (0, 1), (1, 1) }; // carré 2x2

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
        TemperaturePreferee = (18, 25);
        EsperanceDeVie = 1825;
        Couleur = ConsoleColor.Green;
    }

    public override List<(int dx, int dy)> Occupation => new() { (0, 0), (1, 0), (2, 0) }; // ligne horizontale

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
    }

    public override List<(int dx, int dy)> Occupation => new() { (0, 0), (1, 0) };

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
