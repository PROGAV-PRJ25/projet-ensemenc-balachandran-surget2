Jardin Jardin1 = new Jardin();
Inventaire inventaire = new Inventaire();
inventaire.AjouterObjet("Arrosoir", 1);
inventaire.AjouterObjet("Graine de tomate", 5);

for (int jour = 1; jour <= 7; jour++)
{
    Console.Clear();
    Console.WriteLine($"🌿 Jour {jour} 🌿");

    Meteo meteo = new Meteo();

    // Génère météo du jour
    meteo.Temperature = new Random().Next(20, 30);
    meteo.Humidite = new Random().Next(40, 90);
    meteo.Vent = new Random().Next(0, 40);
    meteo.Condition = new[] { "Ensoleillé", "Nuageux", "Pluie" }[new Random().Next(0, 3)];

    meteo.Afficher();

    Console.WriteLine();
    Jardin1.TracerJardin(2, 3);
    Console.WriteLine();
    inventaire.Afficher();

    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.Write("Appuie sur une touche pour passer au jour suivant...");
    Console.ResetColor();
    Console.ReadKey(); // Attente que l'utilisateur appuie sur une touche
    
}