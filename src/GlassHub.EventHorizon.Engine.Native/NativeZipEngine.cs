using System.IO.Compression;
using GlassHub.EventHorizon.Core.Interfaces;
using GlassHub.EventHorizon.Core.Models;

namespace GlassHub.EventHorizon.Engine.Native;

/// <summary>
/// Native .NET Archive Engine utilizing System.IO.Compression.
/// Provides zero-dependency ZIP archive creation, extraction, listing, and validation.
/// </summary>
public sealed class NativeZipEngine : IArchiveEngine
{
    public string EngineName => "Native .NET ZipEngine (System.IO.Compression)";

    public bool CanHandle(string archivePathOrExtension)
    {
        if (string.IsNullOrWhiteSpace(archivePathOrExtension))
            return false;

        string ext = Path.GetExtension(archivePathOrExtension).ToLowerInvariant();
        return ext == ".zip" || ext == ".zipx" || archivePathOrExtension.EndsWith(".zip", StringComparison.OrdinalIgnoreCase);
    }

    public void Compress(IEnumerable<string> sourcePaths, string outputPath, string? password = null)
    {
        if (sourcePaths is null || !sourcePaths.Any())
            throw new ArgumentException("At least one input file or directory must be specified.");

        if (File.Exists(outputPath))
        {
            File.Delete(outputPath);
        }

        string? parentDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(parentDir) && !Directory.Exists(parentDir))
        {
            Directory.CreateDirectory(parentDir);
        }

        using var zipStream = new FileStream(outputPath, FileMode.CreateNew, FileAccess.Write, FileShare.None);
        using var archive = new ZipArchive(zipStream, ZipArchiveMode.Create);

        foreach (string sourcePath in sourcePaths)
        {
            if (File.Exists(sourcePath))
            {
                string entryName = Path.GetFileName(sourcePath);
                archive.CreateEntryFromFile(sourcePath, entryName, CompressionLevel.Optimal);
            }
            else if (Directory.Exists(sourcePath))
            {
                AddDirectoryToArchive(archive, sourcePath, Path.GetFileName(sourcePath));
            }
            else
            {
                throw new FileNotFoundException($"Input path not found: {sourcePath}");
            }
        }
    }

    private static void AddDirectoryToArchive(ZipArchive archive, string sourceDir, string entryPrefix)
    {
        foreach (string file in Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories))
        {
            string relativePath = Path.GetRelativePath(sourceDir, file);
            string entryName = Path.Combine(entryPrefix, relativePath).Replace('\\', '/');
            archive.CreateEntryFromFile(file, entryName, CompressionLevel.Optimal);
        }
    }

    public void Decompress(string sourcePath, string outputPath, string? password = null)
    {
        if (!File.Exists(sourcePath))
            throw new FileNotFoundException($"Archive target file not found: {sourcePath}");

        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        ZipFile.ExtractToDirectory(sourcePath, outputPath, overwriteFiles: true);
    }

    public IReadOnlyList<string> ListContents(string archivePath, string? password = null)
    {
        if (!File.Exists(archivePath))
            throw new FileNotFoundException($"Archive file not found: {archivePath}");

        using var archive = ZipFile.OpenRead(archivePath);
        return archive.Entries.Select(e => e.FullName).ToList();
    }

    public ArchiveMetadata GetMetadata(string archivePath, string? password = null)
    {
        var fileInfo = new FileInfo(archivePath);
        if (!fileInfo.Exists)
            throw new FileNotFoundException($"Archive file not found: {archivePath}");

        long uncompressedSize = 0;
        int entryCount = 0;

        try
        {
            using var archive = ZipFile.OpenRead(archivePath);
            entryCount = archive.Entries.Count;
            uncompressedSize = archive.Entries.Sum(e => e.Length);
        }
        catch
        {
            // If reading details fails, fall back to basic info
        }

        return new ArchiveMetadata
        {
            Name = fileInfo.Name,
            Format = fileInfo.Extension.TrimStart('.').ToLowerInvariant(),
            CompressedSize = fileInfo.Length,
            UncompressedSize = uncompressedSize,
            EntryCount = entryCount,
            CreatedAt = fileInfo.CreationTime,
            IsEncrypted = !string.IsNullOrEmpty(password),
            EngineUsed = EngineName
        };
    }

    public bool VerifyIntegrity(string archivePath, string? password = null)
    {
        if (!File.Exists(archivePath))
            return false;

        try
        {
            using var archive = ZipFile.OpenRead(archivePath);
            foreach (var entry in archive.Entries)
            {
                using var stream = entry.Open();
                byte[] buffer = new byte[8192];
                while (stream.Read(buffer, 0, buffer.Length) > 0)
                {
                    // Read through to verify stream CRC & uncompressed data integrity
                }
            }
            return true;
        }
        catch
        {
            return false;
        }
    }
}
