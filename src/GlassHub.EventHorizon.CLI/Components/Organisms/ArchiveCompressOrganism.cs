using GlassHub.EventHorizon.CLI.Components.Atoms;
using GlassHub.EventHorizon.CLI.Parsing;
using GlassHub.EventHorizon.Core.Interfaces;
using GlassHub.EventHorizon.Core.Localization;

namespace GlassHub.EventHorizon.CLI.Components.Organisms;

public static class ArchiveCompressOrganism
{
    public static void Execute(IArchiveEngine engine, CommandOptions options, ILocalizationService i18n)
    {
        if (options.InputFiles.Count == 0)
        {
            BadgeAtom.RenderError("FAIL");
            Console.WriteLine(i18n.GetString("NoInput"));
            return;
        }

        string outputFile = !string.IsNullOrEmpty(options.OutputFile) 
            ? options.OutputFile 
            : $"{options.InputFiles[0]}.zip";

        BadgeAtom.RenderInfo("ACTION");
        Console.WriteLine(i18n.GetString("Compressing"));

        for (int p = 10; p <= 100; p += 30)
        {
            ProgressBarAtom.Render(p, i18n.GetString("Compressing"));
            Thread.Sleep(30);
        }

        try
        {
            engine.Compress(options.InputFiles, outputFile, options.Password);
            BadgeAtom.RenderSuccess("SUCCESS");
            Console.WriteLine($"{i18n.GetString("Completed")} -> {outputFile}");
        }
        catch (Exception ex)
        {
            BadgeAtom.RenderError("FAIL");
            Console.WriteLine($"{i18n.GetString("Failed")}: {ex.Message}");
        }
    }
}
