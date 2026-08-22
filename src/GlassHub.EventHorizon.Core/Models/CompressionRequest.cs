namespace GlassHub.EventHorizon.Core.Models;

/// <summary>
/// Encapsulates parameters for a compression job request.
/// </summary>
public sealed class CompressionRequest
{
    public List<string> SourcePaths { get; set; } = new();
    public string OutputPath { get; set; } = string.Empty;
    public string? Password { get; set; }
    public string Format { get; set; } = "zip";
}