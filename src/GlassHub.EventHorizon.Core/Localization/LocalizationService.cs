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
            ["NoInput"] = "Error: No input files or directories specified (-i / -f).",
            ["NoOutput"] = "Error: No output archive destination specified (-o).",
            ["NoDestination"] = "Error: No extraction destination directory specified (-d).",
            ["EngineNative"] = "Motor: Native .NET (System.IO.Compression)",
            ["EngineSevenZip"] = "Motor: 7-Zip Engine CLI",
            ["EngineFallback"] = "Motor: Smart Fallback (Native Zip / 7-Zip)",
            ["VerificationSuccess"] = "Integrity Check: PASSED - Archive structure is valid.",
            ["VerificationFailed"] = "Integrity Check: FAILED - Archive is corrupt or password protected.",
            ["EntryListHeader"] = "Archive Content Registry:",
            ["MetadataHeader"] = "Archive Telemetry Details:"
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
            ["NoInput"] = "Erro: Nenhum arquivo ou diretório de entrada informado (-i / -f).",
            ["NoOutput"] = "Erro: Nenhum destino de arquivo compactado informado (-o).",
            ["NoDestination"] = "Erro: Nenhum diretório de destino de extração informado (-d).",
            ["EngineNative"] = "Motor: Nativo .NET (System.IO.Compression)",
            ["EngineSevenZip"] = "Motor: 7-Zip Engine CLI",
            ["EngineFallback"] = "Motor: Chaveamento Inteligente (Nativo / 7-Zip)",
            ["VerificationSuccess"] = "Verificação de Integridade: APROVADO - Estrutura válida.",
            ["VerificationFailed"] = "Verificação de Integridade: FALHOU - Arquivo corrompido ou protegido.",
            ["EntryListHeader"] = "Registro de Conteúdo do Arquivo:",
            ["MetadataHeader"] = "Detalhes de Telemetria e Metadados:"
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
