using GlassHub.EventHorizon.CLI.Commands;
using GlassHub.EventHorizon.CLI.Components.Organisms;
using GlassHub.EventHorizon.CLI.Components.Templates;
using GlassHub.EventHorizon.CLI.Parsing;
using GlassHub.EventHorizon.Core.Interfaces;
using GlassHub.EventHorizon.Core.Localization;

namespace GlassHub.EventHorizon.CLI;

public sealed class CommandDispatcher
{
    private readonly IArchiveEngine _engine;
    private readonly ILocalizationService _i18n;

    public CommandDispatcher(IArchiveEngine engine, ILocalizationService i18n)
    {
        _engine = engine;
        _i18n = i18n;
    }

    public void Execute(string[] args)
    {
        var options = ArgumentParser.Parse(args);

        if (!string.IsNullOrEmpty(options.Language))
        {
            _i18n.SetCulture(options.Language);
        }

        GlassConsoleTemplate.RenderShell(_i18n, () =>
        {
            switch (options.Command.ToLowerInvariant())
            {
                case "compress":
                case "c":
                    ArchiveCompressOrganism.Execute(_engine, options, _i18n);
                    break;

                case "extract":
                case "x":
                    ArchiveExtractOrganism.Execute(_engine, options, _i18n);
                    break;

                case "list":
                case "l":
                    ArchiveInspectorOrganism.List(_engine, options, _i18n);
                    break;

                case "info":
                    ArchiveInspectorOrganism.Info(_engine, options, _i18n);
                    break;

                case "verify":
                case "v":
                    ArchiveInspectorOrganism.Verify(_engine, options, _i18n);
                    break;

                case "help":
                default:
                    HelpCommand.Execute(_i18n);
                    break;
            }
        });
    }
}