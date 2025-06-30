using ProtoBuf;
using System.ComponentModel;

namespace ApacheTech.VintageMods.StructureLogger.Features.LogStructures.Settings;

/// <summary>
///     Provides configuration settings for the LogStructures feature.
/// </summary>
[ProtoContract]
public class LogStructuresSetttings : FeatureSettings<LogStructuresSetttings>
{
    /// <summary>
    ///     When enabled, include vanilla structures in the logging output.
    /// </summary>
    [ProtoMember(1), DefaultValue(true)]
    public bool IncludeVanillaResults { get; set; } = true;

    /// <summary>
    ///     When enabled, logs surface ruins placed by the structure generator.
    /// </summary>
    [ProtoMember(2), DefaultValue(true)]
    public bool IncludeSurfaceRuins { get; set; } = true;

    /// <summary>
    ///     When enabled, logs surface structures placed by the structure generator.
    /// </summary>
    [ProtoMember(3), DefaultValue(true)]
    public bool IncludeSurfaceStructures { get; set; } = true;

    /// <summary>
    ///     When enabled, logs underwater structures placed by the structure generator.
    /// </summary>
    [ProtoMember(4), DefaultValue(true)]
    public bool IncludeUnderwaterStructures { get; set; } = true;

    /// <summary>
    ///     When enabled, logs underground structures placed by the structure generator.
    /// </summary>
    [ProtoMember(5), DefaultValue(true)]
    public bool IncludeUndergroundStructures { get; set; } = true;
}