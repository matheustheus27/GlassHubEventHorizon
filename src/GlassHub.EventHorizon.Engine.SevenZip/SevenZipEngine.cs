using System.Diagnostics;

using GlassHub.EventHorizon.Core.Interfaces;
using GlassHub.EventHorizon.Core.Models;

namespace GlassHub.EventHorizon.Engine.SevenZip;

/// <summary>
/// 7-Zip CLI binary engine wrapper supporting 7z, ZIP, TAR, GZ, RAR formats and AES encryption.
/// </summary>
public sealed class SevenZipEngine : IArchiveEngine
{
    private readonly string _sevenZipPath;

    public string EngineName => "7-Zip CLI Archive Engine";

    public SevenZipEngine(string? customSevenZipPath = null)
    {
        _sevenZipPath = customSevenZipPath 
            ?? FindSystemSevenZip() 
            ?? Path.Combine(AppContext.BaseDirectory, "binaries", "7z.exe");
    }

    public bool IsAvailable => File.Exists(_sevenZipPath);

    private static string? FindSystemSevenZip()
    {
        string[] candidatePaths =
        [
            @"C:\Program Files\7-Zip\7z.exe",
            @"C:\Program Files (x86)\7-Zip\7z.exe",
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "7-Zip", "7z.exe")
        ];

        foreach (string path in candidatePaths)
        {
            if (File.Exists(path))
                return path;
        }

        return null;
    }

    public bool CanHandle(string archivePathOrExtension)
    {
        if (string.IsNullOrWhiteSpace(archivePathOrExtension))
            return false;

        string ext = Path.GetExtension(archivePathOrExtension).ToLowerInvariant();
        return ext is ".7z" or ".zip" or ".tar" or ".gz" or ".rar" or ".bz2" or ".xz";
    }

    public void Compress(IEnumerable<string> sourcePaths, string outputPath, string? password = null)
    {
        if (!IsAvailable)
            throw new FileNotFoundException($"7-Zip binary not found at path: {_sevenZipPath}");

        if (sourcePaths is null || !sourcePaths.Any())
            throw new ArgumentException("At least one input path must be specified.");

        string inputArguments = string.Join(" ", sourcePaths.Select(path => $"\"{path}\""));
        string formatFlag = outputPath.EndsWith(".zip", StringComparison.OrdinalIgnoreCase) ? "-tzip" : "-t7z";
        string arguments = $"a \"{outputPath}\" {inputArguments} {formatFlag}";

        if (!string.IsNullOrWhiteSpace(password))
        {
            arguments += $" -p\"{password}\" -mhe=on";
        }

        ExecuteProcess(arguments);
    }

    public void Decompress(string sourcePath, string outputPath, string? password = null)
    {
        if (!IsAvailable)
            throw new FileNotFoundException($"7-Zip binary not found at path: {_sevenZipPath}");

        if (!File.Exists(sourcePath))
            throw new FileNotFoundException($"Archive target not found: {sourcePath}");

        string arguments = $"x \"{sourcePath}\" -o\"{outputPath}\" -y";

        if (!string.IsNullOrEmpty(password))
        {
            arguments += $" -p\"{password}\"";
        }

        ExecuteProcess(arguments);
    }

    public IReadOnlyList<string> ListContents(string archivePath, string? password = null)
    {
        if (!IsAvailable)
            throw new FileNotFoundException($"7-Zip binary not found at path: {_sevenZipPath}");

        string arguments = $"l \"{archivePath}\"";

        if (!string.IsNullOrWhiteSpace(password))
        {
            arguments += $" -p\"{password}\"";
        }

        string output = ExecuteProcessAndCaptureOutput(arguments);

        return output
            .Split(Environment.NewLine)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .ToList();
    }

    public ArchiveMetadata GetMetadata(string archivePath, string? password = null)
    {
        var fileInfo = new FileInfo(archivePath);

        return new ArchiveMetadata
        {
            Name = fileInfo.Name,
            Format = fileInfo.Extension.TrimStart('.').ToLowerInvariant(),
            CompressedSize = fileInfo.Length,
            UncompressedSize = fileInfo.Length,
            EntryCount = 1,
            CreatedAt = fileInfo.CreationTime,
            IsEncrypted = !string.IsNullOrEmpty(password),
            EngineUsed = EngineName
        };
    }

    public bool VerifyIntegrity(string archivePath, string? password = null)
    {
        if (!IsAvailable)
            return false;

        string arguments = $"t \"{archivePath}\"";

        if (!string.IsNullOrWhiteSpace(password))
        {
            arguments += $" -p\"{password}\"";
        }

        try
        {
            ExecuteProcess(arguments);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private void ExecuteProcess(string arguments)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = _sevenZipPath,
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new Process { StartInfo = startInfo };
        process.Start();
        string errors = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            throw new Exception($"7-Zip process exited with code {process.ExitCode}. Error details: {errors}");
        }
    }

    private string ExecuteProcessAndCaptureOutput(string arguments)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = _sevenZipPath,
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new Process { StartInfo = startInfo };
        process.Start();
        string output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();

        return output;
    }
}