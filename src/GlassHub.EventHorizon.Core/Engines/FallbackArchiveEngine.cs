using GlassHub.EventHorizon.Core.Interfaces;
using GlassHub.EventHorizon.Core.Models;

namespace GlassHub.EventHorizon.Core.Engines;

/// <summary>
/// Composite Fallback Archive Engine.
/// Dynamically routes compression/decompression requests to 7-Zip when available,
/// seamlessly falling back to the Native .NET Zip Engine for 100% out-of-the-box reliability.
/// </summary>
public sealed class FallbackArchiveEngine : IArchiveEngine
{
    private readonly IArchiveEngine _nativeEngine;
    private readonly IArchiveEngine _sevenZipEngine;

    public string EngineName => "GlassHub Smart Fallback Engine (7-Zip + Native Zip)";

    public FallbackArchiveEngine(IArchiveEngine nativeEngine, IArchiveEngine sevenZipEngine)
    {
        _nativeEngine = nativeEngine;
        _sevenZipEngine = sevenZipEngine;
    }

    public bool CanHandle(string archivePathOrExtension)
    {
        return _nativeEngine.CanHandle(archivePathOrExtension) || _sevenZipEngine.CanHandle(archivePathOrExtension);
    }

    private IArchiveEngine SelectEngine(string pathOrExt, bool requiresPassword = false)
    {
        if (requiresPassword)
        {
            return _sevenZipEngine;
        }

        if (_sevenZipEngine.CanHandle(pathOrExt))
        {
            try
            {
                if (_sevenZipEngine.VerifyIntegrity(pathOrExt))
                {
                    return _sevenZipEngine;
                }
            }
            catch
            {
                // Fall back to native if 7-Zip check fails
            }
        }

        return _nativeEngine;
    }

    public void Compress(IEnumerable<string> sourcePaths, string outputPath, string? password = null)
    {
        bool usePass = !string.IsNullOrEmpty(password);
        var engine = usePass ? _sevenZipEngine : (outputPath.EndsWith(".7z", StringComparison.OrdinalIgnoreCase) ? _sevenZipEngine : _nativeEngine);

        try
        {
            engine.Compress(sourcePaths, outputPath, password);
        }
        catch when (!usePass && engine == _sevenZipEngine)
        {
            // Fallback attempt to Native engine if 7-Zip CLI process is missing or failed
            _nativeEngine.Compress(sourcePaths, outputPath, password);
        }
    }

    public void Decompress(string sourcePath, string outputPath, string? password = null)
    {
        var engine = SelectEngine(sourcePath, !string.IsNullOrEmpty(password));

        try
        {
            engine.Decompress(sourcePath, outputPath, password);
        }
        catch
        {
            _nativeEngine.Decompress(sourcePath, outputPath, password);
        }
    }

    public IReadOnlyList<string> ListContents(string archivePath, string? password = null)
    {
        try
        {
            return _nativeEngine.ListContents(archivePath, password);
        }
        catch
        {
            return _sevenZipEngine.ListContents(archivePath, password);
        }
    }

    public ArchiveMetadata GetMetadata(string archivePath, string? password = null)
    {
        try
        {
            return _nativeEngine.GetMetadata(archivePath, password);
        }
        catch
        {
            return _sevenZipEngine.GetMetadata(archivePath, password);
        }
    }

    public bool VerifyIntegrity(string archivePath, string? password = null)
    {
        return _nativeEngine.VerifyIntegrity(archivePath, password) || _sevenZipEngine.VerifyIntegrity(archivePath, password);
    }
}
