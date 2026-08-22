namespace GlassHub.EventHorizon.CLI.Components.Atoms;

public static class DividerAtom
{
    public static void Render(int length = 60, char symbol = '─')
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine(new string(symbol, length));
        Console.ResetColor();
    }
}
