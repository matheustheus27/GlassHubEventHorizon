using GlassHub.EventHorizon.CLI.Components.Atoms;
using GlassHub.EventHorizon.Core.Models;

namespace GlassHub.EventHorizon.CLI.Components.Molecules;

public static class FileMetadataMolecule
{
    public static void Render(ArchiveMetadata metadata, string headerTitle)
    {
        BadgeAtom.RenderInfo("METADATA");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(headerTitle);
        Console.ResetColor();
        DividerAtom.Render(50, '┄');

        Console.WriteLine($"  📄 Name             : {metadata.Name}");
        Console.WriteLine($"  📦 Format           : .{metadata.Format}");
        Console.WriteLine($"  💾 Compressed Size  : {FormatBytes(metadata.CompressedSize)}");
        Console.WriteLine($"  📂 Uncompressed Size: {FormatBytes(metadata.UncompressedSize)}");
        Console.WriteLine($"  📊 Compression Ratio: {metadata.CompressionRatio}%");
        Console.WriteLine($"  🔢 Entries Count   : {metadata.EntryCount}");
        Console.WriteLine($"  🔒 Password Encrypted: {(metadata.IsEncrypted ? "YES [AES-256]" : "NO")}");
        Console.WriteLine($"  ⚙️ Engine Applied   : {metadata.EngineUsed}");
        Console.WriteLine($"  🕒 Timestamp        : {metadata.CreatedAt:yyyy-MM-dd HH:mm:ss}");

        DividerAtom.Render(50, '┄');
    }

    private static string FormatBytes(long bytes)
    {
        string[] suffix = { "B", "KB", "MB", "GB", "TB" };
        int i;
        double dblSByte = bytes;
        for (i = 0; i < suffix.Length && bytes >= 1024; i++, bytes /= 1024)
        {
            dblSByte = bytes / 1024.0;
        }
        return $"{dblSByte:0.##} {suffix[i]}";
    }
}
