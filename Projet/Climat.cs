public class Meteo
{
    public string Condition { get; set; } = "Ensoleillé";
    public int Temperature { get; set; } = 18;
    public int Humidite { get; set; } = 55;
    public int Vent { get; set; } = 10;

    public void Afficher()
    {
        Console.WriteLine(); // Ligne vide pour séparer du jardin

        Console.WriteLine("=== Météo 🌤️ ===\n");
        Console.WriteLine($"    Température : {Temperature}°C");
        Console.WriteLine($"    Humidité    : {Humidite}%");
        Console.WriteLine($"    Vent        : {Vent} km/h");
        Console.WriteLine($"    Ciel        : {Condition}\n");

    }
}
