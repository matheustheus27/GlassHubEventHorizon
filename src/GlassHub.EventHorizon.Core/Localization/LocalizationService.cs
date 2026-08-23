using System.Globalization;

namespace GlassHub.EventHorizon.Core.Localization;

public class LocalizationService : ILocalizationService
{
    private string _currentLanguage = "en-US";

    private readonly Dictionary<string, Dictionary<string, string>> _dictionary = new()
    {
        ["en-US"] = new Dictionary<string, string>
        {
            ["AppHeader"] = "GlassHubEventHorizon - High-Performance Compression System",
            ["EcosystemTag"] = "Part of the GlassHub Ecosystem",
            ["HelpCompress"] = "Compress files or directories into a target archive",
            ["HelpExtract"] = "Extract an archive file to a destination directory",
            ["HelpList"] = "List entries contained within an archive file",
            ["HelpVerify"] = "Test and verify structural integrity of an archive",
            ["HelpInfo"] = "Display metadata, format, compression ratio and telemetry",
            ["Compressing"] = "Compressing target resources...",
            ["Extracting"] = "Extracting archive contents...",
            ["Completed"] = "Operation completed successfully.",
            ["Failed"] = "Operation failed.",
            ["FileNotFound"] = "Source path or file was not found: {0}",
            ["NoInput"] = "Error: No input files or directories specified.",
            ["NoOutput"] = "Error: No output archive destination specified.",
            ["NoDestination"] = "Error: No extraction destination directory specified.",
            ["EngineNative"] = "Motor: Native .NET (System.IO.Compression)",
            ["EngineSevenZip"] = "Motor: 7-Zip Engine CLI",
            ["EngineFallback"] = "Motor: Smart Fallback (Native Zip / 7-Zip)",
            ["VerificationSuccess"] = "Integrity Check: PASSED - Archive structure is valid.",
            ["VerificationFailed"] = "Integrity Check: FAILED - Archive is corrupt or password protected.",
            ["EntryListHeader"] = "Archive Content Registry:",
            ["MetadataHeader"] = "Archive Telemetry Details:",
            
            // GUI Modern Windows 11 Tabs, Navigation & Controls
            ["NavCompress"] = "Compactar",
            ["NavExtract"] = "Extrair",
            ["NavInspect"] = "Inspetor & Ferramentas",
            ["NavSettings"] = "Configurações",

            ["TabCompress"] = "📦 Compress",
            ["TabExtract"] = "📂 Extract",
            ["TabTelemetry"] = "📊 Telemetry & Inspection",
            ["SettingsTitle"] = "⚙️ System & Engine Settings",
            ["DragDropTitle"] = "Drag & Drop Files or Folders Here",
            ["DragDropSubtitle"] = "Release files here or click to browse",
            ["BtnAddFiles"] = "📁 Add Files",
            ["BtnAddFolder"] = "📁 Add Folder",
            ["BtnClearList"] = "🗑️ Clear All",
            ["StagedItemsHeader"] = "Selected Items to Compress ({0}):",
            ["FormatLabel"] = "Archive Format:",
            ["CompressionLevelLabel"] = "Compression Level:",
            ["PasswordLabel"] = "Encryption Password (AES-256):",
            ["OutputPathLabel"] = "Output Archive File:",
            ["Browse"] = "Browse...",
            ["BtnCompressAction"] = "🚀 COMPRESS NOW",
            ["BtnExtractAction"] = "⚡ EXTRACT NOW",
            ["BtnInspectAction"] = "🔍 Inspect Content",
            ["BtnVerifyAction"] = "🛡️ Test Integrity",
            ["ArchiveSelectLabel"] = "Select Archive File:",
            ["DestFolderLabel"] = "Destination Directory:",
            ["LanguageLabel"] = "Interface Language:",
            ["EngineSelectLabel"] = "Preferred Archive Engine:",
            ["AdvancedModeLabel"] = "Advanced Mode (Show manual text box controls)",
            ["SaveAndClose"] = "Save & Apply Settings",
            ["StatusReady"] = "Status: Ready for operation",
            ["StatusCompressSuccess"] = "Status: Compressed to {0} successfully!",
            ["StatusExtractSuccess"] = "Status: Extracted to {0} successfully!",
            ["StatusInspected"] = "Status: Inspected archive metadata.",
            ["LevelNormal"] = "Normal (Balanced)",
            ["LevelFast"] = "Fast (Quick)",
            ["LevelUltra"] = "Ultra (Maximum Compression)",
            ["LevelStore"] = "Store (No Compression)",
            ["ThemeLabel"] = "Visual Theme / Skin:",
            ["ThemeCosmicDark"] = "GlassHub Cosmic Dark (Default)",
            ["ThemeGlassDark"] = "Glass Dark (VS Code)",
            ["ThemeFluentLight"] = "Fluent Light (Windows 11)",
            ["ThemeWinRar"] = "WinRAR Cyber (Metal)",
            ["ThemeNeon"] = "Neon Cyberpunk",
            ["AdvancedOptions"] = "⚙️ Advanced Options (Format, Encryption, Volumes, Threads)",
            ["BtnSelectUnified"] = "📁 Browse Files or Folder...",
            ["DragDropExtractTitle"] = "Drag & Drop Archive Here to Extract",
            ["DragDropExtractSubtitle"] = "Supports .zip, .7z, .rar, .tar, .gz, .zst",

            // 1-Click Quick Presets
            ["PresetLabel"] = "Quick 1-Click Presets:",
            ["PresetMinSize"] = "🗜️ Minimum Size (7z Ultra)",
            ["PresetBalanced"] = "⚖️ Balanced (ZIP Normal)",
            ["PresetFast"] = "⚡ Ultra Fast (Fast)",
            ["PresetStore"] = "💎 Uncompressed / Raw",

            // Granular Advanced Options
            ["VolumeSplitLabel"] = "Split into Volumes (Part Size):",
            ["CpuThreadsLabel"] = "CPU Core / Thread Limit:",
            ["ThreadsAuto"] = "Auto ({0} threads detected)",
            ["CustomCliArgsLabel"] = "Custom CLI Parameters:",
            ["ExplorerIntegration"] = "Integrate into Windows Explorer Context Menu",
            ["DefaultOutputDir"] = "Default Destination Directory:",
            ["TelemetryEntries"] = "ENTRIES",
            ["TelemetryUncompressed"] = "UNCOMPRESSED SIZE",
            ["TelemetryRatio"] = "COMPRESSION RATIO",
            ["TelemetryIntegrity"] = "INTEGRITY STATUS",
            ["BatchConversion"] = "Batch Conversion & Repacking",
            ["SearchingEntries"] = "Search archive contents..."
        },
        ["pt-BR"] = new Dictionary<string, string>
        {
            ["AppHeader"] = "GlassHubEventHorizon - Sistema de Alta Performance de Compressão",
            ["EcosystemTag"] = "Integrante do Ecossistema GlassHub",
            ["HelpCompress"] = "Compactar arquivos ou diretórios em um arquivo final",
            ["HelpExtract"] = "Descompactar um arquivo para um diretório de destino",
            ["HelpList"] = "Listar as entradas contidas em um arquivo compactado",
            ["HelpVerify"] = "Testar e verificar a integridade estrutural do arquivo",
            ["HelpInfo"] = "Exibir metadados, formato, taxa de compressão e telemetria",
            ["Compressing"] = "Compactando recursos alvo...",
            ["Extracting"] = "Descompactando conteúdo do arquivo...",
            ["Completed"] = "Operação concluída com sucesso.",
            ["Failed"] = "Operação falhou.",
            ["FileNotFound"] = "Caminho ou arquivo de origem não encontrado: {0}",
            ["NoInput"] = "Erro: Nenhum arquivo ou diretório de entrada selecionado.",
            ["NoOutput"] = "Erro: Nenhum destino de arquivo compactado informado.",
            ["NoDestination"] = "Erro: Nenhum diretório de destino de extração informado.",
            ["EngineNative"] = "Motor: Nativo .NET (System.IO.Compression)",
            ["EngineSevenZip"] = "Motor: 7-Zip Engine CLI",
            ["EngineFallback"] = "Motor: Chaveamento Inteligente (Nativo / 7-Zip)",
            ["VerificationSuccess"] = "Verificação de Integridade: APROVADO - Estrutura válida.",
            ["VerificationFailed"] = "Verificação de Integridade: FALHOU - Arquivo corrompido ou protegido.",
            ["EntryListHeader"] = "Registro de Conteúdo do Arquivo:",
            ["MetadataHeader"] = "Detalhes de Telemetria e Metadados:",

            // GUI Modern Windows 11 Tabs, Navigation & Controls
            ["NavCompress"] = "Compactar",
            ["NavExtract"] = "Extrair",
            ["NavInspect"] = "Inspetor & Ferramentas",
            ["NavSettings"] = "Configurações",

            ["TabCompress"] = "📦 Compactar",
            ["TabExtract"] = "📂 Extrair",
            ["TabTelemetry"] = "📊 Telemetria & Inspeção",
            ["SettingsTitle"] = "⚙️ Configurações Globais do Sistema",
            ["DragDropTitle"] = "Arraste & Solte Arquivos ou Pastas Aqui",
            ["DragDropSubtitle"] = "Solte os arquivos aqui ou clique no botão para procurar",
            ["BtnAddFiles"] = "📁 Adicionar Arquivos",
            ["BtnAddFolder"] = "📁 Adicionar Pasta",
            ["BtnClearList"] = "🗑️ Limpar Todos",
            ["StagedItemsHeader"] = "Itens Selecionados para Compactar ({0}):",
            ["FormatLabel"] = "Formato de Saída:",
            ["CompressionLevelLabel"] = "Nível de Compressão:",
            ["PasswordLabel"] = "Senha de Criptografia (AES-256):",
            ["OutputPathLabel"] = "Arquivo de Saída Compactado:",
            ["Browse"] = "Procurar...",
            ["BtnCompressAction"] = "🚀 COMPACTAR AGORA",
            ["BtnExtractAction"] = "⚡ EXTRAIR AGORA",
            ["BtnInspectAction"] = "🔍 Inspecionar Conteúdo",
            ["BtnVerifyAction"] = "🛡️ Testar Integridade",
            ["ArchiveSelectLabel"] = "Selecione o Arquivo Compactado:",
            ["DestFolderLabel"] = "Diretório de Destino:",
            ["LanguageLabel"] = "Idioma da Interface:",
            ["EngineSelectLabel"] = "Motor de Compressão Preferido:",
            ["AdvancedModeLabel"] = "Modo Avançado (Exibir caixas manuais de texto)",
            ["SaveAndClose"] = "Salvar e Aplicar Configurações",
            ["StatusReady"] = "Status: Pronto para operação",
            ["StatusCompressSuccess"] = "Status: Compactado para {0} com sucesso!",
            ["StatusExtractSuccess"] = "Status: Extraído para {0} com sucesso!",
            ["StatusInspected"] = "Status: Metadados inspecionados.",
            ["LevelNormal"] = "Normal (Balanceado)",
            ["LevelFast"] = "Rápido (Compressão Veloz)",
            ["LevelUltra"] = "Ultra (Compressão Máxima)",
            ["LevelStore"] = "Armazenar (Sem Compressão)",
            ["ThemeLabel"] = "Tema Visual / Skin:",
            ["ThemeCosmicDark"] = "GlassHub Cosmic Dark (Padrão)",
            ["ThemeGlassDark"] = "Glass Dark (VS Code)",
            ["ThemeFluentLight"] = "Fluent Light (Windows 11)",
            ["ThemeWinRar"] = "WinRAR Cyber (Metal)",
            ["ThemeNeon"] = "Neon Cyberpunk",
            ["AdvancedOptions"] = "⚙️ Opções Avançadas (Formato, Criptografia, Volumes, Threads)",
            ["BtnSelectUnified"] = "📁 Procurar Arquivos ou Pasta...",
            ["DragDropExtractTitle"] = "Arraste o Arquivo Compactado Aqui para Extrair",
            ["DragDropExtractSubtitle"] = "Suporta .zip, .7z, .rar, .tar, .gz, .zst",

            // 1-Click Quick Presets
            ["PresetLabel"] = "Presets Rápidos em 1 Clique:",
            ["PresetMinSize"] = "🗜️ Tamanho Mínimo (7z Ultra)",
            ["PresetBalanced"] = "⚖️ Balanceado (ZIP Normal)",
            ["PresetFast"] = "⚡ Ultra Rápido (Veloz)",
            ["PresetStore"] = "💎 Sem Perdas / Raw",

            // Granular Advanced Options
            ["VolumeSplitLabel"] = "Divisão em Volumes (Tamanho):",
            ["CpuThreadsLabel"] = "Limite de Threads/Núcleos de CPU:",
            ["ThreadsAuto"] = "Automático ({0} threads detectadas)",
            ["CustomCliArgsLabel"] = "Parâmetros Adicionais via CLI:",
            ["ExplorerIntegration"] = "Integrar ao Menu de Contexto do Windows Explorer",
            ["DefaultOutputDir"] = "Diretório Padrão de Extração/Saída:",
            ["TelemetryEntries"] = "ENTRADAS",
            ["TelemetryUncompressed"] = "TAMANHO ORIGINAL",
            ["TelemetryRatio"] = "TAXA COMPRESSÃO",
            ["TelemetryIntegrity"] = "INTEGRIDADE",
            ["BatchConversion"] = "Conversão e Repacotamento em Lote",
            ["SearchingEntries"] = "Pesquisar entradas no arquivo..."
        }
    };

    public LocalizationService(string? initialCulture = null)
    {
        string culture = initialCulture ?? CultureInfo.CurrentUICulture.Name;
        SetCulture(culture);
    }

    public string CurrentLanguage => _currentLanguage;

    public void SetCulture(string cultureCode)
    {
        _currentLanguage = cultureCode.StartsWith("pt", StringComparison.OrdinalIgnoreCase)
            ? "pt-BR"
            : "en-US";

        var cultureInfo = new CultureInfo(_currentLanguage);
        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;
    }

    public string GetString(string key)
    {
        if (_dictionary.TryGetValue(_currentLanguage, out var dict) && dict.TryGetValue(key, out string? value))
        {
            return value;
        }

        if (_dictionary["en-US"].TryGetValue(key, out string? fallbackValue))
        {
            return fallbackValue;
        }

        return key;
    }

    public string Format(string key, params object[] args)
    {
        string raw = GetString(key);
        return string.Format(raw, args);
    }
}
