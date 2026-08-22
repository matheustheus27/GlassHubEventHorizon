using GlassHub.EventHorizon.CLI.Components.Atoms;
using GlassHub.EventHorizon.CLI.Parsing;

namespace GlassHub.EventHorizon.CLI.Components.Molecules;

public static class CommandOptionsMolecule
{
    public static void Render(CommandOptions options)
    {
        BadgeAtom.RenderInfo("PARAMS");
        Console.WriteLine($"Command: {options.Command}");
        if (options.InputFiles.Count > 0)
        {
            Console.WriteLine($"  Inputs      : {string.Join(", ", options.InputFiles)}");
        }
        if (!string.IsNullOrEmpty(options.OutputFile))
        {
            Console.WriteLine($"  Output      : {options.OutputFile}");
        }
        if (!string.IsNullOrEmpty(options.SourceFile))
        {
            Console.WriteLine($"  Source      : {options.SourceFile}");
        }
        if (!string.IsNullOrEmpty(options.DestinationDirectory))
        {
            Console.WriteLine($"  Destination : {options.DestinationDirectory}");
        }
        if (!string.IsNullOrEmpty(options.Password))
        {
            Console.WriteLine($"  Password    : [PROTECTED]");
        }
    }
}
