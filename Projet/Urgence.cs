public class Urgence
{
    public void AfficherPageUrgence()
    {
        Jardin jardin = new Jardin();
        JardinCurseur curseur = new JardinCurseur(jardin);

        // Mode URGENCE qui clignote
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;
        for (int i = 0; i < 6; i++)
        {
            Console.WriteLine(TextHelper.CenterText(@",__  __  ___  ____  _____   _   _ ____   ____ _____ _   _  ____ _____   _ "));
            Console.WriteLine(TextHelper.CenterText(@"|  \/  |/ _ \|  _ \| ____| | | | |  _ \ / ___| ____| \ | |/ ___| ____| | |"));
            Console.WriteLine(TextHelper.CenterText(@"| |\/| | | | | | | |  _|   | | | | |_) | |  _|  _| |  \| | |   |  _|   | |"));
            Console.WriteLine(TextHelper.CenterText(@"| |  | | |_| | |_| | |___  | |_| |  _ <| |_| | |___| |\  | |___| |___  |_|"));
            Console.WriteLine(TextHelper.CenterText(@"|_|  |_|\___/|____/|_____|  \___/|_| \_\\____|_____|_| \_|\____|_____| (_)"));

            // Attendre 5 secondes avant de nettoyer l'écran
            Thread.Sleep(300); // Pause de 300ms pour voir le texte
            Console.Clear(); // Efface le texte
            Thread.Sleep(300); // Pause de 300ms avant de le réafficher

        }
        Console.Clear(); // Efface le texte
        Console.ResetColor();

        

        curseur.Afficher();

        


    }
}

