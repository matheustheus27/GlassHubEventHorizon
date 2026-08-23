using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using GlassHub.EventHorizon.Core.Interfaces;
using GlassHub.EventHorizon.Core.Localization;

namespace GlassHub.EventHorizon.GUI.ViewModels;

public class InspectViewModel : ViewModelBase
{
    private readonly IArchiveEngine _engine;
    private readonly ILocalizationService _i18n;
    private readonly Action<string> _updateStatus;

    private string _sourcePath = "";
    private string _searchQuery = "";
    private string _entryCountText = "0";
    private string _uncompressedSizeText = "0.00 MB";
    private string _compressionRatioText = "0.0 %";
    private string _integrityStatusText = "UNCHECKED";
    private bool _isIntegrityPassed = true;
    private bool _isInspected;
    private bool _isBusy;
    private readonly List<string> _allEntries = new();

    public InspectViewModel(IArchiveEngine engine, ILocalizationService i18n, Action<string> updateStatus)
    {
        _engine = engine;
        _i18n = i18n;
        _updateStatus = updateStatus;

        FilteredEntries = new ObservableCollection<string>();

        BrowseSourceCommand = new RelayCommand(ExecuteBrowseSource, () => !IsBusy);
        InspectCommand = new AsyncRelayCommand(ExecuteInspectAsync, () => !IsBusy && !string.IsNullOrWhiteSpace(SourcePath));
        VerifyIntegrityCommand = new AsyncRelayCommand(ExecuteVerifyIntegrityAsync, () => !IsBusy && !string.IsNullOrWhiteSpace(SourcePath));
    }

    public ObservableCollection<string> FilteredEntries { get; }

    public string SourcePath
    {
        get => _sourcePath;
        set
        {
            if (SetProperty(ref _sourcePath, value))
            {
                OnPropertyChanged(nameof(HasSourcePath));
                ((AsyncRelayCommand)InspectCommand).RaiseCanExecuteChanged();
                ((AsyncRelayCommand)VerifyIntegrityCommand).RaiseCanExecuteChanged();
            }
        }
    }

    public bool HasSourcePath => !string.IsNullOrWhiteSpace(SourcePath);

    public string SearchQuery
    {
        get => _searchQuery;
        set
        {
            if (SetProperty(ref _searchQuery, value))
            {
                FilterEntries();
            }
        }
    }

    public string EntryCountText
    {
        get => _entryCountText;
        set => SetProperty(ref _entryCountText, value);
    }

    public string UncompressedSizeText
    {
        get => _uncompressedSizeText;
        set => SetProperty(ref _uncompressedSizeText, value);
    }

    public string CompressionRatioText
    {
        get => _compressionRatioText;
        set => SetProperty(ref _compressionRatioText, value);
    }

    public string IntegrityStatusText
    {
        get => _integrityStatusText;
        set => SetProperty(ref _integrityStatusText, value);
    }

    public bool IsIntegrityPassed
    {
        get => _isIntegrityPassed;
        set => SetProperty(ref _isIntegrityPassed, value);
    }

    public bool IsInspected
    {
        get => _isInspected;
        set => SetProperty(ref _isInspected, value);
    }

    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            if (SetProperty(ref _isBusy, value))
            {
                ((RelayCommand)BrowseSourceCommand).RaiseCanExecuteChanged();
                ((AsyncRelayCommand)InspectCommand).RaiseCanExecuteChanged();
                ((AsyncRelayCommand)VerifyIntegrityCommand).RaiseCanExecuteChanged();
            }
        }
    }

    public ICommand BrowseSourceCommand { get; }
    public ICommand InspectCommand { get; }
    public ICommand VerifyIntegrityCommand { get; }

    public void SetFile(string path)
    {
        if (File.Exists(path))
        {
            SourcePath = path;
            _ = ExecuteInspectAsync();
        }
    }

    private void ExecuteBrowseSource()
    {
        var dlg = new OpenFileDialog
        {
            Filter = "Archive Files (*.zip;*.7z;*.rar;*.tar;*.gz;*.zst)|*.zip;*.7z;*.rar;*.tar;*.gz;*.zst|All Files (*.*)|*.*",
            Title = _i18n.GetString("ArchiveSelectLabel")
        };

        if (dlg.ShowDialog() == true)
        {
            SourcePath = dlg.FileName;
            _ = ExecuteInspectAsync();
        }
    }

    private async Task ExecuteInspectAsync()
    {
        string path = SourcePath.Trim();
        if (string.IsNullOrEmpty(path) || !File.Exists(path))
        {
            MessageBox.Show(_i18n.GetString("FileNotFound"), "GlassHub Event Horizon", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        IsBusy = true;
        _updateStatus($"{_i18n.GetString("BtnInspectAction")}: {Path.GetFileName(path)}");

        try
        {
            await Task.Run(() =>
            {
                var metadata = _engine.GetMetadata(path);
                var entries = _engine.ListContents(path);
                bool valid = _engine.VerifyIntegrity(path);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    _allEntries.Clear();
                    _allEntries.AddRange(entries);
                    FilterEntries();

                    EntryCountText = entries.Count.ToString("N0");
                    UncompressedSizeText = $"{metadata.UncompressedSize / 1024.0 / 1024.0:F2} MB";
                    CompressionRatioText = $"{metadata.CompressionRatio:F1} %";
                    IsIntegrityPassed = valid;
                    IntegrityStatusText = valid ? "PASSED" : "FAILED";
                    IsInspected = true;
                });
            });

            _updateStatus(_i18n.GetString("StatusInspected"));
        }
        catch (Exception ex)
        {
            MessageBox.Show($"{_i18n.GetString("Failed")}: {ex.Message}", "GlassHub Event Horizon", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task ExecuteVerifyIntegrityAsync()
    {
        string path = SourcePath.Trim();
        if (string.IsNullOrEmpty(path) || !File.Exists(path)) return;

        IsBusy = true;
        try
        {
            bool valid = await Task.Run(() => _engine.VerifyIntegrity(path));
            IsIntegrityPassed = valid;
            IntegrityStatusText = valid ? "PASSED" : "FAILED";

            string msg = valid ? _i18n.GetString("VerificationSuccess") : _i18n.GetString("VerificationFailed");
            MessageBox.Show(msg, "GlassHub Event Horizon", MessageBoxButton.OK, valid ? MessageBoxImage.Information : MessageBoxImage.Warning);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"{_i18n.GetString("Failed")}: {ex.Message}", "GlassHub Event Horizon", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void FilterEntries()
    {
        FilteredEntries.Clear();
        string q = SearchQuery.Trim();
        foreach (var entry in _allEntries)
        {
            if (string.IsNullOrEmpty(q) || entry.Contains(q, StringComparison.OrdinalIgnoreCase))
            {
                FilteredEntries.Add(entry);
            }
        }
    }
}
