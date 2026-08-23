using System.Windows;
using System.Windows.Media;

namespace GlassHub.EventHorizon.GUI;

public static class ThemeManager
{
    public static string CurrentTheme { get; private set; } = "Glass Dark";

    public static void ApplyTheme(string themeName)
    {
        CurrentTheme = themeName;
        var res = Application.Current.Resources;

        switch (themeName)
        {
            case "Fluent Light": // Windows 11 Light Theme
                SetResourceColor(res, "BackgroundDarkColor", "#F3F3F7");
                SetResourceColor(res, "NavBackgroundDarkColor", "#E8E8EE");
                SetResourceColor(res, "CardGlassBackground", "#FFFFFF");
                SetResourceColor(res, "PopupBackgroundColor", "#FFFFFF");
                SetResourceColor(res, "GlassBorderColor", "#D1D5DB");
                SetResourceColor(res, "TextPrimaryColor", "#111827");
                SetResourceColor(res, "TextSecondaryColor", "#4B5563");
                SetResourceColor(res, "NeonCyanColor", "#0284C7");
                SetResourceColor(res, "NeonMagentaColor", "#2563EB");
                SetResourceColor(res, "DropZoneBackgroundColor", "#F9FAFB");
                SetResourceColor(res, "ControlBackgroundColor", "#E5E7EB");
                break;

            case "WinRAR Cyber": // Dark Metal Cyber Style
                SetResourceColor(res, "BackgroundDarkColor", "#181A1F");
                SetResourceColor(res, "NavBackgroundDarkColor", "#14161A");
                SetResourceColor(res, "CardGlassBackground", "#21252B");
                SetResourceColor(res, "PopupBackgroundColor", "#282C34");
                SetResourceColor(res, "GlassBorderColor", "#3B4048");
                SetResourceColor(res, "TextPrimaryColor", "#ABB2BF");
                SetResourceColor(res, "TextSecondaryColor", "#5C6370");
                SetResourceColor(res, "NeonCyanColor", "#98C379");
                SetResourceColor(res, "NeonMagentaColor", "#61AFEF");
                SetResourceColor(res, "DropZoneBackgroundColor", "#1B1E24");
                SetResourceColor(res, "ControlBackgroundColor", "#2C313A");
                break;

            case "Neon Cyberpunk": // Vibrant Cyberpunk Style
                SetResourceColor(res, "BackgroundDarkColor", "#0B0813");
                SetResourceColor(res, "NavBackgroundDarkColor", "#07050E");
                SetResourceColor(res, "CardGlassBackground", "#15102A");
                SetResourceColor(res, "PopupBackgroundColor", "#1D1737");
                SetResourceColor(res, "GlassBorderColor", "#FF007F");
                SetResourceColor(res, "TextPrimaryColor", "#FFFFFF");
                SetResourceColor(res, "TextSecondaryColor", "#A9A5C3");
                SetResourceColor(res, "NeonCyanColor", "#00F2FE");
                SetResourceColor(res, "NeonMagentaColor", "#FF007F");
                SetResourceColor(res, "DropZoneBackgroundColor", "#100B22");
                SetResourceColor(res, "ControlBackgroundColor", "#231B44");
                break;

            case "Glass Dark": // VS Code / GitHub Dark
                SetResourceColor(res, "BackgroundDarkColor", "#0D1117");
                SetResourceColor(res, "NavBackgroundDarkColor", "#090D13");
                SetResourceColor(res, "CardGlassBackground", "#161B22");
                SetResourceColor(res, "PopupBackgroundColor", "#21262D");
                SetResourceColor(res, "GlassBorderColor", "#30363D");
                SetResourceColor(res, "TextPrimaryColor", "#F0F6FC");
                SetResourceColor(res, "TextSecondaryColor", "#8B949E");
                SetResourceColor(res, "NeonCyanColor", "#58A6FF");
                SetResourceColor(res, "NeonMagentaColor", "#7EE787");
                SetResourceColor(res, "DropZoneBackgroundColor", "#090D13");
                SetResourceColor(res, "ControlBackgroundColor", "#21262D");
                break;

            case "GlassHub Cosmic Dark": // GlassHub Cosmic Glassmorphism Signature Theme
            default:
                SetResourceColor(res, "BackgroundDarkColor", "#070B13");
                SetResourceColor(res, "NavBackgroundDarkColor", "#05080E");
                SetResourceColor(res, "CardGlassBackground", "#0E1626");
                SetResourceColor(res, "PopupBackgroundColor", "#111C30");
                SetResourceColor(res, "GlassBorderColor", "#2600E5FF");
                SetResourceColor(res, "TextPrimaryColor", "#F0F6FC");
                SetResourceColor(res, "TextSecondaryColor", "#8B9BB4");
                SetResourceColor(res, "NeonCyanColor", "#00E5FF");
                SetResourceColor(res, "NeonMagentaColor", "#8054FF");
                SetResourceColor(res, "DropZoneBackgroundColor", "#090F1B");
                SetResourceColor(res, "ControlBackgroundColor", "#111C30");
                break;
        }
    }

    private static void SetResourceColor(ResourceDictionary res, string key, string hexColor)
    {
        var color = (Color)ColorConverter.ConvertFromString(hexColor);
        res[key] = color;
        res[key.Replace("Color", "Brush")] = new SolidColorBrush(color);
    }
}
