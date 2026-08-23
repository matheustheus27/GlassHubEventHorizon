using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
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
    private readonly ObservableCollection<string> _stagedPaths = new();

    public MainWindow()
    {
        InitializeComponent();

        _i18n = new LocalizationService();
        _engine = new FallbackArchiveEngine(new NativeZipEngine(), new SevenZipEngine());

        LstStagedItems.ItemsSource = _stagedPaths;
        ThemeManager.ApplyTheme("Glass Dark");
        UpdateLocalizationUI();
    }

    private void UpdateLocalizationUI()
    {
        // Headers & Tabs
        TxtTabCompressHeader.Text = _i18n.GetString("TabCompress").Replace("📦 ", "");
        TxtTabExtractHeader.Text = _i18n.GetString("TabExtract").Replace("📂 ", "");
        TxtTabTelemetryHeader.Text = _i18n.GetString("TabTelemetry").Replace("📊 ", "");

        // Compress Tab
        TxtDragDropTitle.Text = _i18n.GetString("DragDropTitle");
        TxtDragDropSubtitle.Text = _i18n.GetString("DragDropSubtitle");
        TxtBtnSelectUnified.Text = _i18n.GetString("BtnSelectUnified");
        BtnClearList.Content = _i18n.GetString("BtnClearList");
        UpdateStagedHeader();

        TxtFormatLabel.Text = _i18n.GetString("FormatLabel");
        TxtCompressionLevelLabel.Text = _i18n.GetString("CompressionLevelLabel");
        TxtCompressPasswordLabel.Text = _i18n.GetString("PasswordLabel");
        ExpCompressOptions.Header = _i18n.GetString("AdvancedOptions");
        BtnCompress.Content = _i18n.GetString("BtnCompressAction");

        // Extract Tab
        TxtDragDropExtractTitle.Text = _i18n.GetString("DragDropExtractTitle");
        TxtDragDropExtractSubtitle.Text = _i18n.GetString("DragDropExtractSubtitle");
        TxtArchiveSelectLabel.Text = _i18n.GetString("ArchiveSelectLabel");
        TxtDestFolderLabel.Text = _i18n.GetString("DestFolderLabel");
        TxtExtractPasswordLabel.Text = _i18n.GetString("PasswordLabel");
        ExpExtractOptions.Header = _i18n.GetString("AdvancedOptions");
        BtnExtract.Content = _i18n.GetString("BtnExtractAction");

        // Settings Modal
        TxtSettingsTitle.Text = _i18n.GetString("SettingsTitle");
        TxtThemeLabel.Text = _i18n.GetString("ThemeLabel");
        TxtLanguageLabel.Text = _i18n.GetString("LanguageLabel");
        TxtEngineSelectLabel.Text = _i18n.GetString("EngineSelectLabel");
        BtnSaveSettings.Content = _i18n.GetString("SaveAndClose");

        // Footer
        TxtStatus.Text = $"{_i18n.GetString("StatusReady")} | {_i18n.GetString("EngineFallback")}";
    }

    // Custom Window Chrome Handlers
    private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
            DragMove();
    }

    private void BtnMinimize_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void BtnMaximize_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
    }

    private void BtnClose_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void UpdateStagedHeader()
    {
        TxtStagedHeader.Text = _i18n.Format("StagedItemsHeader", _stagedPaths.Count);
    }

    private string GetSelectedFormatExtension()
    {
        return CmbFormat.SelectedIndex switch
        {
            1 => ".7z",
            2 => ".tar",
            3 => ".gz",
            _ => ".zip"
        };
    }

    // Drag and Drop Logic - Compress Tab
    private void Window_DragOver(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
            e.Effects = DragDropEffects.Copy;
        else
            e.Effects = DragDropEffects.None;
        e.Handled = true;
    }

    private void Window_Drop(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                if (!_stagedPaths.Contains(file))
                    _stagedPaths.Add(file);
            }
            UpdateStagedHeader();
        }
    }

    private void DropZone_DragOver(object sender, DragEventArgs e) => Window_DragOver(sender, e);
    private void DropZone_Drop(object sender, DragEventArgs e) => Window_Drop(sender, e);

    // Drag and Drop Logic - Extract Tab
    private void DropZoneExtract_DragOver(object sender, DragEventArgs e) => Window_DragOver(sender, e);

    private void DropZoneExtract_Drop(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length > 0)
            {
                string path = files[0];
                TxtExtractSource.Text = path;
                string parentDir = Path.GetDirectoryName(path) ?? "";
                string folderName = Path.GetFileNameWithoutExtension(path);
                TxtExtractDest.Text = Path.Combine(parentDir, folderName + "_extracted");
            }
        }
    }

    // Staging List Management & Unified Selection
    private void BtnAddFiles_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog dlg = new()
        {
            Multiselect = true,
            Title = "Selecionar Arquivos ou Pasta para Compactar"
        };

        if (dlg.ShowDialog() == true)
        {
            foreach (string file in dlg.FileNames)
            {
                if (!_stagedPaths.Contains(file))
                    _stagedPaths.Add(file);
            }
            UpdateStagedHeader();
        }
    }

    private void BtnClearList_Click(object sender, RoutedEventArgs e)
    {
        _stagedPaths.Clear();
        UpdateStagedHeader();
    }

    private void BtnRemoveItem_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is string item)
        {
            _stagedPaths.Remove(item);
            UpdateStagedHeader();
        }
    }

    private void CmbFormat_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
    }

    // Compression Execution
    private void BtnCompress_Click(object sender, RoutedEventArgs e)
    {
        if (_stagedPaths.Count == 0)
        {
            MessageBox.Show(_i18n.GetString("NoInput"), "GlassHub Event Horizon", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        string first = _stagedPaths[0];
        string baseDir = Directory.Exists(first) ? Path.GetDirectoryName(first)! : Path.GetDirectoryName(first)!;
        string baseName = Directory.Exists(first) ? new DirectoryInfo(first).Name : Path.GetFileNameWithoutExtension(first);

        if (string.IsNullOrEmpty(baseDir))
        {
            baseDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }

        string ext = GetSelectedFormatExtension();
        string output = Path.Combine(baseDir, $"{baseName}_archive{ext}");
        string pass = TxtCompressPassword.Password;

        try
        {
            _engine.Compress(_stagedPaths, output, pass);
            TxtStatus.Text = _i18n.Format("StatusCompressSuccess", output);
            MessageBox.Show(_i18n.GetString("Completed"), "GlassHub Event Horizon", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"{_i18n.GetString("Failed")}: {ex.Message}", "GlassHub Event Horizon", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    // Extract Tab Actions
    private void BtnBrowseExtractSource_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog dlg = new()
        {
            Filter = "Archive Files (*.zip;*.7z;*.rar;*.tar;*.gz)|*.zip;*.7z;*.rar;*.tar;*.gz|All Files (*.*)|*.*",
            Title = "Selecionar Arquivo Compactado"
        };

        if (dlg.ShowDialog() == true)
        {
            TxtExtractSource.Text = dlg.FileName;
            string parentDir = Path.GetDirectoryName(dlg.FileName) ?? "";
            string folderName = Path.GetFileNameWithoutExtension(dlg.FileName);
            TxtExtractDest.Text = Path.Combine(parentDir, folderName + "_extracted");
        }
    }

    private void BtnBrowseExtractDest_Click(object sender, RoutedEventArgs e)
    {
        OpenFolderDialog dlg = new()
        {
            Title = "Escolher Pasta de Destino para Extração"
        };

        if (dlg.ShowDialog() == true)
        {
            TxtExtractDest.Text = dlg.FolderName;
        }
    }

    private void BtnExtract_Click(object sender, RoutedEventArgs e)
    {
        string source = TxtExtractSource.Text.Trim();
        string dest = TxtExtractDest.Text.Trim();
        string pass = TxtExtractPassword.Password;

        if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(dest))
        {
            MessageBox.Show(_i18n.GetString("NoDestination"), "GlassHub Event Horizon", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            _engine.Decompress(source, dest, pass);
            TxtStatus.Text = _i18n.Format("StatusExtractSuccess", dest);
            MessageBox.Show(_i18n.GetString("Completed"), "GlassHub Event Horizon", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"{_i18n.GetString("Failed")}: {ex.Message}", "GlassHub Event Horizon", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    // Telemetry Tab Actions
    private void BtnBrowseTelemetrySource_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog dlg = new()
        {
            Filter = "Archive Files (*.zip;*.7z;*.rar;*.tar;*.gz)|*.zip;*.7z;*.rar;*.tar;*.gz|All Files (*.*)|*.*"
        };

        if (dlg.ShowDialog() == true)
        {
            TxtTelemetrySource.Text = dlg.FileName;
            InspectArchive(dlg.FileName);
        }
    }

    private void BtnInspect_Click(object sender, RoutedEventArgs e)
    {
        string path = TxtTelemetrySource.Text.Trim();
        if (string.IsNullOrEmpty(path) || !File.Exists(path))
        {
            MessageBox.Show("Por favor, selecione um arquivo válido para inspecionar.", "GlassHub Event Horizon", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        InspectArchive(path);
    }

    private void InspectArchive(string path)
    {
        try
        {
            var metadata = _engine.GetMetadata(path);
            var entries = _engine.ListContents(path);
            bool isValid = _engine.VerifyIntegrity(path);

            TxtTelemetryCount.Text = entries.Count.ToString();
            TxtTelemetryUncompressed.Text = $"{metadata.UncompressedSize / 1024.0 / 1024.0:F2} MB";
            TxtTelemetryRatio.Text = $"{metadata.CompressionRatio:F1} %";
            TxtTelemetryIntegrity.Text = isValid ? "PASSED" : "FAILED";
            TxtTelemetryIntegrity.Foreground = isValid
                ? (System.Windows.Media.Brush)FindResource("NeonCyanBrush")
                : (System.Windows.Media.Brush)new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Crimson);

            LstTelemetryEntries.ItemsSource = entries;
            TxtStatus.Text = _i18n.GetString("StatusInspected");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Falha na inspeção de telemetria: {ex.Message}", "GlassHub Event Horizon", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    // Settings Modal & Theme Selection
    private void BtnOpenSettings_Click(object sender, RoutedEventArgs e)
    {
        SettingsOverlay.Visibility = Visibility.Visible;
    }

    private void BtnCloseSettings_Click(object sender, RoutedEventArgs e)
    {
        SettingsOverlay.Visibility = Visibility.Collapsed;
    }

    private void CmbTheme_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!IsInitialized) return;

        string selectedTheme = CmbTheme.SelectedIndex switch
        {
            1 => "Fluent Light",
            2 => "WinRAR Cyber",
            3 => "Neon Cyberpunk",
            _ => "Glass Dark"
        };

        ThemeManager.ApplyTheme(selectedTheme);
    }

    private void BtnLangPt_Click(object sender, RoutedEventArgs e)
    {
        _i18n.SetCulture("pt-BR");
        UpdateLocalizationUI();
    }

    private void BtnLangEn_Click(object sender, RoutedEventArgs e)
    {
        _i18n.SetCulture("en-US");
        UpdateLocalizationUI();
    }
}


