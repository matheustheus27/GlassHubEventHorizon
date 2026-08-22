using GlassHub.EventHorizon.CLI.Components.Atoms;
using GlassHub.EventHorizon.CLI.Parsing;
using GlassHub.EventHorizon.Core.Interfaces;
using GlassHub.EventHorizon.Core.Localization;

namespace GlassHub.EventHorizon.CLI.Components.Organisms;

public static class ArchiveExtractOrganism
{
    public static void Execute(IArchiveEngine engine, CommandOptions options, ILocalizationService i18n)
    {
        string sourceFile = !string.IsNullOrEmpty(options.SourceFile) 
            ? options.SourceFile 
            : (options.InputFiles.Count > 0 ? options.InputFiles[0] : string.Empty);

        if (string.IsNullOrEmpty(sourceFile))
        {
            BadgeAtom.RenderError("FAIL");
            Console.WriteLine(i18n.GetString("NoInput"));
            return;
        }

        string destination = !string.IsNullOrEmpty(options.DestinationDirectory) 
            ? options.DestinationDirectory 
            : Path.GetFileNameWithoutExtension(sourceFile);

        BadgeAtom.RenderInfo("ACTION");
        Console.WriteLine(i18n.GetString("Extracting"));

        for (int p = 10; p <= 100; p += 30)
        {
            ProgressBarAtom.Render(p, i18n.GetString("Extracting"));
            Thread.Sleep(30);
        }

        try
        {
            engine.Decompress(sourceFile, destination, options.Password);
            BadgeAtom.RenderSuccess("SUCCESS");
            Console.WriteLine($"{i18n.GetString("Completed")} -> {destination}");
        }
        catch (Exception ex)
        {
            BadgeAtom.RenderError("FAIL");
            Console.WriteLine($"{i18n.GetString("Failed")}: {ex.Message}");
        }
    }
}
