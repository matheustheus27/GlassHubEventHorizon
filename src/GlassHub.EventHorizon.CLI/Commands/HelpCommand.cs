using GlassHub.EventHorizon.CLI.Components.Atoms;
using GlassHub.EventHorizon.Core.Localization;

namespace GlassHub.EventHorizon.CLI.Commands;

public static class HelpCommand
{
    public static void Execute(ILocalizationService i18n)
    {
        BadgeAtom.RenderInfo("USAGE");
        Console.WriteLine("evh <command|flag> [options]");
        Console.WriteLine();

        BadgeAtom.RenderInfo("COMMANDS & ALIASES");
        Console.WriteLine("  compress, c, --compress, -c   Compress files or directories into a target archive");
        Console.WriteLine("  extract,  x, --extract,  -x   Extract an archive into a destination directory");
        Console.WriteLine("  list,        --list           List internal archive entries and file hierarchy");
        Console.WriteLine("  info,        --info           Display technical telemetry, format, ratio, and engine");
        Console.WriteLine("  verify,   v, --verify,   -v   Test structural integrity and CRC of an archive");
        Console.WriteLine("  help,     h, --help,     -h   Show this comprehensive help manual");
        Console.WriteLine();

        BadgeAtom.RenderInfo("PARAMETER OPTIONS");
        Console.WriteLine("  -i, --input <path>     Input source file or directory path");
        Console.WriteLine("  -o, --output <path>    Target output archive path (.zip / .7z)");
        Console.WriteLine("  -f, --file <path>      Target archive file for extraction/inspection");
        Console.WriteLine("  -d, --dest <path>      Destination directory for extracted content");
        Console.WriteLine("  -p, --password <pass>  Optional password protection (AES-256)");
        Console.WriteLine("      --lang <en|pt>     Dynamic language selection (en-US / pt-BR)");
        Console.WriteLine();

        BadgeAtom.RenderInfo("EXAMPLES");
        Console.WriteLine("  evh compress -i data/ -o backup.zip");
        Console.WriteLine("  evh -c -i data/ -o backup.zip");
        Console.WriteLine("  evh extract -f backup.zip -d output/");
        Console.WriteLine("  evh -x -f backup.zip -d output/");
        Console.WriteLine("  evh info -f backup.zip");
        Console.WriteLine("  evh --info -f backup.zip");
        Console.WriteLine("  evh verify -f backup.zip");
        Console.WriteLine("  evh -v -f backup.zip");
    }
}