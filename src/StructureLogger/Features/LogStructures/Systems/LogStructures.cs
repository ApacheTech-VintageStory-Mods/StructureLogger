using ApacheTech.VintageMods.StructureLogger.Features.LogStructures.Settings;
using Gantry.Core.Hosting.Registration;
using Gantry.Services.FileSystem.Configuration;
using Gantry.Services.FileSystem.Hosting;
using System.Text;
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
        services.AddFeatureGlobalSettings<LogStructuresSetttings>();
    }

    /// <summary>
    ///     Called during the server startup sequence, before full initialisation.
    ///     Loads settings and registers the <c>/slogger</c> chat command with optional filtering subcommands.
    /// </summary>
    /// <param name="sapi">The core server API.</param>
    protected override void StartPreServerSide(ICoreServerAPI sapi)
    {
        _settings = ModSettings.Global.Feature<LogStructuresSetttings>();

        var command = sapi.ChatCommands
            .Create("slogger")
            .RequiresPrivilege(Privilege.controlserver)
            .WithDescription(T("Description"))
            .HandleWith(OnSloggerCommand);

        command
            .BeginSubCommand("vanilla")
                .WithAlias("v")
                .WithArgs(sapi.ChatCommands.Parsers.Bool("include", trueAlias: "on"))
                .WithDescription(T("IncludeVanillaResults.Description"))
                .HandleWith(IncludeVanillaResults)
            .EndSubCommand();

        command
            .BeginSubCommand("surface-ruins")
                .WithAlias("sr")
                .WithArgs(sapi.ChatCommands.Parsers.Bool("include", trueAlias: "on"))
                .WithDescription(T("IncludeSurfaceRuins.Description"))
                .HandleWith(IncludeSurfaceRuins)
            .EndSubCommand();


        command
            .BeginSubCommand("surface-structures")
                .WithAlias("ss")
                .WithArgs(sapi.ChatCommands.Parsers.Bool("include", trueAlias: "on"))
                .WithDescription(T("IncludeSurfaceStructures.Description"))
                .HandleWith(IncludeSurfaceStructures)
            .EndSubCommand();


        command
            .BeginSubCommand("underwater")
                .WithAlias("uw")
                .WithArgs(sapi.ChatCommands.Parsers.Bool("include", trueAlias: "on"))
                .WithDescription(T("IncludeUnderwaterStructures.Description"))
                .HandleWith(IncludeUnderwaterStructures)
            .EndSubCommand();


        command
            .BeginSubCommand("underground")
                .WithAlias("ug")
                .WithArgs(sapi.ChatCommands.Parsers.Bool("include", trueAlias: "on"))
                .WithDescription(T("IncludeUndergroundStructures.Description"))
                .HandleWith(IncludeUndergroundStructures)
            .EndSubCommand();
    }

    /// <summary>
    ///     Handles the root <c>/slogger</c> command and reports the current setting for filtering vanilla results.
    /// </summary>
    /// <param name="args">The command arguments and calling context.</param>
    /// <returns>A success or error result, depending on the availability of settings.</returns>
    private TextCommandResult OnSloggerCommand(TextCommandCallingArgs args)
    {
        if (_settings is null) return TextCommandResult.Error(T("Error"));

        var message = new StringBuilder()
            .AppendLine(T("IncludeVanillaResults.Value", _settings.IncludeVanillaResults))
            .AppendLine(T("IncludeSurfaceRuins.Value", _settings.IncludeSurfaceRuins))
            .AppendLine(T("IncludeSurfaceStructures.Value", _settings.IncludeSurfaceStructures))
            .AppendLine(T("IncludeUnderwaterStructures.Value", _settings.IncludeUnderwaterStructures))
            .AppendLine(T("IncludeUndergroundStructures.Value", _settings.IncludeUndergroundStructures))
            .ToString();

        return TextCommandResult.Success(message);
    }

    /// <summary>
    ///     Handles the command to include or exclude vanilla structures in logging output.
    /// </summary>
    /// <param name="args">The command arguments, including the boolean value indicating inclusion.</param>
    /// <returns>A result indicating whether the setting was updated successfully, or an error if settings are unavailable.</returns>
    private TextCommandResult IncludeVanillaResults(TextCommandCallingArgs args)
    {
        if (_settings is null) return TextCommandResult.Error(T("Error"));
        var include = args.Parsers[0].GetValue().To<bool>();
        _settings.IncludeVanillaResults = include;
        return TextCommandResult.Success(T("IncludeVanillaResults.Value", _settings.IncludeVanillaResults));
    }

    /// <summary>
    ///     Handles the command to include or exclude surface ruins in logging output.
    /// </summary>
    /// <param name="args">The command arguments, including the boolean value indicating inclusion.</param>
    /// <returns>A result indicating whether the setting was updated successfully, or an error if settings are unavailable.</returns>
    private TextCommandResult IncludeSurfaceRuins(TextCommandCallingArgs args)
    {
        if (_settings is null) return TextCommandResult.Error(T("Error"));
        var include = args.Parsers[0].GetValue().To<bool>();
        _settings.IncludeSurfaceRuins = include;
        return TextCommandResult.Success(T("IncludeSurfaceRuins.Value", _settings.IncludeSurfaceRuins));
    }

    /// <summary>
    ///     Handles the command to include or exclude surface structures in logging output.
    /// </summary>
    /// <param name="args">The command arguments, including the boolean value indicating inclusion.</param>
    /// <returns>A result indicating whether the setting was updated successfully, or an error if settings are unavailable.</returns>
    private TextCommandResult IncludeSurfaceStructures(TextCommandCallingArgs args)
    {
        if (_settings is null) return TextCommandResult.Error(T("Error"));
        var include = args.Parsers[0].GetValue().To<bool>();
        _settings.IncludeSurfaceStructures = include;
        return TextCommandResult.Success(T("IncludeSurfaceStructures.Value", _settings.IncludeSurfaceStructures));
    }

    /// <summary>
    ///     Handles the command to include or exclude underwater structures in logging output.
    /// </summary>
    /// <param name="args">The command arguments, including the boolean value indicating inclusion.</param>
    /// <returns>A result indicating whether the setting was updated successfully, or an error if settings are unavailable.</returns>
    private TextCommandResult IncludeUnderwaterStructures(TextCommandCallingArgs args)
    {
        if (_settings is null) return TextCommandResult.Error(T("Error"));
        var include = args.Parsers[0].GetValue().To<bool>();
        _settings.IncludeUnderwaterStructures = include;
        return TextCommandResult.Success(T("IncludeUnderwaterStructures.Value", _settings.IncludeUnderwaterStructures));
    }

    /// <summary>
    ///     Handles the command to include or exclude underground structures in logging output.
    /// </summary>
    /// <param name="args">The command arguments, including the boolean value indicating inclusion.</param>
    /// <returns>A result indicating whether the setting was updated successfully, or an error if settings are unavailable.</returns>
    private TextCommandResult IncludeUndergroundStructures(TextCommandCallingArgs args)
    {
        if (_settings is null) return TextCommandResult.Error(T("Error"));
        var include = args.Parsers[0].GetValue().To<bool>();
        _settings.IncludeUndergroundStructures = include;
        return TextCommandResult.Success(T("IncludeUndergroundStructures.Value", _settings.IncludeUndergroundStructures));
    }

    /// <summary>
    ///     Retrieves a localised string for the LogStructures feature using the specified path and formatting arguments.
    /// </summary>
    /// <param name="path">The key path to the localised string within the feature's language resource file.</param>
    /// <param name="args">Optional arguments to format into the resulting string, if placeholders exist.</param>
    /// <returns>The formatted, localised string associated with the specified path.</returns>
    private static string T(string path, params object[] args)
        => path switch
        {
            "Description" => "Better Ruins Structure Logger",
            "Error" => "Structure Logger Settings not found.",
            "IncludeVanillaResults.Description" => "Include vanilla structures that are generated.",
            "IncludeVanillaResults.Value" => $"Include Vanilla Results: {args[0]}",
            "IncludeSurfaceRuins.Description" => "Include ruins generated on the surface.",
            "IncludeSurfaceRuins.Value" => $"Include Surface Ruins: {args[0]}",
            "IncludeSurfaceStructures.Description" => "Include structures generated on the surface.",
            "IncludeSurfaceStructures.Value" => $"Include Surface Structures: {args[0]}",
            "IncludeUnderwaterStructures.Description" => "Include structures generated underwater.",
            "IncludeUnderwaterStructures.Value" => $"Include Underwater Structures: {args[0]}",
            "IncludeUndergroundStructures.Description" => "Include structures generated underground.",
            "IncludeUndergroundStructures.Value" => $"Include Underground Structures: {args[0]}",
            _ => path
        };
}