public class Case
{
    public Plantes Plante { get; set; }

    public string Afficher()
    {
        return Plante == null ? "□" : Plante.Croissance;
    }
}

public class Terrain
{
    public Case[,] Cases { get; }

    public Terrain()
    {
        Cases = new Case[3, 3];
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                Cases[i, j] = new Case();
    }
}

public class Jardin
{
    public Terrain[,] Terrains { get; }

    public Jardin()
    {
        Terrains = new Terrain[2, 3];
        for (int i = 0; i < 2; i++)
            for (int j = 0; j < 3; j++)
                Terrains[i, j] = new Terrain();
    }
}

public class JardinCurseur
{
    public Jardin jardin;
    public int terrainX;
    public int terrainY;
    public int caseX;
    public int caseY;

    public JardinCurseur(Jardin jardin)
    {
        this.jardin = jardin;
        terrainX = 0;
        terrainY = 0;
        caseX = 0;
        caseY = 0;
    }

    public void Deplacer()
    {
        bool enDeplacement = true; // Variable pour quitter la boucle
        while (enDeplacement)
        {
            Console.Clear();
            Console.WriteLine("Choisissez une case avec les flèches et appuyez sur Entrée.");
            Afficher();
            var keyInfo = Console.ReadKey(true);
            var key = keyInfo.Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    if (caseY == 0)
                    {
                        terrainX = (terrainX + 1) % 2; // terrain précédent verticalement
                        caseY = 2;
                    }
                    else
                    {
                        caseY--;
                    }
                    break;

                case ConsoleKey.DownArrow:
                    if (caseY == 2)
                    {
                        terrainX = (terrainX + 1) % 2; // terrain suivant verticalement
                        caseY = 0;
                    }
                    else
                    {
                        caseY++;
                    }
                    break;

                case ConsoleKey.LeftArrow:
                    if (caseX == 0)
                    {
                        terrainY = (terrainY + 2) % 3; // terrain précédent horizontalement
                        caseX = 2;
                    }
                    else
                    {
                        caseX--;
                    }
                    break;

                case ConsoleKey.RightArrow:
                    if (caseX == 2)
                    {
                        terrainY = (terrainY + 1) % 3; // terrain suivant horizontalement
                        caseX = 0;
                    }
                    else
                    {
                        caseX++;
                    }
                    break;

                case ConsoleKey.Enter: // Quitter le mode déplacement
                enDeplacement = false;
                break;
            }
        }
    }

    public void Afficher()
    {
        for (int tx = 0; tx < 2; tx++)
        {
            for (int row = 0; row < 3; row++)
            {
                for (int ty = 0; ty < 3; ty++)
                {
                    for (int col = 0; col < 3; col++)
                    {
                        if (tx == terrainX && ty == terrainY && row == caseY && col == caseX)
                            Console.Write("▣ "); // Curseur
                        else
                        {
                            var plante = jardin.Terrains[tx, ty].Cases[row, col].Plante;
                            if (plante == null)
                            {
                                Console.Write("□ ");
                            }
                            else
                            {
                                Console.ForegroundColor = plante.Couleur;
                                Console.Write(plante.Croissance + " ");
                                Console.ResetColor();
                            }

                        }
                    }
                    Console.Write("   ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }



    public void Planter(Plantes plante)
    {
        jardin.Terrains[terrainX, terrainY].Cases[caseY, caseX].Plante = plante;
    }
}

