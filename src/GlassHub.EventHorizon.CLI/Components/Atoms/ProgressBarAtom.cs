namespace GlassHub.EventHorizon.CLI.Components.Atoms;

public static class ProgressBarAtom
{
    public static void Render(double percentage, string label = "")
    {
        int totalBlocks = 30;
        int filledBlocks = (int)Math.Round((percentage / 100.0) * totalBlocks);
        filledBlocks = Math.Clamp(filledBlocks, 0, totalBlocks);

        string filled = new string('█', filledBlocks);
        string empty = new string('░', totalBlocks - filledBlocks);

        Console.Write("\r[");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write(filled);
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write(empty);
        Console.ResetColor();
        Console.Write($"] {percentage:F1}% {label}");

        if (percentage >= 100.0)
        {
            Console.WriteLine();
        }
    }
}
