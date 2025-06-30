using ApacheTech.VintageMods.StructureLogger.Features.Dialogue;
using ApacheTech.VintageMods.StructureLogger.Features.LogStructures.Settings;
using Gantry.Core.Annotation;
using Gantry.Core.Hosting.Registration;
using Gantry.Services.FileSystem.Configuration;
using Gantry.Services.FileSystem.Hosting;
using Gantry.Services.Network.Extensions;
using Vintagestory.API.Server;

namespace ApacheTech.VintageMods.StructureLogger.Features.LogStructures.Systems;

/// <summary>
///     Adds server-side configuration for structure logging, including command registration
///     and runtime settings for filtering vanilla structure data.
/// </summary>
public class LogStructures : UniversalModSystem, IServerServiceRegistrar
{
    private LogStructuresSetttings? _settings;
    private IServerNetworkChannel? _serverChannel;

    /// <summary>
    ///     Registers mod-specific services into the server's dependency injection container,
    ///     including feature-level world settings for structure logging.
    /// </summary>
    /// <param name="services">The server-side service collection.</param>
    /// <param name="sapi">The core server API.</param>
    public void ConfigureServerModServices(IServiceCollection services, ICoreServerAPI sapi)
    {
        services.AddFeatureGlobalSettings<LogStructuresSetttings>();
    }

    protected override void StartPreClientSide(ICoreClientAPI capi)
    {
        capi.Network
            .RegisterDefaultChannel()
            .RegisterPacket<LogStructuresSetttings>(OnGuiRequested);
    }

    /// <summary>
    ///     Called during the server startup sequence, before full initialisation.
    ///     Loads settings and registers the <c>/slogger</c> chat command with optional filtering subcommands.
    /// </summary>
    /// <param name="sapi">The core server API.</param>
    protected override void StartPreServerSide(ICoreServerAPI sapi)
    {
        _settings = ModSettings.Global.Feature<LogStructuresSetttings>();
        _serverChannel = sapi.Network
            .RegisterDefaultChannel()
            .RegisterPacket<LogStructuresSetttings>(OnSettingsChanged);

        var command = sapi.ChatCommands
            .Create("slogger")
            .RequiresPrivilege(Privilege.controlserver)
            .WithDescription(T("ShowDialogue.Description"))
            .HandleWith(ShowSettingsDialogue);
    }

    [ClientSide]
    private void OnGuiRequested(LogStructuresSetttings currentSettings)
    {
        var dialogue = new LogStructuresSetttingsDialogue(Capi, currentSettings);
        dialogue.ToggleGui();
    }

    [ServerSide]
    private void OnSettingsChanged(IServerPlayer fromPlayer, LogStructuresSetttings newSettings)
    {
        if (_settings is null) return;
        _settings.IncludeVanillaResults = newSettings.IncludeVanillaResults;
        _settings.IncludeSurfaceRuins = newSettings.IncludeSurfaceRuins;
        _settings.IncludeSurfaceStructures = newSettings.IncludeSurfaceStructures;
        _settings.IncludeUnderwaterStructures = newSettings.IncludeUnderwaterStructures;
        _settings.IncludeUndergroundStructures = newSettings.IncludeUndergroundStructures;
    }

    /// <summary>
    ///     Handles the root <c>/slogger</c> command and reports the current setting for filtering vanilla results.
    /// </summary>
    /// <param name="args">The command arguments and calling context.</param>
    /// <returns>A success or error result, depending on the availability of settings.</returns>
    private TextCommandResult ShowSettingsDialogue(TextCommandCallingArgs args)
    {
        if (_settings is null) return TextCommandResult.Error(T("Error"));
        var player = args.Caller.Player.To<IServerPlayer>();
        var settings = new LogStructuresSetttings
        {
            IncludeVanillaResults = _settings.IncludeVanillaResults,
            IncludeSurfaceRuins = _settings.IncludeSurfaceRuins,
            IncludeSurfaceStructures = _settings.IncludeSurfaceStructures,
            IncludeUnderwaterStructures = _settings.IncludeUnderwaterStructures,
            IncludeUndergroundStructures = _settings.IncludeUndergroundStructures
        };
        _serverChannel?.SendPacket(settings, player);
        return TextCommandResult.Success();
    }

    /// <summary>
    ///     Retrieves a localised string for the LogStructures feature using the specified path and formatting arguments.
    /// </summary>
    /// <param name="path">The key path to the localised string within the feature's language resource file.</param>
    /// <param name="_">Optional arguments to format into the resulting string, if placeholders exist.</param>
    /// <returns>The formatted, localised string associated with the specified path.</returns>
    private static string T(string path, params object[] _)
        => path switch
        {
            "ShowDialogue.Description" => "Shows the settings dialogue for Better Ruins Structure Logger.",
            "Error" => "Structure Logger Settings not found.",
            _ => path
        };
}