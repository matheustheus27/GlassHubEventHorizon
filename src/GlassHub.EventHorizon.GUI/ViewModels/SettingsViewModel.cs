using System.Windows.Input;
using Microsoft.Win32;
using GlassHub.EventHorizon.Core.Localization;

namespace GlassHub.EventHorizon.GUI.ViewModels;

public class SettingsViewModel : ViewModelBase
{
    private readonly ILocalizationService _i18n;
    private readonly Action _onLanguageChanged;
    private readonly Action<string> _updateStatus;

    private int _selectedThemeIndex = 0; // 0: Cosmic Dark, 1: Fluent Light, 2: WinRAR Cyber, 3: Neon Cyberpunk, 4: Glass Dark
    private int _selectedLanguageIndex = 0; // 0: pt-BR, 1: en-US
    private int _preferredEngineIndex = 0; // 0: Smart Fallback, 1: Native, 2: 7-Zip
    private int _cpuThreadsLimit = Math.Max(1, Environment.ProcessorCount);
    private string _defaultExtractionPath = "";
    private bool _integrateExplorer = true;

    public SettingsViewModel(ILocalizationService i18n, Action onLanguageChanged, Action<string> updateStatus)
    {
        _i18n = i18n;
        _onLanguageChanged = onLanguageChanged;
        _updateStatus = updateStatus;

        _selectedLanguageIndex = _i18n.CurrentLanguage.StartsWith("pt", StringComparison.OrdinalIgnoreCase) ? 0 : 1;

        BrowseDefaultPathCommand = new RelayCommand(ExecuteBrowseDefaultPath);
    }

    public int SelectedThemeIndex
    {
        get => _selectedThemeIndex;
        set
        {
            if (SetProperty(ref _selectedThemeIndex, value))
            {
                ApplySelectedTheme();
            }
        }
    }

    public int SelectedLanguageIndex
    {
        get => _selectedLanguageIndex;
        set
        {
            if (SetProperty(ref _selectedLanguageIndex, value))
            {
                ApplySelectedLanguage();
            }
        }
    }

    public int PreferredEngineIndex
    {
        get => _preferredEngineIndex;
        set => SetProperty(ref _preferredEngineIndex, value);
    }

    public int CpuThreadsLimit
    {
        get => _cpuThreadsLimit;
        set => SetProperty(ref _cpuThreadsLimit, value);
    }

    public int MaxCpuThreads => Environment.ProcessorCount;

    public string DefaultExtractionPath
    {
        get => _defaultExtractionPath;
        set => SetProperty(ref _defaultExtractionPath, value);
    }

    public bool IntegrateExplorer
    {
        get => _integrateExplorer;
        set => SetProperty(ref _integrateExplorer, value);
    }

    public ICommand BrowseDefaultPathCommand { get; }

    private void ExecuteBrowseDefaultPath()
    {
        var dlg = new OpenFolderDialog
        {
            Title = _i18n.GetString("DefaultOutputDir")
        };

        if (dlg.ShowDialog() == true && !string.IsNullOrEmpty(dlg.FolderName))
        {
            DefaultExtractionPath = dlg.FolderName;
        }
    }

    private void ApplySelectedTheme()
    {
        string theme = SelectedThemeIndex switch
        {
            1 => "Fluent Light",
            2 => "WinRAR Cyber",
            3 => "Neon Cyberpunk",
            4 => "Glass Dark",
            _ => "GlassHub Cosmic Dark"
        };

        ThemeManager.ApplyTheme(theme);
    }

    private void ApplySelectedLanguage()
    {
        string lang = SelectedLanguageIndex == 0 ? "pt-BR" : "en-US";
        _i18n.SetCulture(lang);
        _onLanguageChanged();
        _updateStatus(_i18n.GetString("StatusReady"));
    }
}
