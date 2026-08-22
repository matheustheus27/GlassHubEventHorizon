using System.Windows;
using GlassHub.EventHorizon.Core.Engines;
using GlassHub.EventHorizon.Core.Interfaces;
using GlassHub.EventHorizon.Core.Localization;
using GlassHub.EventHorizon.Engine.Native;
using GlassHub.EventHorizon.Engine.SevenZip;

namespace GlassHub.EventHorizon.GUI;

public partial class MainWindow : Window
{
    private readonly IArchiveEngine _engine;
    private readonly ILocalizationService _i18n;

    public MainWindow()
    {
        InitializeComponent();

        _i18n = new LocalizationService();
        _engine = new FallbackArchiveEngine(new NativeZipEngine(), new SevenZipEngine());

        UpdateLocalizationUI();
    }

    private void UpdateLocalizationUI()
    {
        TxtStatus.Text = $"Status: {_i18n.GetString("Completed")} | {_i18n.GetString("EngineFallback")}";
    }

    private void BtnLangEn_Click(object sender, RoutedEventArgs e)
    {
        _i18n.SetCulture("en-US");
        UpdateLocalizationUI();
    }

    private void BtnLangPt_Click(object sender, RoutedEventArgs e)
    {
        _i18n.SetCulture("pt-BR");
        UpdateLocalizationUI();
    }

    private void BtnCompress_Click(object sender, RoutedEventArgs e)
    {
        string source = TxtSourcePath.Text.Trim();
        string output = TxtOutputPath.Text.Trim();
        string pass = TxtCompressPassword.Password;

        if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(output))
        {
            MessageBox.Show(_i18n.GetString("NoInput"), "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            _engine.Compress(new[] { source }, output, pass);
            TxtStatus.Text = $"Status: {output} compressed successfully!";
            MessageBox.Show(_i18n.GetString("Completed"), "GlassHubEventHorizon", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"{_i18n.GetString("Failed")}: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void BtnExtract_Click(object sender, RoutedEventArgs e)
    {
        string source = TxtExtractSource.Text.Trim();
        string dest = TxtExtractDest.Text.Trim();
        string pass = TxtExtractPassword.Password;

        if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(dest))
        {
            MessageBox.Show(_i18n.GetString("NoDestination"), "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            _engine.Decompress(source, dest, pass);
            TxtStatus.Text = $"Status: Extracted to {dest} successfully!";
            MessageBox.Show(_i18n.GetString("Completed"), "GlassHubEventHorizon", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"{_i18n.GetString("Failed")}: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
