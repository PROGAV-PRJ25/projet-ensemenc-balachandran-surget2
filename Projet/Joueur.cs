public class Joueur
{
    public int Argent { get; set; }
    public Inventaire Inventaire { get; set; }
    public Joueur()
    {
        Argent = 0; // départ
        Inventaire = new Inventaire();
    }

    // Méthode pour ajouter de l'argent
    public void AjouterArgent(int montant)
    {
        Argent += montant;
    }

    // Méthode pour dépenser de l'argent
    public bool DepenserArgent(int montant)
    {
        if (Argent >= montant)
        {
            Argent -= montant;
            return true;
        }
        return false;
    }
}
