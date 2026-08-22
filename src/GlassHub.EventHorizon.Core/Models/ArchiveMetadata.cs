namespace GlassHub.EventHorizon.Core.Models;

/// <summary>
/// Technical metadata and telemetry describing a compressed archive file.
/// </summary>
public sealed class ArchiveMetadata
{
    public string Name { get; set; } = string.Empty;
    public string Format { get; set; } = string.Empty;
    public long CompressedSize { get; set; }
    public long UncompressedSize { get; set; }
    public int EntryCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsEncrypted { get; set; }
    public string EngineUsed { get; set; } = string.Empty;

    public double CompressionRatio => UncompressedSize > 0
        ? Math.Round((1.0 - ((double)CompressedSize / UncompressedSize)) * 100.0, 2)
        : 0.0;
}