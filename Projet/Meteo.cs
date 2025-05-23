public class Meteo
{
    public string Condition { get; set; } = "Ensoleillé";
    public int Temperature { get; set; } = 18;
    public int Humidite { get; set; } = 55;
    public int Vent { get; set; } = 10;


    // Méthode pour afficher la météo dans le jeu
    public void Afficher()
    {
        Console.WriteLine(); // Ligne vide pour séparer du jardin

        Console.WriteLine("      === Météo 🌤️ ===\n");
        Console.WriteLine($"    T° : {Temperature}°C");
        Console.WriteLine($"    Vent        : {Vent} km/h");
        Console.WriteLine($"    Ciel        : {Condition}\n");
    }

    public bool Pleut()
    {
        string conditionLower = Condition.ToLower();
        return conditionLower.Contains("pluie");
    }
}
