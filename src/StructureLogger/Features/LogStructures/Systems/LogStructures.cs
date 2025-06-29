using ApacheTech.VintageMods.StructureLogger.Features.LogStructures.Settings;
using Gantry.Core.Hosting.Registration;
using Gantry.Services.FileSystem.Configuration;
using Gantry.Services.FileSystem.Hosting;
using Vintagestory.API.Server;

namespace ApacheTech.VintageMods.StructureLogger.Features.LogStructures.Systems;

/// <summary>
///     Adds server-side configuration for structure logging, including command registration
///     and runtime settings for filtering vanilla structure data.
/// </summary>
public class LogStructures : ServerModSystem, IServerServiceRegistrar
{
    private LogStructuresSetttings? _settings;

    /// <summary>
    ///     Registers mod-specific services into the server's dependency injection container,
    ///     including feature-level world settings for structure logging.
    /// </summary>
    /// <param name="services">The server-side service collection.</param>
    /// <param name="sapi">The core server API.</param>
    public void ConfigureServerModServices(IServiceCollection services, ICoreServerAPI sapi)
    {
        services.AddFeatureWorldSettings<LogStructuresSetttings>();
    }

    /// <summary>
    ///     Called during the server startup sequence, before full initialisation.
    ///     Loads settings and registers the <c>/slogger</c> chat command with optional filtering subcommands.
    /// </summary>
    /// <param name="sapi">The core server API.</param>
    protected override void StartPreServerSide(ICoreServerAPI sapi)
    {
        _settings = ModSettings.World.Feature<LogStructuresSetttings>();

        sapi.ChatCommands
            .Create("slogger")
            .RequiresPrivilege(Privilege.controlserver)
            .WithDescription(T("Commands.Slogger.Description"))
            .HandleWith(OnSloggerCommand)
            .BeginSubCommand("filter")
                .WithAlias("f")
                .WithArgs(sapi.ChatCommands.Parsers.Bool("filter", trueAlias: "on"))
                .WithDescription(T("Commands.Slogger.Filter.Description"))
                .HandleWith(OnFilterCommand)
                .EndSubCommand();
    }

    /// <summary>
    ///     Handles the root <c>/slogger</c> command and reports the current setting for filtering vanilla results.
    /// </summary>
    /// <param name="args">The command arguments and calling context.</param>
    /// <returns>A success or error result, depending on the availability of settings.</returns>
    private TextCommandResult OnSloggerCommand(TextCommandCallingArgs args)
    {
        if (_settings is null) return TextCommandResult.Error(T("Commands.Slogger.Error"));
        return TextCommandResult.Success(T("Commands.Slogger.Success", _settings.FilterVanillaResults));
    }

    /// <summary>
    ///     Handles the <c>/slogger filter</c> subcommand, allowing runtime configuration of vanilla result filtering.
    /// </summary>
    /// <param name="args">The command arguments and calling context.</param>
    /// <returns>A success or error result after updating the setting.</returns>
    private TextCommandResult OnFilterCommand(TextCommandCallingArgs args)
    {
        if (_settings is null) return TextCommandResult.Error(T("Commands.Slogger.Error"));
        var filter = args.Parsers[0].GetValue().To<bool>();
        _settings.FilterVanillaResults = filter;
        return TextCommandResult.Success(T("Commands.Slogger.Success", _settings.FilterVanillaResults));
    }

    /// <summary>
    ///     Retrieves a localised string for the LogStructures feature using the specified path and formatting arguments.
    /// </summary>
    /// <param name="path">The key path to the localised string within the feature's language resource file.</param>
    /// <param name="args">Optional arguments to format into the resulting string, if placeholders exist.</param>
    /// <returns>The formatted, localised string associated with the specified path.</returns>
    public static string T(string path, params object[] args)
        => LangEx.FeatureString(nameof(LogStructures), path, args);
}