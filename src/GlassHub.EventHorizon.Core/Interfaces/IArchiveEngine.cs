using GlassHub.EventHorizon.Core.Models;

namespace GlassHub.EventHorizon.Core.Interfaces;

/// <summary>
/// Domain contract for stream and file-based compression engines.
/// </summary>
public interface IArchiveEngine
{
    /// <summary>
    /// Gets the human-readable identifier of the engine implementation.
    /// </summary>
    string EngineName { get; }

    /// <summary>
    /// Checks whether this engine supports the specified archive format or file path.
    /// </summary>
    bool CanHandle(string archivePathOrExtension);

    /// <summary>
    /// Compresses a set of source files/folders into a target output archive.
    /// </summary>
    void Compress(
        IEnumerable<string> sourcePaths,
        string outputPath,
        string? password = null);

    /// <summary>
    /// Decompresses a target archive into an output destination directory.
    /// </summary>
    void Decompress(
        string sourcePath,
        string outputPath,
        string? password = null);

    /// <summary>
    /// Lists all file entries stored inside an archive.
    /// </summary>
    IReadOnlyList<string> ListContents(
        string archivePath,
        string? password = null);

    /// <summary>
    /// Reads technical metadata and telemetry for an archive.
    /// </summary>
    ArchiveMetadata GetMetadata(
        string archivePath,
        string? password = null);

    /// <summary>
    /// Verifies the structural integrity and CRC of an archive.
    /// </summary>
    bool VerifyIntegrity(
        string archivePath,
        string? password = null);
}