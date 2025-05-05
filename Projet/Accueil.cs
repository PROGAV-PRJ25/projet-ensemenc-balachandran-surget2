
public class Accueil
{
    public void AfficherPageAccueil()
    {
        // Affichage du dessin ASCII pour la page d'accueil
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(@"  _____ _           _             _             ");
        Console.WriteLine(@" |_   _| |__   __ _| |_ ___ _ __ (_) __ _ _ __  ");
        Console.WriteLine(@"   | | | '_ \ / _` | __/ _ \ '_ \| |/ _` | '_ \ ");
        Console.WriteLine(@"   | | | | | | (_| | ||  __/ | | | | (_| | | | |");
        Console.WriteLine(@"   |_| |_| |_|\__,_|\__\___|_| |_|_|\__,_|_| |_|");
        Console.WriteLine();
        Console.WriteLine("Bienvenue dans le simulateur de jardinage !");
        Console.ResetColor();

        // Afficher le menu principal
        Console.WriteLine("\nQue souhaitez-vous faire ?");
        Console.WriteLine("1. Jouer");
        Console.WriteLine("2. Lire les règles");
        Console.WriteLine("3. Quitter");

        // Lecture de la sélection de l'utilisateur
        Console.Write("Choisissez une option (1, 2, ou 3) : ");
        var choix = Console.ReadKey().Key;

        Console.WriteLine();

        // Traitement de l'option choisie
        switch (choix)
        {
            case ConsoleKey.D1:
                Jouer();
                break;
            case ConsoleKey.D2:
                LireRegles();
                break;
            case ConsoleKey.D3:
                Quitter();
                break;
            default:
                Console.WriteLine("Choix invalide. Essayez encore.");
                AfficherPageAccueil(); // Afficher à nouveau le menu en cas de mauvais choix
                break;
        }
    }

    // Fonction pour commencer à jouer
    public void Jouer()
    {
        Console.Clear();
        Console.WriteLine("Vous avez choisi de jouer !");

        Jardin Jardin1 = new Jardin();
        Meteo meteo = new Meteo();
        Inventaire inventaire = new Inventaire();
        /*
        List<Plante> potager = new List<Plante>
            {
                new Plante { Nom = "Tomate" },
                new Plante { Nom = "Carotte" },
                new Plante { Nom = "Salade" }
            };
        */

        // Ajouter des objets à l'inventaire
        inventaire.AjouterObjet("Arrosoir", 1);
        inventaire.AjouterObjet("Graine de tomate", 5);
        inventaire.AjouterObjet("Graine de carotte", 3);


        for (int jour = 1; jour <= 7; jour++)
        {
            Console.Clear();
            Console.WriteLine($"🌿 Jour {jour} 🌿");

            // Génère météo du jour
            meteo.Temperature = new Random().Next(25, 35);
            meteo.Humidite = new Random().Next(60, 90);
            meteo.Vent = new Random().Next(5, 20);
            meteo.Condition = new[] { "Ensoleillé", "Nuageux", "Pluie" }[new Random().Next(0, 3)];

            meteo.Afficher();

            Console.WriteLine();
            Jardin1.TracerJardin(2, 3);

            // Afficher l'inventaire
            Console.WriteLine();
            inventaire.Afficher();

            Console.WriteLine();
            // Menu d'actions
            Console.WriteLine("\nQue voulez-vous faire ?");
            Console.WriteLine("1. Arroser les plantes");
            Console.WriteLine("2. Semer une graine");
            Console.WriteLine("3. Récolter des plantes");
            Console.WriteLine("4. Passer au jour suivant");
            Console.Write("Choix : ");

            var choix = Console.ReadKey().Key;
            Console.WriteLine();

            switch (choix)
            {
                case ConsoleKey.D1: // Arroser
                    if (inventaire.UtiliserObjet("Arrosoir"))
                    {
                        Console.WriteLine("Tu as arrosé les plantes. 🌧️ Croissance +5%");
                        //foreach (var plante in potager)
                        {
                            //  plante.Croissance += 5;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Tu n’as pas d’arrosoir !");
                    }
                    break;

                case ConsoleKey.D2: // Semer
                    Console.WriteLine("Quelles graines voulez-vous semer ?");
                    Console.WriteLine("1. Graine de tomate");
                    Console.WriteLine("2. Graine de carotte");
                    Console.Write("Choix : ");
                    var semerChoix = Console.ReadKey().Key;
                    Console.WriteLine();

                    if (semerChoix == ConsoleKey.D1 && inventaire.SemerGraine("Graine de tomate"))
                    {
                        Console.WriteLine("Tu as semé une graine de tomate.");
                    }
                    else if (semerChoix == ConsoleKey.D2 && inventaire.SemerGraine("Graine de carotte"))
                    {
                        Console.WriteLine("Tu as semé une graine de carotte.");
                    }
                    else
                    {
                        Console.WriteLine("Tu n'as pas cette graine dans ton inventaire.");
                    }
                    break;

                case ConsoleKey.D3: // Récolter
                    Console.WriteLine("Récolter ?");

                    // foreach (var plante in potager)
                    {
                        //if (plante.Croissance == 100)
                        {
                            //Console.WriteLine($"{plante.Nom} est prête à être récoltée !");
                        }
                    }
                    break;

                case ConsoleKey.D4: // Passer le jour
                    Console.WriteLine("Passage au jour suivant...");
                    break;

                default:
                    Console.WriteLine("Choix invalide.");
                    break;
            }

            Console.WriteLine("\nAppuie sur une touche pour continuer...");
            Console.ReadKey();
        }


    }

    // Fonction pour lire les règles du jeu
    public void LireRegles()
    {
        Console.Clear();
        Console.WriteLine("Voici les règles du jeu :\n");
        Console.WriteLine("1. Vous êtes un jardinier et vous devez cultiver des plantes.");
        Console.WriteLine("2. Vous avez un inventaire avec différents outils et graines.");
        Console.WriteLine("3. Chaque jour, vous devez arroser vos plantes et semer des graines.");
        Console.WriteLine("4. Vous pouvez récolter les plantes une fois qu'elles sont matures.");
        Console.WriteLine("5. Faites attention à la météo ! Elle peut affecter la croissance de vos plantes.");
        Console.WriteLine("6. L'objectif est de faire pousser vos plantes et récolter le plus de ressources possible.\n");
        Console.WriteLine("Appuyez sur une touche pour revenir à l'écran d'accueil.");
        Console.ReadKey();
        AfficherPageAccueil(); // Retour à l'accueil
    }

    // Fonction pour quitter le jeu
    public void Quitter()
    {
        Console.Clear();
        Console.WriteLine("Merci d'avoir joué ! À bientôt.");
        Environment.Exit(0); // Quitte l'application
    }
}