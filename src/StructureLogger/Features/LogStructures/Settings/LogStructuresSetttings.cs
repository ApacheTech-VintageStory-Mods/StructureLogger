namespace ApacheTech.VintageMods.StructureLogger.Features.LogStructures.Settings;

/// <summary>
///     Provides configuration settings for the LogStructures feature, including whether to
///     exclude vanilla-generated structures from output.
/// </summary>
public class LogStructuresSetttings : FeatureSettings<LogStructuresSetttings>
{
    /// <summary>
    ///     When enabled, prevents vanilla structures from being included in the structure logging output.
    /// </summary>
    public bool FilterVanillaResults { get; set; } = true;
}