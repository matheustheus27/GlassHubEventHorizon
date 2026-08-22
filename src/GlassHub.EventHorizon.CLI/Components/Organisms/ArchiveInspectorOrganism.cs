using GlassHub.EventHorizon.CLI.Components.Atoms;
using GlassHub.EventHorizon.CLI.Components.Molecules;
using GlassHub.EventHorizon.CLI.Parsing;
using GlassHub.EventHorizon.Core.Interfaces;
using GlassHub.EventHorizon.Core.Localization;

namespace GlassHub.EventHorizon.CLI.Components.Organisms;

public static class ArchiveInspectorOrganism
{
    public static void List(IArchiveEngine engine, CommandOptions options, ILocalizationService i18n)
    {
        string archiveFile = GetArchiveFile(options);
        if (string.IsNullOrEmpty(archiveFile) || !File.Exists(archiveFile))
        {
            BadgeAtom.RenderError("FAIL");
            Console.WriteLine(i18n.Format("FileNotFound", archiveFile));
            return;
        }

        BadgeAtom.RenderInfo("LIST");
        Console.WriteLine(i18n.GetString("EntryListHeader"));
        DividerAtom.Render(50, '─');

        try
        {
            var entries = engine.ListContents(archiveFile, options.Password);
            foreach (var entry in entries)
            {
                Console.WriteLine($"  ├─ {entry}");
            }
            DividerAtom.Render(50, '─');
            BadgeAtom.RenderSuccess("DONE");
            Console.WriteLine($"Total Entries: {entries.Count}");
        }
        catch (Exception ex)
        {
            BadgeAtom.RenderError("FAIL");
            Console.WriteLine($"{i18n.GetString("Failed")}: {ex.Message}");
        }
    }

    public static void Info(IArchiveEngine engine, CommandOptions options, ILocalizationService i18n)
    {
        string archiveFile = GetArchiveFile(options);
        if (string.IsNullOrEmpty(archiveFile) || !File.Exists(archiveFile))
        {
            BadgeAtom.RenderError("FAIL");
            Console.WriteLine(i18n.Format("FileNotFound", archiveFile));
            return;
        }

        try
        {
            var metadata = engine.GetMetadata(archiveFile, options.Password);
            FileMetadataMolecule.Render(metadata, i18n.GetString("MetadataHeader"));
        }
        catch (Exception ex)
        {
            BadgeAtom.RenderError("FAIL");
            Console.WriteLine($"{i18n.GetString("Failed")}: {ex.Message}");
        }
    }

    public static void Verify(IArchiveEngine engine, CommandOptions options, ILocalizationService i18n)
    {
        string archiveFile = GetArchiveFile(options);
        if (string.IsNullOrEmpty(archiveFile) || !File.Exists(archiveFile))
        {
            BadgeAtom.RenderError("FAIL");
            Console.WriteLine(i18n.Format("FileNotFound", archiveFile));
            return;
        }

        bool isValid = engine.VerifyIntegrity(archiveFile, options.Password);

        if (isValid)
        {
            BadgeAtom.RenderSuccess("PASS");
            Console.WriteLine(i18n.GetString("VerificationSuccess"));
        }
        else
        {
            BadgeAtom.RenderError("FAIL");
            Console.WriteLine(i18n.GetString("VerificationFailed"));
        }
    }

    private static string GetArchiveFile(CommandOptions options)
    {
        if (!string.IsNullOrEmpty(options.SourceFile)) return options.SourceFile;
        if (!string.IsNullOrEmpty(options.OutputFile)) return options.OutputFile;
        if (options.InputFiles.Count > 0) return options.InputFiles[0];
        return string.Empty;
    }
}
