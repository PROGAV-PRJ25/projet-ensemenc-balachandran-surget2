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