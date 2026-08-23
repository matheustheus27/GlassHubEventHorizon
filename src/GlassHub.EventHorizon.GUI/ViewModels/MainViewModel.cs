using System.Windows.Input;
using GlassHub.EventHorizon.Core.Engines;
using GlassHub.EventHorizon.Core.Interfaces;
using GlassHub.EventHorizon.Core.Localization;
using GlassHub.EventHorizon.Engine.Native;
using GlassHub.EventHorizon.Engine.SevenZip;

namespace GlassHub.EventHorizon.GUI.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly IArchiveEngine _engine;
    private readonly ILocalizationService _i18n;

    private ViewModelBase _currentView;
    private string _currentViewName = "Compress";
    private string _statusMessage = "";

    public MainViewModel()
    {
        _i18n = new LocalizationService();
        _engine = new FallbackArchiveEngine(new NativeZipEngine(), new SevenZipEngine());

        CompressVM = new CompressViewModel(_engine, _i18n, msg => StatusMessage = msg);
        ExtractVM = new ExtractViewModel(_engine, _i18n, msg => StatusMessage = msg);
        InspectVM = new InspectViewModel(_engine, _i18n, msg => StatusMessage = msg);
        SettingsVM = new SettingsViewModel(_i18n, RefreshLocalization, msg => StatusMessage = msg);

        _currentView = CompressVM;
        _statusMessage = $"{_i18n.GetString("StatusReady")} | {_i18n.GetString("EngineFallback")}";

        NavigateCompressCommand = new RelayCommand(() => NavigateTo("Compress", CompressVM));
        NavigateExtractCommand = new RelayCommand(() => NavigateTo("Extract", ExtractVM));
        NavigateInspectCommand = new RelayCommand(() => NavigateTo("Inspect", InspectVM));
        NavigateSettingsCommand = new RelayCommand(() => NavigateTo("Settings", SettingsVM));
    }

    public CompressViewModel CompressVM { get; }
    public ExtractViewModel ExtractVM { get; }
    public InspectViewModel InspectVM { get; }
    public SettingsViewModel SettingsVM { get; }

    public ILocalizationService I18n => _i18n;

    public ViewModelBase CurrentView
    {
        get => _currentView;
        set => SetProperty(ref _currentView, value);
    }

    public string CurrentViewName
    {
        get => _currentViewName;
        set
        {
            if (SetProperty(ref _currentViewName, value))
            {
                OnPropertyChanged(nameof(IsCompressActive));
                OnPropertyChanged(nameof(IsExtractActive));
                OnPropertyChanged(nameof(IsInspectActive));
                OnPropertyChanged(nameof(IsSettingsActive));
            }
        }
    }

    public bool IsCompressActive => CurrentViewName == "Compress";
    public bool IsExtractActive => CurrentViewName == "Extract";
    public bool IsInspectActive => CurrentViewName == "Inspect";
    public bool IsSettingsActive => CurrentViewName == "Settings";

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    public ICommand NavigateCompressCommand { get; }
    public ICommand NavigateExtractCommand { get; }
    public ICommand NavigateInspectCommand { get; }
    public ICommand NavigateSettingsCommand { get; }

    public void NavigateTo(string viewName, ViewModelBase view)
    {
        CurrentViewName = viewName;
        CurrentView = view;
    }

    public void HandleGlobalFileDrop(string[] files)
    {
        if (files.Length == 0) return;

        string first = files[0];
        string ext = System.IO.Path.GetExtension(first).ToLowerInvariant();

        // If it's an archive format, route to Extract view
        if (ext is ".zip" or ".7z" or ".rar" or ".tar" or ".gz" or ".zst" or ".bz2" or ".xz")
        {
            ExtractVM.SetSourceArchive(first);
            NavigateTo("Extract", ExtractVM);
        }
        else
        {
            // Otherwise, route to Compress view and stage files
            CompressVM.AddPaths(files);
            NavigateTo("Compress", CompressVM);
        }
    }

    private void RefreshLocalization()
    {
        OnPropertyChanged(nameof(I18n));
        StatusMessage = $"{_i18n.GetString("StatusReady")} | {_i18n.GetString("EngineFallback")}";
    }
}
