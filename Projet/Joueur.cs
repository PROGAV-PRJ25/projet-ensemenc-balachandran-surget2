public class Joueur
{
    public int Argent { get; set; }
    public Inventaire Inventaire { get; set; }

    public Joueur()
    {
        Argent = 100; // départ
        Inventaire = new Inventaire();
    }

    public void AjouterArgent(int montant)
    {
        Argent += montant;
    }

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
