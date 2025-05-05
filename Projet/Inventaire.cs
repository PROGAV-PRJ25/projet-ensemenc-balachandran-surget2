public class Objet
{
    public string Nom { get; set; }
    public int Quantite { get; set; }

    public Objet(string nom, int quantite)
    {
        Nom = nom;
        Quantite = quantite;
    }

    public override string ToString()
    {
        return $"{Nom} x{Quantite}";
    }
}

public class Inventaire
{
    public List<Objet> Objets { get; set; } = new List<Objet>();

    public void AjouterObjet(string nom, int quantite)
    {
        var objet = Objets.FirstOrDefault(o => o.Nom == nom);
        if (objet != null)
        {
            objet.Quantite += quantite;
        }
        else
        {
            Objets.Add(new Objet(nom, quantite));
        }
    }

    public bool UtiliserObjet(string nom, int quantite = 1)
    {
        var objet = Objets.FirstOrDefault(o => o.Nom == nom);
        if (objet != null && objet.Quantite >= quantite)
        {
            objet.Quantite -= quantite;
            if (objet.Quantite == 0) Objets.Remove(objet);
            return true;
        }
        return false;
    }

    public void Afficher()
    {
        Console.WriteLine("🎒 Inventaire :");
        foreach (var objet in Objets)
        {
            Console.WriteLine($" - {objet}");
        }

        if (Objets.Count == 0)
            Console.WriteLine(" - (vide)");
    }

    public bool SemerGraine(string nom)
    {
        if (Objets.Any(o => o.Nom == nom && o.Quantite > 0))
        {
            UtiliserObjet(nom);
            return true;
        }
        return false;
    }
}
