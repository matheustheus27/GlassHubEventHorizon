using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using GlassHub.EventHorizon.Core.Interfaces;
using GlassHub.EventHorizon.Core.Localization;

namespace GlassHub.EventHorizon.GUI.ViewModels;

public class CompressViewModel : ViewModelBase
{
    private readonly IArchiveEngine _engine;
    private readonly ILocalizationService _i18n;
    private readonly Action<string> _updateStatus;

    private int _selectedFormatIndex = 0; // 0: .zip, 1: .7z, 2: .tar, 3: .gz, 4: .zst
    private int _selectedLevelIndex = 0;  // 0: Normal, 1: Fast, 2: Ultra, 3: Store
    private string _password = "";
    private int _selectedVolumeSplitIndex = 0; // 0: None, 1: 100MB, 2: 700MB, 3: 4.7GB
    private string _customVolumeSplit = "";
    private int _selectedThreads = Math.Max(1, Environment.ProcessorCount);
    private string _customCliArgs = "";
    private string _outputPath = "";
    private bool _isBusy;
    private double _progressPercentage;
    private string _progressStatusText = "";
    private string _activePreset = "Balanced";
    private bool _isDragOver;

    public CompressViewModel(IArchiveEngine engine, ILocalizationService i18n, Action<string> updateStatus)
    {
        _engine = engine;
        _i18n = i18n;
        _updateStatus = updateStatus;

        StagedItems = new ObservableCollection<string>();

        // Commands
        AddFilesCommand = new RelayCommand(ExecuteAddFiles, () => !IsBusy);
        AddFolderCommand = new RelayCommand(ExecuteAddFolder, () => !IsBusy);
        RemoveItemCommand = new RelayCommand(ExecuteRemoveItem, () => !IsBusy);
        ClearListCommand = new RelayCommand(ExecuteClearList, () => !IsBusy && StagedItems.Count > 0);
        BrowseOutputCommand = new RelayCommand(ExecuteBrowseOutput, () => !IsBusy);
        SelectPresetCommand = new RelayCommand(ExecuteSelectPreset, () => !IsBusy);
        CompressCommand = new AsyncRelayCommand(ExecuteCompressAsync, () => !IsBusy && StagedItems.Count > 0);
    }

    public ObservableCollection<string> StagedItems { get; }

    public int StagedCount => StagedItems.Count;

    public bool HasStagedItems => StagedItems.Count > 0;

    public int SelectedFormatIndex
    {
        get => _selectedFormatIndex;
        set
        {
            if (SetProperty(ref _selectedFormatIndex, value))
            {
                UpdateDefaultOutputPath();
            }
        }
    }

    public int SelectedLevelIndex
    {
        get => _selectedLevelIndex;
        set => SetProperty(ref _selectedLevelIndex, value);
    }

    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public int SelectedVolumeSplitIndex
    {
        get => _selectedVolumeSplitIndex;
        set => SetProperty(ref _selectedVolumeSplitIndex, value);
    }

    public string CustomVolumeSplit
    {
        get => _customVolumeSplit;
        set => SetProperty(ref _customVolumeSplit, value);
    }

    public int SelectedThreads
    {
        get => _selectedThreads;
        set => SetProperty(ref _selectedThreads, value);
    }

    public int MaxThreads => Environment.ProcessorCount;

    public string CustomCliArgs
    {
        get => _customCliArgs;
        set => SetProperty(ref _customCliArgs, value);
    }

