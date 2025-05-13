public abstract class Plantes
{
    public string Nom { get; set; }
    public bool Vivacite { get; set; }
    public bool Comestible { get; set; }
    public string[] SaisonsDeSemis { get; set; }
    public string TerrainPrefere { get; set; }
    public int TailleX { get; set; }
    public int TailleY { get; set; }
    public int JoursPourMaturité { get; set; }
    public int EauHebdomadaire { get; set; }
    public string Lumiere { get; set; }
    public (int min, int max) TemperaturePreferee { get; set; }
    public int EsperanceDeVie { get; set; }
    public int ProductionParCycle { get; set; }
    public ConsoleColor Couleur { get; set; }

    // Attributs de croissance
    public string Phase { get; set; } = "Graine";
    public int JoursDepuisSemis { get; set; } = 0;

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
        TailleX = 1;
        TailleY = 1;
        JoursPourMaturité = 60;
        EauHebdomadaire = 25;
        Lumiere = "Plein Soleil";
        TemperaturePreferee = (20, 30);
        EsperanceDeVie = 120;
        ProductionParCycle = 3;
    }

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
        TailleX = 3;
        TailleY = 3;
        JoursPourMaturité = 180;
        EauHebdomadaire = 20;
        Lumiere = "Plein Soleil";
        TemperaturePreferee = (25, 35);
        EsperanceDeVie = 3650;
        ProductionParCycle = 10;
        Couleur = ConsoleColor.Yellow;
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
        TailleX = 1;
        TailleY = 1;
        JoursPourMaturité = 70;
        EauHebdomadaire = 30;
        Lumiere = "Plein Soleil";
        TemperaturePreferee = (22, 30);
        EsperanceDeVie = 120;
        ProductionParCycle = 4;
        Couleur = ConsoleColor.DarkMagenta;
    }

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
        TailleX = 2;
        TailleY = 2;
        JoursPourMaturité = 365;
        EauHebdomadaire = 35;
        Lumiere = "Mi-ombre";
        TemperaturePreferee = (18, 25);
        EsperanceDeVie = 1825;
        ProductionParCycle = 5;
        Couleur = ConsoleColor.Green;
    }

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
        TailleX = 2;
        TailleY = 2;
        JoursPourMaturité = 90;
        EauHebdomadaire = 40;
        Lumiere = "Plein Soleil";
        TemperaturePreferee = (23, 35);
        EsperanceDeVie = 1000;
        ProductionParCycle = 6;
        Couleur = ConsoleColor.Magenta;
    }

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
