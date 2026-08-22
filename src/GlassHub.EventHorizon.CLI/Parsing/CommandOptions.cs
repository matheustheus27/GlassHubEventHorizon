namespace GlassHub.EventHorizon.CLI.Parsing;

public sealed class CommandOptions
{
    public string Command { get; set; } = "help";
    public List<string> InputFiles { get; set; } = new();
    public string OutputFile { get; set; } = string.Empty;
    public string SourceFile { get; set; } = string.Empty;
    public string DestinationDirectory { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
}