    public string OutputPath
    {
        get => _outputPath;
        set => SetProperty(ref _outputPath, value);
    }

    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            if (SetProperty(ref _isBusy, value))
            {
                OnPropertyChanged(nameof(IsNotBusy));
                ((RelayCommand)AddFilesCommand).RaiseCanExecuteChanged();
                ((RelayCommand)AddFolderCommand).RaiseCanExecuteChanged();
                ((RelayCommand)ClearListCommand).RaiseCanExecuteChanged();
                ((AsyncRelayCommand)CompressCommand).RaiseCanExecuteChanged();
            }
        }
    }

    public bool IsNotBusy => !IsBusy;

    public double ProgressPercentage
    {
        get => _progressPercentage;
        set => SetProperty(ref _progressPercentage, value);
    }

    public string ProgressStatusText
    {
        get => _progressStatusText;
        set => SetProperty(ref _progressStatusText, value);
    }

    public string ActivePreset
    {
        get => _activePreset;
        set => SetProperty(ref _activePreset, value);
    }

    public bool IsDragOver
    {
        get => _isDragOver;
        set => SetProperty(ref _isDragOver, value);
    }

    // Commands
    public ICommand AddFilesCommand { get; }
    public ICommand AddFolderCommand { get; }
    public ICommand RemoveItemCommand { get; }
    public ICommand ClearListCommand { get; }
    public ICommand BrowseOutputCommand { get; }
    public ICommand SelectPresetCommand { get; }
    public ICommand CompressCommand { get; }

    public void AddPaths(IEnumerable<string> paths)
    {
        foreach (var path in paths)
        {
            if (!string.IsNullOrWhiteSpace(path) && !StagedItems.Contains(path))
            {
                StagedItems.Add(path);
            }
        }
        OnPropertyChanged(nameof(StagedCount));
        OnPropertyChanged(nameof(HasStagedItems));
        ((RelayCommand)ClearListCommand).RaiseCanExecuteChanged();
        ((AsyncRelayCommand)CompressCommand).RaiseCanExecuteChanged();

        if (string.IsNullOrEmpty(OutputPath))
        {
            UpdateDefaultOutputPath();
        }
    }

    private void ExecuteAddFiles()
    {
        var dlg = new OpenFileDialog
        {
            Multiselect = true,
            Title = _i18n.GetString("BtnAddFiles")
        };

        if (dlg.ShowDialog() == true)
        {
            AddPaths(dlg.FileNames);
        }
    }

    private void ExecuteAddFolder()
    {
        var dlg = new OpenFolderDialog
        {
            Title = _i18n.GetString("BtnAddFolder")
        };

        if (dlg.ShowDialog() == true && !string.IsNullOrEmpty(dlg.FolderName))
        {
            AddPaths(new[] { dlg.FolderName });
        }
    }

    private void ExecuteRemoveItem(object? param)
    {
        if (param is string item)
        {
            StagedItems.Remove(item);
            OnPropertyChanged(nameof(StagedCount));
            OnPropertyChanged(nameof(HasStagedItems));
            ((RelayCommand)ClearListCommand).RaiseCanExecuteChanged();
            ((AsyncRelayCommand)CompressCommand).RaiseCanExecuteChanged();
        }
    }

    private void ExecuteClearList()
    {
        StagedItems.Clear();
        OutputPath = "";
        OnPropertyChanged(nameof(StagedCount));
        OnPropertyChanged(nameof(HasStagedItems));
        ((RelayCommand)ClearListCommand).RaiseCanExecuteChanged();
        ((AsyncRelayCommand)CompressCommand).RaiseCanExecuteChanged();
    }

    private void ExecuteSelectPreset(object? param)
    {
        if (param is string preset)
        {
            ActivePreset = preset;
            switch (preset)
            {
                case "MinSize": // 7z Ultra
                    SelectedFormatIndex = 1; // .7z
                    SelectedLevelIndex = 2;  // Ultra
                    break;
                case "Fast": // Zip Fast
                    SelectedFormatIndex = 0; // .zip
                    SelectedLevelIndex = 1;  // Fast
                    break;
                case "Store": // Zip Store (0 compression, fastest)
                    SelectedFormatIndex = 0; // .zip
                    SelectedLevelIndex = 3;  // Store
                    break;
                case "Balanced": // Zip Normal
                default:
                    SelectedFormatIndex = 0; // .zip
                    SelectedLevelIndex = 0;  // Normal
                    break;
            }
        }
    }

    private void ExecuteBrowseOutput()
    {
        string ext = GetSelectedFormatExtension();
        var dlg = new SaveFileDialog
        {
            Filter = $"Archive File (*{ext})|*{ext}|All Files (*.*)|*.*",
            DefaultExt = ext,
            Title = _i18n.GetString("OutputPathLabel")
        };

        if (dlg.ShowDialog() == true)
        {
            OutputPath = dlg.FileName;
        }
    }

    public string GetSelectedFormatExtension()
    {
        return SelectedFormatIndex switch
        {
            1 => ".7z",
            2 => ".tar",
            3 => ".gz",
            4 => ".zst",
            _ => ".zip"
        };
    }

    private void UpdateDefaultOutputPath()
    {
        if (StagedItems.Count == 0) return;

        string first = StagedItems[0];
        string baseDir = Directory.Exists(first) ? Path.GetDirectoryName(first)! : Path.GetDirectoryName(first)!;
        string baseName = Directory.Exists(first) ? new DirectoryInfo(first).Name : Path.GetFileNameWithoutExtension(first);

        if (string.IsNullOrEmpty(baseDir))
        {
            baseDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }

        string ext = GetSelectedFormatExtension();
        OutputPath = Path.Combine(baseDir, $"{baseName}_archive{ext}");
    }

    private async Task ExecuteCompressAsync()
    {
        if (StagedItems.Count == 0)
        {
            MessageBox.Show(_i18n.GetString("NoInput"), "GlassHub Event Horizon", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(OutputPath))
        {
            UpdateDefaultOutputPath();
        }

        string targetOutput = OutputPath;
        string pass = Password;
        var itemsToCompress = StagedItems.ToList();

        IsBusy = true;
        ProgressPercentage = 20;
        ProgressStatusText = _i18n.GetString("Compressing");
        _updateStatus($"{_i18n.GetString("Compressing")} -> {Path.GetFileName(targetOutput)}");

        try
        {
            await Task.Run(() =>
            {
                _engine.Compress(itemsToCompress, targetOutput, string.IsNullOrEmpty(pass) ? null : pass);
            });

            ProgressPercentage = 100;
            ProgressStatusText = _i18n.GetString("Completed");
            _updateStatus(_i18n.Format("StatusCompressSuccess", targetOutput));

            MessageBox.Show(
                $"{_i18n.GetString("Completed")}\n\n{targetOutput}",
                "GlassHub Event Horizon",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            ProgressPercentage = 0;
            ProgressStatusText = _i18n.GetString("Failed");
            _updateStatus($"{_i18n.GetString("Failed")}: {ex.Message}");

            MessageBox.Show(
                $"{_i18n.GetString("Failed")}: {ex.Message}",
                "GlassHub Event Horizon",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }
}
