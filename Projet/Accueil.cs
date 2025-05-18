
public class TextHelper
{
    public static string CenterText(string text)
    {
        int largeurConsole = Console.WindowWidth;
        int positionX = (largeurConsole - text.Length) / 2;
        return new string(' ', Math.Max(positionX, 0))+ text.Trim();
    }
}



public class Accueil
{
    public void AfficherPageAccueil()
    {
        // Affichage du dessin ASCII pour la page d'accueil

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(TextHelper.CenterText(@",_____ _   _ ____  _____ __  __ _____ _   _  ____ "));
        Console.WriteLine(TextHelper.CenterText(@"| ____| \ | / ___|| ____|  \/  | ____| \ | |/ ___|"));
        Console.WriteLine(TextHelper.CenterText(@"|  _| |  \| \___ \|  _| | |\/| |  _| |  \| | |    "));
        Console.WriteLine(TextHelper.CenterText(@"| |___| |\  |___) | |___| |  | | |___| |\  | |___ "));
        Console.WriteLine(TextHelper.CenterText(@"|_____|_| \_|____/|_____|_|  |_|_____|_| \_|\____|"));

        Console.WriteLine();
        Console.WriteLine(TextHelper.CenterText("Bienvenue dans le simulateur de jardinage !"));
        Console.ResetColor();
        Console.WriteLine();

        // Afficher le menu principal
        Console.WriteLine(TextHelper.CenterText("\nQue souhaitez-vous faire ?"));
        Console.WriteLine(TextHelper.CenterText("1. Jouer"));
        Console.WriteLine(TextHelper.CenterText("2. Lire les règles"));
        Console.WriteLine(TextHelper.CenterText("3. Quitter"));
        Console.WriteLine();

        // Lecture de la sélection de l'utilisateur
        Console.Write(TextHelper.CenterText("Choisissez une option (1, 2, ou 3) : "));
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
        Meteo meteo = new Meteo();
        Jardin jardin = new Jardin(meteo);
        JardinCurseur curseur = new JardinCurseur(jardin);
        Inventaire inventaire = new Inventaire();


        // Ajouter des objets à l'inventaire
        inventaire.AjouterObjet("Arrosoir", 1);
        inventaire.AjouterObjet("graine de tomate", 5);
        inventaire.AjouterObjet("graine de aubergine", 3);
        inventaire.AjouterObjet("graine de mangue", 3);
        inventaire.AjouterObjet("graine de thé", 3);
        inventaire.AjouterObjet("graine de hibiscus", 3);

        Random rnd = new Random();
        for (int semaine = 1; semaine <= 15; semaine++)
        {

            // Génère météo du jour
            meteo.Temperature = rnd.Next(20, 35);
            meteo.Humidite = rnd.Next(60, 90);
            meteo.Vent = rnd.Next(5, 20);
            meteo.Condition = new[] { "Ensoleillé", "Nuageux", "Pluie" }[rnd.Next(0, 3)];



            bool finTour = false;
            while (!finTour)
            {
                Console.Clear();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"🌿 Semaine {semaine} 🌿");
                Console.ResetColor();

                meteo.Afficher();

                Console.WriteLine();
                Console.WriteLine();

                curseur.Afficher();

                Console.WriteLine();

                inventaire.Afficher();

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nQue voulez-vous faire ?");
                Console.WriteLine("0. Voir caractéristiques des plantes");
                Console.WriteLine("1. Arroser les plantes");
                Console.WriteLine("2. Semer une graine");
                Console.WriteLine("3. Récolter des plantes");
                Console.WriteLine("4. Passer à la semaine suivante");
                Console.Write("Choix : ");
                Console.ResetColor();


                ConsoleKey choix = Console.ReadKey().Key;
                Console.WriteLine();

                switch (choix)
                {
                    case ConsoleKey.D0:
                        curseur.VoirCaracteristiquesEnDeplacement();
                        break;

                    case ConsoleKey.D1:
                        Console.Clear();
                        Console.WriteLine("Tu vas arroser une plante. 🌿");

                        // Déplacement du curseur pour choisir l'emplacement à arroser
                        curseur.Deplacer();

                        // Récupérer la plante à l'endroit du curseur
                        Plantes? plante = curseur.ObtenirPlante();

                        if (plante == null)
                        {
                            Console.WriteLine("Il n'y a pas de plante ici à arroser.");
                        }
                        else
                        {
                            plante.Arroser(); // Appelle la méthode d'arrosage sur l'objet plante
                            Console.WriteLine($"💧 Tu as arrosé la plante !");
                        }

                        Console.WriteLine("\nAppuie sur une touche pour revenir au menu.");
                        Console.ReadKey();
                        break;

                    case ConsoleKey.D2:
                        curseur.Deplacer();
                        Console.WriteLine("\nQuelles graines voulez-vous semer ?");
                        var graines = inventaire.Objets.Where(o => o.Nom.Contains("graine")).ToList();

                        if (graines.Count == 0)
                        {
                            Console.WriteLine("Tu n'as pas de graines à semer !");
                            break;
                        }

                        // Afficher toutes les graines disponibles
                        for (int i = 0; i < graines.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {graines[i].Nom} x{graines[i].Quantite}");
                        }

                        Console.Write("\nChoix (tapez le numéro correspondant) : ");

                        if (int.TryParse(Console.ReadLine()!, out int choixGraine) && choixGraine > 0 && choixGraine <= graines.Count)
                        {
                            var graineChoisie = graines[choixGraine - 1];

                            Plantes planteChoisie = graineChoisie.Nom switch
                            {
                                "graine de tomate" => new Tomate(),
                                "graine de aubergine" => new Aubergine(),
                                "graine de mangue" => new Mangue(),
                                "graine de hibiscus" => new Hibiscus(),
                                "graine de thé" => new The(),
                                _ => throw new InvalidOperationException("Type de graine non reconnu.")
                            };

                            if (!curseur.PeutPlanter(planteChoisie))
                            {
                                Console.WriteLine("Impossible de planter ici : les cases sont occupées ou hors limites.");
                                break;
                            }

                            curseur.Planter(planteChoisie);

                            if (inventaire.SemerGraine(graineChoisie.Nom))
                            {
                                Console.WriteLine($"Tu as semé une {graineChoisie.Nom} 🌱 !");
                            }
                            else
                            {
                                Console.WriteLine("Erreur : Impossible de retirer la graine de l'inventaire.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Choix invalide.");
                        }
                        break;

                    case ConsoleKey.D3:
                        var recoltes = jardin.InventaireRecoltes(inventaire);
                        if (recoltes.Count == 0)
                        {
                            Console.WriteLine("🌾 Aucune plante n'est prête à être récoltée.");
                        }
                        else
                        {
                            Console.WriteLine("🌿 Récolte effectuée !");
                            foreach (var entry in recoltes)
                            {
                                string nom = entry.Key;
                                int qte = entry.Value;
                                string nomAffiche = qte > 1 ? nom + "s" : nom;
                                Console.WriteLine($"- {qte} {nomAffiche} ajouté(s) à l'inventaire.");
                            }
                        }
                        break;

                    case ConsoleKey.D4:
                        Console.WriteLine("Passage à la semaine suivante...");
                        Thread.Sleep(1000);
                        jardin.ToutPousser(20);
                        // Baisser l'hydratation de toutes les plantes de 20
                        BaisserHydratationPlantes(jardin);
                        finTour = true; // Permet de sortir de la boucle et avancer la semaine
                        break;

                    default:
                        Console.WriteLine("Choix invalide.");
                        break;
                }


                Console.WriteLine("\nAppuie sur une touche pour continuer...");
                Console.ReadKey();
            }
        }
    }

    public void BaisserHydratationPlantes(Jardin jardin)
    {
        foreach (var plante in jardin.ObtenirToutesLesPlantes())
        {
            plante.NiveauHydratation -= 20;
            if (plante.NiveauHydratation < 0)
                plante.NiveauHydratation = 0;
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
