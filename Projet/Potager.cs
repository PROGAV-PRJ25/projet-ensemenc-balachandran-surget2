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
    private Jardin jardin;
    private int terrainX;
    private int terrainY;
    private int caseX;
    private int caseY;

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
        while (true)
        {
            Console.Clear();
            Afficher();
            ConsoleKey key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.UpArrow: caseY = (caseY + 2) % 3; break; // haut
                case ConsoleKey.DownArrow: caseY = (caseY + 1) % 3; break; // bas
                case ConsoleKey.LeftArrow: caseX = (caseX + 2) % 3; break; // gauche
                case ConsoleKey.RightArrow: caseX = (caseX + 1) % 3; break; // droite
                case ConsoleKey.A: terrainX = (terrainX + 1) % 2; break;
                case ConsoleKey.Z: terrainX = (terrainX + 1) % 2; break;
                case ConsoleKey.E: terrainY = (terrainY + 2) % 3; break;
                case ConsoleKey.R: terrainY = (terrainY + 1) % 3; break;
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
                            Console.Write(plante == null ? "□ " : plante.Croissance + " ");
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

