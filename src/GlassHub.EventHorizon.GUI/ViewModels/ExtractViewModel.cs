using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using GlassHub.EventHorizon.Core.Interfaces;
using GlassHub.EventHorizon.Core.Localization;

namespace GlassHub.EventHorizon.GUI.ViewModels;

public class ExtractViewModel : ViewModelBase
{
    private readonly IArchiveEngine _engine;
    private readonly ILocalizationService _i18n;
    private readonly Action<string> _updateStatus;

    private string _sourceArchivePath = "";
    private string _destinationDirectory = "";
    private string _password = "";
    private bool _isBusy;
    private double _progressPercentage;
    private string _progressStatusText = "";
    private bool _isDragOver;

    public ExtractViewModel(IArchiveEngine engine, ILocalizationService i18n, Action<string> updateStatus)
    {
        _engine = engine;
        _i18n = i18n;
        _updateStatus = updateStatus;

        BrowseSourceCommand = new RelayCommand(ExecuteBrowseSource, () => !IsBusy);
        BrowseDestinationCommand = new RelayCommand(ExecuteBrowseDestination, () => !IsBusy);
        ExtractCommand = new AsyncRelayCommand(ExecuteExtractAsync, () => !IsBusy && !string.IsNullOrWhiteSpace(SourceArchivePath));
    }

    public string SourceArchivePath
    {
        get => _sourceArchivePath;
        set
        {
            if (SetProperty(ref _sourceArchivePath, value))
            {
                OnPropertyChanged(nameof(HasSourceFile));
                ((AsyncRelayCommand)ExtractCommand).RaiseCanExecuteChanged();
                AutoComputeDestination(value);
            }
        }
    }

    public bool HasSourceFile => !string.IsNullOrWhiteSpace(SourceArchivePath);

    public string DestinationDirectory
    {
        get => _destinationDirectory;
        set => SetProperty(ref _destinationDirectory, value);
    }

    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            if (SetProperty(ref _isBusy, value))
            {
                OnPropertyChanged(nameof(IsNotBusy));
                ((RelayCommand)BrowseSourceCommand).RaiseCanExecuteChanged();
                ((RelayCommand)BrowseDestinationCommand).RaiseCanExecuteChanged();
                ((AsyncRelayCommand)ExtractCommand).RaiseCanExecuteChanged();
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

    public bool IsDragOver
    {
        get => _isDragOver;
        set => SetProperty(ref _isDragOver, value);
    }

    public ICommand BrowseSourceCommand { get; }
    public ICommand BrowseDestinationCommand { get; }
    public ICommand ExtractCommand { get; }

    public void SetSourceArchive(string path)
    {
        if (File.Exists(path))
        {
            SourceArchivePath = path;
        }
    }

    private void AutoComputeDestination(string archivePath)
    {
        if (string.IsNullOrWhiteSpace(archivePath) || !File.Exists(archivePath))
            return;

        string parentDir = Path.GetDirectoryName(archivePath) ?? Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string folderName = Path.GetFileNameWithoutExtension(archivePath);
        DestinationDirectory = Path.Combine(parentDir, folderName + "_extracted");
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
            SetSourceArchive(dlg.FileName);
        }
    }

    private void ExecuteBrowseDestination()
    {
        var dlg = new OpenFolderDialog
        {
            Title = _i18n.GetString("DestFolderLabel")
        };

        if (dlg.ShowDialog() == true && !string.IsNullOrEmpty(dlg.FolderName))
        {
            DestinationDirectory = dlg.FolderName;
        }
    }

    private async Task ExecuteExtractAsync()
    {
        string source = SourceArchivePath.Trim();
        string dest = DestinationDirectory.Trim();
        string pass = Password;

        if (string.IsNullOrEmpty(source) || !File.Exists(source))
        {
            MessageBox.Show(_i18n.GetString("FileNotFound"), "GlassHub Event Horizon", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (string.IsNullOrEmpty(dest))
        {
            MessageBox.Show(_i18n.GetString("NoDestination"), "GlassHub Event Horizon", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        IsBusy = true;
        ProgressPercentage = 20;
        ProgressStatusText = _i18n.GetString("Extracting");
        _updateStatus($"{_i18n.GetString("Extracting")} -> {dest}");

        try
        {
            await Task.Run(() =>
            {
                _engine.Decompress(source, dest, string.IsNullOrEmpty(pass) ? null : pass);
            });

            ProgressPercentage = 100;
            ProgressStatusText = _i18n.GetString("Completed");
            _updateStatus(_i18n.Format("StatusExtractSuccess", dest));

            MessageBox.Show(
                $"{_i18n.GetString("Completed")}\n\n{dest}",
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
