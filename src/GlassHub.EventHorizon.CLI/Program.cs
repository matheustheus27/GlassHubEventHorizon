using GlassHub.EventHorizon.CLI;
using GlassHub.EventHorizon.Core.Engines;
using GlassHub.EventHorizon.Core.Interfaces;
using GlassHub.EventHorizon.Core.Localization;
using GlassHub.EventHorizon.Engine.Native;
using GlassHub.EventHorizon.Engine.SevenZip;

ILocalizationService localization = new LocalizationService();

IArchiveEngine nativeEngine = new NativeZipEngine();
IArchiveEngine sevenZipEngine = new SevenZipEngine();
IArchiveEngine fallbackEngine = new FallbackArchiveEngine(nativeEngine, sevenZipEngine);

var dispatcher = new CommandDispatcher(fallbackEngine, localization);
dispatcher.Execute(args);