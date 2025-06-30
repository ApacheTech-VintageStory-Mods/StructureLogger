using ApacheTech.VintageMods.StructureLogger.Features.LogStructures.Settings;
using Gantry.Core.GameContent.GUI.Abstractions;
using Gantry.Services.Network.Extensions;

namespace ApacheTech.VintageMods.StructureLogger.Features.Dialogue;

/// <summary />
public class LogStructuresSetttingsDialogue : GenericDialogue
{
    private readonly LogStructuresSetttings _currentSettings;
    private readonly IClientNetworkChannel? _clientChannel;

    public LogStructuresSetttingsDialogue(ICoreClientAPI capi, LogStructuresSetttings currentSettings) : base(capi)
    {
        Title = "Better Ruins Structure Logger";
        Alignment = EnumDialogArea.CenterMiddle;
        Modal = true;
        ModalTransparency = 0.4f;
        _currentSettings = currentSettings;
        _clientChannel = capi.Network.GetDefaultChannel();
    }

    /// <summary>
    ///     Refreshes the displayed values on the form.
    /// </summary>
    protected override void RefreshValues()
    {
        SingleComposer.GetSwitch("IncludeVanillaResults").SetValue(_currentSettings.IncludeVanillaResults);
        SingleComposer.GetSwitch("IncludeSurfaceRuins").SetValue(_currentSettings.IncludeSurfaceRuins);
        SingleComposer.GetSwitch("IncludeSurfaceStructures").SetValue(_currentSettings.IncludeSurfaceStructures);
        SingleComposer.GetSwitch("IncludeUnderwaterStructures").SetValue(_currentSettings.IncludeUnderwaterStructures);
        SingleComposer.GetSwitch("IncludeUndergroundStructures").SetValue(_currentSettings.IncludeUndergroundStructures);
    }

    protected override void ComposeBody(GuiComposer composer)
    {
        var labelFont = CairoFont.WhiteSmallText();
        var topBounds = ElementBounds.FixedSize(600, 30);

        var left = ElementBounds.FixedSize(300, 30).FixedUnder(topBounds, 10);
        var right = ElementBounds.FixedSize(270, 30).FixedUnder(topBounds, 10).FixedRightOf(left, 10);

        var controlRowBoundsRightFixed = ElementBounds.FixedSize(100, 30).WithAlignment(EnumDialogArea.RightFixed);

        composer
            .AddStaticText(T("IncludeVanillaResults"), labelFont, EnumTextOrientation.Right, left)
            .AddHoverText(T("IncludeVanillaResults.HoverText"), labelFont, 260, left)
            .AddSwitch(IncludeVanillaResults, right, "IncludeVanillaResults");

        left = ElementBounds.FixedSize(300, 30).FixedUnder(left, 10);
        right = ElementBounds.FixedSize(270, 30).FixedUnder(right, 10).FixedRightOf(left, 10);

        composer
            .AddStaticText(T("IncludeSurfaceRuins"), labelFont, EnumTextOrientation.Right, left)
            .AddHoverText(T("IncludeSurfaceRuins.HoverText"), labelFont, 260, left)
            .AddSwitch(IncludeSurfaceRuins, right, "IncludeSurfaceRuins");

        left = ElementBounds.FixedSize(300, 30).FixedUnder(left, 10);
        right = ElementBounds.FixedSize(270, 30).FixedUnder(right, 10).FixedRightOf(left, 10);

        composer
            .AddStaticText(T("IncludeSurfaceStructures"), labelFont, EnumTextOrientation.Right, left)
            .AddHoverText(T("IncludeSurfaceRuins.IncludeSurfaceStructures"), labelFont, 260, left)
            .AddSwitch(IncludeSurfaceStructures, right, "IncludeSurfaceStructures");

        left = ElementBounds.FixedSize(300, 30).FixedUnder(left, 10);
        right = ElementBounds.FixedSize(270, 30).FixedUnder(right, 10).FixedRightOf(left, 10);

        composer
            .AddStaticText(T("IncludeUnderwaterStructures"), labelFont, EnumTextOrientation.Right, left)
            .AddHoverText(T("IncludeUnderwaterStructures.HoverText"), labelFont, 260, left)
            .AddSwitch(IncludeUnderwaterStructures, right, "IncludeUnderwaterStructures");

        left = ElementBounds.FixedSize(300, 30).FixedUnder(left, 10);
        right = ElementBounds.FixedSize(270, 30).FixedUnder(right, 10).FixedRightOf(left, 10);

        composer
            .AddStaticText(T("IncludeUndergroundStructures"), labelFont, EnumTextOrientation.Right, left)
            .AddHoverText(T("IncludeUndergroundStructures.HoverText"), labelFont, 260, left)
            .AddSwitch(IncludeUndergroundStructures, right, "IncludeUndergroundStructures");

        left = ElementBounds.FixedSize(300, 30).FixedUnder(left, 10);
        right = ElementBounds.FixedSize(270, 30).FixedUnder(right, 10).FixedRightOf(left, 10);

        composer
            .AddSmallButton("OK", OnOkButtonPressed, controlRowBoundsRightFixed.FixedUnder(right, 10));
    }

    private void IncludeVanillaResults(bool state)
    {
        _currentSettings.IncludeVanillaResults = state;
    }

    private void IncludeSurfaceRuins(bool state)
    {
        _currentSettings.IncludeSurfaceRuins = state;
    }

    private void IncludeSurfaceStructures(bool state)
    {
        _currentSettings.IncludeSurfaceStructures = state;
    }

    private void IncludeUnderwaterStructures(bool state)
    {
        _currentSettings.IncludeUnderwaterStructures = state;
    }

    private void IncludeUndergroundStructures(bool state)
    {
        _currentSettings.IncludeUndergroundStructures = state;
    }

    private bool OnOkButtonPressed()
    {
        _clientChannel?.SendPacket(_currentSettings);
        return TryClose();
    }

    private static string T(string path, params object[] _)
        => path switch
        {
            "IncludeVanillaResults" => $"Include Vanilla Results",
            "IncludeVanillaResults.HoverText" => "Include vanilla structures that are generated.",
            "IncludeSurfaceRuins" => $"Include Surface Ruins",
            "IncludeSurfaceRuins.HoverText" => "Include ruins generated on the surface.",
            "IncludeSurfaceStructures" => $"Include Surface Structures",
            "IncludeSurfaceStructures.HoverText" => "Include structures generated on the surface.",
            "IncludeUnderwaterStructures" => $"Include Underwater Structures",
            "IncludeUnderwaterStructures.HoverText" => "Include structures generated underwater.",
            "IncludeUndergroundStructures" => $"Include Underground Structures",
            "IncludeUndergroundStructures.HoverText" => "Include structures generated underground.",
            _ => path
        };
}