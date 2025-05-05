// CONSTRUCTION POTAGER
public class Jardin
{
    public void TracerJardin(int rows, int cols)
    {
        int LargeurGrille = 13; // Largeur d'une grille
        int HauteurGrille = 7; // Hauteur d'une grille
        int EspaceHorizontal = 4; // Espacement horizontal entre les grilles
        int EspaceVertical = 2; // Espacement vertical entre les rangées de grilles

        // Calcul des dimensions totales du rectangle
        int Largeur = cols * LargeurGrille + (cols - 1) * EspaceHorizontal + 2; // Largeur totale ajustée
        int Hauteur = rows * HauteurGrille + (rows - 1) * EspaceVertical + 4; // Hauteur totale

        // Dessiner le cadre supérieur
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔" + new string('═', Largeur - 8) + "╗");

        for (int row = 0; row < rows; row++)
        {
            for (int gridRow = 0; gridRow < 3; gridRow++) // Chaque grille a 3 lignes
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("║"); // Bord gauche
                Console.ResetColor();
                for (int col = 0; col < cols; col++)
                {
                    Console.Write("   |   |   ");
                    if (col < cols - 1)
                    {
                        Console.Write(new string(' ', EspaceHorizontal)); // Espacement horizontal
                    }
                }
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("║"); // Bord droit
                Console.ResetColor();

                if (gridRow < 2) // Lignes séparant les cases dans une grille
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("║"); // Bord gauche
                    Console.ResetColor();
                    for (int col = 0; col < cols; col++)
                    {
                        Console.Write("---+---+---");
                        if (col < cols - 1)
                        {
                            Console.Write(new string(' ', EspaceVertical)); // Espacement horizontal
                        }
                    }
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("║"); // Bord droit
                    Console.ResetColor();
                }
            }

            if (row < rows - 1) // Espacement entre les rangées de grilles
            {
                for (int i = 0; i < EspaceVertical; i++)
                {
                    Console.WriteLine(); // Ligne vide pour espacer
                }
            }
        }

        // Dessiner le cadre inférieur
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╚" + new string('═', Largeur - 8) + "╝");
        Console.ResetColor();
    }
}

// TERRAIN

public abstract class Terrain {

}
public class TerrainSableux : Terrain{

}
public class TerrainArgileux : Terrain{

}
public class TerrainTerreux : Terrain
{

}

