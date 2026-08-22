namespace GlassHub.EventHorizon.CLI.Components.Atoms;

public static class BadgeAtom
{
    public static void Render(string text, ConsoleColor foregroundColor = ConsoleColor.Cyan, ConsoleColor backgroundColor = ConsoleColor.Black)
    {
        Console.ForegroundColor = foregroundColor;
        Console.BackgroundColor = backgroundColor;
        Console.Write($"[{text}] ");
        Console.ResetColor();
    }

    public static void RenderSuccess(string text) => Render(text, ConsoleColor.Green);
    public static void RenderError(string text) => Render(text, ConsoleColor.Red);
    public static void RenderInfo(string text) => Render(text, ConsoleColor.DarkCyan);
    public static void RenderEcosystem(string text) => Render(text, ConsoleColor.Magenta);
}
