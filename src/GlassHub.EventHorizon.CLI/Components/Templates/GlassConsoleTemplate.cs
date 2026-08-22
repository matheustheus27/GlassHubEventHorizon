using GlassHub.EventHorizon.CLI.Components.Atoms;
using GlassHub.EventHorizon.Core.Constants;
using GlassHub.EventHorizon.Core.Localization;

namespace GlassHub.EventHorizon.CLI.Components.Templates;

public static class GlassConsoleTemplate
{
    public static void RenderShell(ILocalizationService i18n, Action action)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        HeaderAtom.RenderHeader("GlassHub EventHorizon CLI", i18n.GetString("AppHeader"));
        
        try
        {
            action();
        }
        finally
        {
            DividerAtom.Render();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"  {AppConstants.Ecosystem} © 2026 | Language: [{i18n.CurrentLanguage}]");
            Console.ResetColor();
        }
    }
}
