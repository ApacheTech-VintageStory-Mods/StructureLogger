using ApacheTech.VintageMods.StructureLogger.Features.LogStructures.Settings;
using Gantry.Services.FileSystem.Configuration.Consumers;
using Vintagestory.API.MathTools;
using Vintagestory.ServerMods;

namespace ApacheTech.VintageMods.StructureLogger.Features.LogStructures.Patches;

/// <summary>
///     Contains Harmony patches to log structure generation events performed by the vanilla
///     <see cref="WorldGenStructure" /> class, restricted to those originating from BetterRuins.
/// </summary>
[HarmonyServerSidePatch]
public class WorldGenStructurePatches : GlobalSettingsConsumer<LogStructuresSetttings>
{
    /// <summary>
    ///     Logs when a ruin is successfully generated on the surface.
    /// </summary>
    /// <param name="__result">The result of the original method, indicating success or failure.</param>
    /// <param name="worldForCollectibleResolve">The world accessor used by the world generation process.</param>
    /// <param name="___LastPlacedSchematic">The last schematic placed by the structure generator.</param>
    /// <param name="___LastPlacedSchematicLocation">The location where the schematic was placed.</param>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorldGenStructure), "TryGenerateRuinAtSurface")]
    public static void LogWhenRuinGeneratedOnSurface(bool __result, IWorldAccessor worldForCollectibleResolve,
        BlockSchematicStructure ___LastPlacedSchematic, Cuboidi ___LastPlacedSchematicLocation)
    {
        if (!__result) return;
        if (Settings?.IncludeSurfaceRuins != true) return;

        Log(
            structureType: "surface ruin",
            api: worldForCollectibleResolve.Api,
            schematic: ___LastPlacedSchematic,
            location: ___LastPlacedSchematicLocation);
    }

    /// <summary>
    ///     Logs when a generic structure is successfully generated on the surface.
    /// </summary>
    /// <param name="__result">The result of the original method, indicating success or failure.</param>
    /// <param name="worldForCollectibleResolve">The world accessor used by the world generation process.</param>
    /// <param name="___LastPlacedSchematic">The last schematic placed by the structure generator.</param>
    /// <param name="___LastPlacedSchematicLocation">The location where the schematic was placed.</param>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorldGenStructure), "TryGenerateAtSurface")]
    public static void LogWhenStructureGeneratedOnSurface(bool __result, IWorldAccessor worldForCollectibleResolve,
        BlockSchematicStructure ___LastPlacedSchematic, Cuboidi ___LastPlacedSchematicLocation)
    {
        if (!__result) return;
        if (Settings?.IncludeSurfaceStructures != true) return;

        Log(
            structureType: "surface structure",
            api: worldForCollectibleResolve.Api,
            schematic: ___LastPlacedSchematic,
            location: ___LastPlacedSchematicLocation);
    }

    /// <summary>
    ///     Logs when a structure is successfully generated underwater.
    /// </summary>
    /// <param name="__result">The result of the original method, indicating success or failure.</param>
    /// <param name="worldForCollectibleResolve">The world accessor used by the world generation process.</param>
    /// <param name="___LastPlacedSchematic">The last schematic placed by the structure generator.</param>
    /// <param name="___LastPlacedSchematicLocation">The location where the schematic was placed.</param>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorldGenStructure), "TryGenerateUnderwater")]
    public static void LogWhenStructureGeneratedUnderwater(bool __result, IWorldAccessor worldForCollectibleResolve,
        BlockSchematicStructure ___LastPlacedSchematic, Cuboidi ___LastPlacedSchematicLocation)
    {
        if (!__result) return;
        if (Settings?.IncludeUnderwaterStructures != true) return;

        Log(
            structureType: "underwater structure",
            api: worldForCollectibleResolve.Api,
            schematic: ___LastPlacedSchematic,
            location: ___LastPlacedSchematicLocation);
    }

    /// <summary>
    ///     Logs when a structure is successfully generated underground.
    /// </summary>
    /// <param name="__result">The result of the original method, indicating success or failure.</param>
    /// <param name="worldForCollectibleResolve">The world accessor used by the world generation process.</param>
    /// <param name="___LastPlacedSchematic">The last schematic placed by the structure generator.</param>
    /// <param name="___LastPlacedSchematicLocation">The location where the schematic was placed.</param>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorldGenStructure), "TryGenerateUnderground")]
    public static void LogWhenStructureGeneratedUnderground(bool __result, IWorldAccessor worldForCollectibleResolve,
        BlockSchematicStructure ___LastPlacedSchematic, Cuboidi ___LastPlacedSchematicLocation)
    {
        if (!__result) return;
        if (Settings?.IncludeUndergroundStructures != true) return;

        Log(
            structureType: "underground structure",
            api: worldForCollectibleResolve.Api,
            schematic: ___LastPlacedSchematic,
            location: ___LastPlacedSchematicLocation);
    }

    /// <summary>
    ///     Logs the generation of a structure if it meets configured logging conditions, such as being part of the BetterRuins mod,
    ///     or bypassing the filter if disabled.
    /// </summary>
    /// <param name="structureType">The type of structure being generated.</param>
    /// <param name="api">The core API used to access assets, logging, and mod configuration.</param>
    /// <param name="schematic">The schematic structure that was placed during world generation.</param>
    /// <param name="location">The bounding box representing the location where the schematic was placed.</param>
    private static void Log(string structureType, ICoreAPI api, BlockSchematicStructure schematic, Cuboidi location)
    {
        var fileName = schematic.FromFileName;

        var (assetLocation, asset) = api.Assets.AllAssets.FirstOrDefault(p => p.Key.GetName() == fileName);
        if (assetLocation is null) return;

        if (Settings?.IncludeVanillaResults != true && !asset.Origin.OriginPath.Contains("BetterRuins")) return;

        var message = $"[BetterRuins] Generating {structureType} - File: {assetLocation.ToShortString()} - Location: {location}";
        api.Logger.Audit(message);
    }
}