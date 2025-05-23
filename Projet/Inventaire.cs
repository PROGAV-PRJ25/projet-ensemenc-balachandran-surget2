
public class Inventaire
{
    public List<Objet> Objets { get; set; } = new List<Objet>(); // Création d'une liste d'objets

    // Méthode pour ajouter un objet dans l'inventaire
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

    // Méthode pour retirer un objet de l'inventaire
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

    // Méthode pour afficher l'inventaire
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

    // Permet de retirer les graines de l'inventaire
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
