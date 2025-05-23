
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
    const int objectifArgent = 100;
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
        Console.WriteLine("");
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
        Console.WriteLine("Choisissez votre saison de départ :");
        Console.WriteLine("1. Printemps 🌸  2. Été 🌞  3. Automne 🍂   4. Hiver ⛄");
        string choixSaison = Console.ReadKey(true).Key switch
        {
            ConsoleKey.D1 => "Printemps",
            ConsoleKey.D2 => "Été",
            ConsoleKey.D3 => "Automne",
            ConsoleKey.D4 => "Hiver",
            _             => "Printemps"
        };
        Console.WriteLine($"Saison sélectionnée : {choixSaison}\n");
        Meteo meteo = new Meteo();
        Jardin jardin = new Jardin(meteo);
        JardinCurseur curseur = new JardinCurseur(jardin);
        Joueur joueur = new Joueur();
        Urgence Urgence = new Urgence(jardin, curseur);


        // Ajouter des objets à l'inventaire
        joueur.Inventaire.AjouterObjet("Arrosoir", 1);
        joueur.Inventaire.AjouterObjet("graine de tomate", 5);
        joueur.Inventaire.AjouterObjet("graine de aubergine", 3);
        joueur.Inventaire.AjouterObjet("graine de mangue", 3);
        joueur.Inventaire.AjouterObjet("graine de thé", 3);
        joueur.Inventaire.AjouterObjet("graine de hibiscus", 3);

        Random rnd = new Random();
        for (int semaine = 1; semaine <= 15; semaine++)
        {

            // Génère météo du jour
            string saison = ObtenirSaison(semaine);
            GenererMeteo(meteo, saison, rnd); 
            AjoutEauPluie(jardin, meteo);

            bool finTour = false;
            while (!finTour)
            {
                Console.Clear();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"=== Saison : {saison} ===");
                Console.ResetColor();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"🌿 Semaine {semaine} 🌿");
                Console.ResetColor();

                meteo.Afficher();

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"    {joueur.Argent} pièces / {objectifArgent} pièces");
                Console.ResetColor();
                Console.WriteLine();

                curseur.Afficher();
                curseur.AfficherInfosSousCurseur();

                Console.WriteLine();

                joueur.Inventaire.Afficher();



                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nQue voulez-vous faire ?");
                Console.WriteLine("1. Semer une graine");
                Console.WriteLine("2. Arroser les plantes");
                Console.WriteLine("3. Récolter des plantes");
                Console.WriteLine("4. Passer à la semaine suivante");
                Console.WriteLine("5. Accéder à la boutique 🛒");
                Console.WriteLine("");
                Console.WriteLine("6. Retour à l'accueil (⚠ progression perdue)");
                Console.Write("Choix : ");
                Console.ResetColor();


                ConsoleKey choix = Console.ReadKey().Key;
                Console.WriteLine();

                switch (choix)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.RightArrow:
                        curseur.DeplacerUneFois(choix);
                        break;


                    case ConsoleKey.D1:
                        
                        bool choixValide = false;
                        while (!choixValide)
                        {
                            Console.Clear();
                            Console.WriteLine("🌱 Vous avez choisi de semer une graine !");
                            Console.WriteLine("\nVoici votre inventaire de graines :\n");

                            var graines = joueur.Inventaire.Objets.Where(o => o.Nom.Contains("graine")).ToList();

                            if (graines.Count == 0)
                            {
                                Console.WriteLine("Tu n'as pas de graines à semer !");
                                Console.WriteLine("\nAppuie sur une touche pour revenir.");
                                Console.ReadKey();
                                break;
                            }

                            for (int i = 0; i < graines.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}. {graines[i].Nom} x{graines[i].Quantite}");
                            }

                            Console.Write("\nChoix (tapez le numéro correspondant) : ");

                            if (int.TryParse(Console.ReadLine()!, out int choixGraine) && choixGraine > 0 && choixGraine <= graines.Count)
                            {
                                choixValide = true;

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

                                bool positionValide = false;

                                while (!positionValide)
                                {
                                    Console.WriteLine("\nChoisissez un emplacement pour semer cette graine.\n");
                                    curseur.Deplacer(instructions: true, plante: planteChoisie);

                                    if (!curseur.PeutPlanter(planteChoisie))
                                    {
                                        int casesNecessaires = planteChoisie.Occupation.Count;
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine($"\n Impossible de planter ici. {planteChoisie.Nom} a besoin de {casesNecessaires} case(s) libres autour de la case centrale.");
                                        Console.ResetColor();
                                        Console.WriteLine("Veuillez choisir un autre emplacement.\n");
                                        continue;
                                    }

                                    // Planter
                                    curseur.Planter(planteChoisie);

                                    if (joueur.Inventaire.SemerGraine(graineChoisie.Nom))
                                    {
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.WriteLine($"\n✅ {planteChoisie.Nom} plantée avec succès !");
                                        Console.ResetColor();
                                    }
                                    else
                                    {
                                        Console.WriteLine(" Erreur : Impossible de retirer la graine de l'inventaire.");
                                    }
                                    
                                    positionValide = true;
                                }
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("\n Choix invalide. Essayez à nouveau.");
                                Console.ReadKey();
                                Console.ResetColor();
                            }
                        }
                        break;

                    
                    case ConsoleKey.D2:
                        Console.Clear();

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
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine($"\n💧 Tu as arrosé tous les plants de {plante.Nom} !");
                            Console.WriteLine($"Cette plante occupe {plante.Occupation.Count} case(s). Toutes ses cases ont été hydratées.");
                            Console.ResetColor();
    
                        }

                        break;

                    case ConsoleKey.D3:
                        var recoltes = jardin.InventaireRecoltes(joueur.Inventaire);
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
                                jardin.ToutPousser(meteo, saison, 7);
                                // Baisser l'hydratation de toutes les plantes de 20
                                BaisserHydratationPlantes(jardin);

                                // URGENCE
                                if (rnd.Next(1, 7) == 1) // 1 chance sur 7
                                {
                                    Urgence.AfficherPageUrgence();
                                    Urgence.Elephant();
                                }
                                finTour = true; // Permet de sortir de la boucle et avancer la semaine
                                break;

                            default:
                                Console.WriteLine("Choix invalide.");
                                break;

                    case ConsoleKey.D5:
                            var boutique = new Boutique();
                            boutique.Afficher(joueur);
                            break;
                    
                    case ConsoleKey.D6:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\n⚠ Vous êtes sur le point de revenir à l'écran d'accueil.");
                            Console.WriteLine("Toute votre progression actuelle sera PERDUE !");
                            Console.ResetColor();
                            Console.Write("\nÊtes-vous sûr ? (o/n) : ");
                            var confirmation = Console.ReadKey().Key;
                            Console.WriteLine();

                            if (confirmation == ConsoleKey.O)
                            {
                                Console.WriteLine("\nRetour à l'accueil...");
                                Thread.Sleep(1000);
                                AfficherPageAccueil(); // Retour à l'accueil
                            }
                            else
                            {
                                Console.WriteLine("\nAction annulée. Retour au jeu.");
                                Thread.Sleep(1000);
                            }
                            break;
                }

                if (joueur.Argent >= objectifArgent)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n🎉 Félicitations !");
                    Console.WriteLine("💰 Vous avez atteint 100 pièces !");
                    Console.WriteLine("🏆 Vous avez gagné !");
                    Console.ResetColor();
                    Console.WriteLine("\nAppuyez sur une touche pour revenir à l'accueil...");
                    Console.ReadKey();
                    AfficherPageAccueil();
                    return; // sort de Jouer()
                }

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

    public void AjoutEauPluie(Jardin jardin, Meteo meteo)
    {
        bool pluie = meteo.Condition.ToLower().Contains("pluie");

        foreach (var plante in jardin.ObtenirToutesLesPlantes())
        {
            if (pluie)
                plante.NiveauHydratation += 10;
        }
    }


    public void GenererMeteo(Meteo meteo, string saison, Random rnd)
    {
        int tMin, tMax, hMin, hMax, vMin, vMax; //température min max, humidité min max, vent min max
        Dictionary<string, int> proba;

        switch (saison)
        {
            case "Printemps":
                tMin = 28; tMax = 33; hMin = 75; hMax = 90; vMin = 5; vMax = 15;
                proba = new() { ["Ensoleillé"] = 20, ["Nuageux"] = 40, ["Pluie"] = 40 };
                break;
            case "Été":
                tMin = 27; tMax = 31; hMin = 80; hMax = 95; vMin = 15; vMax = 25;
                proba = new() { ["Ensoleillé"] = 50, ["Nuageux"] = 20, ["Pluie"] = 30 };
                break;
            case "Automne":
                tMin = 27; tMax = 32; hMin = 75; hMax = 90; vMin = 10; vMax = 20;
                proba = new() { ["Ensoleillé"] = 20, ["Nuageux"] = 40, ["Pluie"] = 40 };
                break;
            case "Hiver":
                tMin = 25; tMax = 30; hMin = 70; hMax = 85; vMin = 10; vMax = 20;
                proba = new() { ["Ensoleillé"] = 30, ["Nuageux"] = 50, ["Pluie"] = 20 };
                break;
            default:
                tMin = 20; tMax = 30; hMin = 50; hMax = 70; vMin = 5; vMax = 20;
                proba = new() { ["Ensoleillé"] = 20, ["Nuageux"] = 40, ["Pluie"] = 40 };
                break;
        }

        meteo.Temperature = rnd.Next(tMin, tMax + 1);
        meteo.Humidite = rnd.Next(hMin, hMax + 1);
        meteo.Vent = rnd.Next(vMin, vMax + 1);

        // tirage pondéré de la condition
        int total = proba.Values.Sum();
        int tir = rnd.Next(1, total + 1), cumul = 0;
        foreach (var kv in proba)
        {
            cumul += kv.Value;
            if (tir <= cumul)
            {
                meteo.Condition = kv.Key;
                break;
            }
        }
    }

    private readonly string[] Saisons = { "Printemps", "Été", "Automne", "Hiver" };
    private const int SemainesParSaison = 4;

    public string ObtenirSaison(int semaine)
{
    // (semaine-1) pour que la semaine 1 soit dans le 1er index
    int index = (semaine - 1) / SemainesParSaison % Saisons.Length;
    return Saisons[index];
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
