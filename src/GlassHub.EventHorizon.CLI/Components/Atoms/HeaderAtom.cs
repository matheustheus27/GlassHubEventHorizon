using GlassHub.EventHorizon.Core.Constants;

namespace GlassHub.EventHorizon.CLI.Components.Atoms;

public static class HeaderAtom
{
    public static void RenderHeader(string title, string subtitle)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@"   ______ _    _ _   _ ");
        Console.WriteLine(@"  |  ____| |  | | | | |");
        Console.WriteLine(@"  | |__  | |  | | |_| |");
        Console.WriteLine(@"  |  __| | |  | |  _  |");
        Console.WriteLine(@"  | |____\ \__/ /| | | |");
        Console.WriteLine(@"  |______| \___/ |_| |_|");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"  >>> {AppConstants.Ecosystem} | GlassHub EventHorizon CLI (evh) v{AppConstants.Version}");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine($"  {subtitle}");
        Console.ResetColor();
        DividerAtom.Render();
    }
}
