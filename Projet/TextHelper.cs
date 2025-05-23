
// Permet de centrer le texte
public class TextHelper
{
    public static string CenterText(string text)
    {
        int largeurConsole = Console.WindowWidth;
        int positionX = (largeurConsole - text.Length) / 2;
        return new string(' ', Math.Max(positionX, 0)) + text.Trim();
    }
}