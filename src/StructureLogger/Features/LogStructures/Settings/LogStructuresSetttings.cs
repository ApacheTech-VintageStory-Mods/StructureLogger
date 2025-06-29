namespace ApacheTech.VintageMods.StructureLogger.Features.LogStructures.Settings;

/// <summary>
///     Provides configuration settings for the LogStructures feature.
/// </summary>
public class LogStructuresSetttings : FeatureSettings<LogStructuresSetttings>
{   
    /// <summary>
    ///     When enabled, include vanilla structures in the logging output. This can log millions of lines to file.
    /// </summary>
    public bool IncludeVanillaResults { get; set; } = false;

    /// <summary>
    ///     When enabled, logs surface ruins placed by the structure generator.
    /// </summary>
    public bool IncludeSurfaceRuins { get; set; } = true;

    /// <summary>
    ///     When enabled, logs surface structures placed by the structure generator.
    /// </summary>
    public bool IncludeSurfaceStructures { get; set; } = true;

    /// <summary>
    ///     When enabled, logs underwater structures placed by the structure generator.
    /// </summary>
    public bool IncludeUnderwaterStructures { get; set; } = true;

    /// <summary>
    ///     When enabled, logs underground structures placed by the structure generator.
    /// </summary>
    public bool IncludeUndergroundStructures { get; set; } = true;
}