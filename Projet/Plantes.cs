public abstract class Plantes
{
    public string Nom { get; set; }
    public bool Vivacite { get; set; }
    public bool Comestible { get; set; }
    public string[] SaisonsDeSemis { get; set; }
    public string TerrainPrefere { get; set; }
    public int TailleX { get; set; } // nombre de cases horizontalement
    public int TailleY { get; set; } // nombre de cases verticalement
    public int JoursPourMaturité { get; set; }
    public int EauHebdomadaire { get; set; } // en mm
    public string Lumiere { get; set; } // "Plein Soleil", "Mi-ombre", etc.
    public (int min, int max) TemperaturePreferee { get; set; }
    public int EsperanceDeVie { get; set; }
    public int ProductionParCycle { get; set; }

    public static class SemisDispo
{
    public static Plantes CreerPlante(string type)
    {
        switch (type.ToLower())
        {
            case "tomate":
                return new Tomate();
            case "mangue":
                return new Mangue();
            case "aubergine":
                return new Aubergine();
            case "hibiscus":
                return new Hibiscus();
            case "thé":
                return new The();
            default:
                Console.WriteLine("Type de plante inconnu.");
                return null;
        }
    }
}


    public abstract void AfficherInfos();
}

public class Tomate : Plantes
{
    public Tomate()
    {
        Nom = "Tomate";
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

    public override void AfficherInfos()
    {
        Console.WriteLine($"Plante : {Nom}, Durée : {JoursPourMaturité}j, Production/cycle : {ProductionParCycle}");
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
    }

    public override void AfficherInfos()
    {
        Console.WriteLine($"Plante : {Nom}, Durée : {JoursPourMaturité}j, Production/cycle : {ProductionParCycle}");
    }
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
    }

    public override void AfficherInfos()
    {
        Console.WriteLine($"Plante : {Nom}, Durée : {JoursPourMaturité}j, Production/cycle : {ProductionParCycle}");
    }
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
    }

    public override void AfficherInfos()
    {
        Console.WriteLine($"Plante : {Nom}, Durée : {JoursPourMaturité}j, Production/cycle : {ProductionParCycle}");
    }
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
    }

    public override void AfficherInfos()
    {
        Console.WriteLine($"Plante : {Nom}, Durée : {JoursPourMaturité}j, Production/cycle : {ProductionParCycle}");
    }
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
