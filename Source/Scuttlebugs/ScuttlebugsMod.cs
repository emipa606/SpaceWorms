using Mlie;
using UnityEngine;
using Verse;

namespace Scuttlebugs;

[StaticConstructorOnStartup]
internal class ScuttlebugsMod : Mod
{
    private static string currentVersion;

    /// <summary>
    ///     The private settings
    /// </summary>
    private ScuttlebugsSettings settings;

    /// <summary>
    ///     Cunstructor
    /// </summary>
    /// <param name="content"></param>
    public ScuttlebugsMod(ModContentPack content) : base(content)
    {
        settings = GetSettings<ScuttlebugsSettings>();
        currentVersion =
            VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    /// <summary>
    ///     The instance-settings for the mod
    /// </summary>
    internal ScuttlebugsSettings Settings
    {
        get
        {
            if (settings == null)
            {
                settings = GetSettings<ScuttlebugsSettings>();
            }

            return settings;
        }
        set => settings = value;
    }

    /// <summary>
    ///     The title for the mod-settings
    /// </summary>
    /// <returns></returns>
    public override string SettingsCategory()
    {
        return "Space Worms";
    }

    /// <summary>
    ///     The settings-window
    ///     For more info: https://rimworldwiki.com/wiki/Modding_Tutorials/ModSettings
    /// </summary>
    /// <param name="rect"></param>
    public override void DoSettingsWindowContents(Rect rect)
    {
        settings.ChangeDef();
        var listing_Standard = new Listing_Standard();
        listing_Standard.Begin(rect);
        listing_Standard.Label("SpWo.BaseChance".Translate(ScuttlebugsSettings.IncidentChance), -1,
            "SpWo.BaseChanceInfo".Translate());
        ScuttlebugsSettings.IncidentChance = Widgets.HorizontalSlider(listing_Standard.GetRect(20),
            ScuttlebugsSettings.IncidentChance, 0, 10f, false, "SpWo.Chance".Translate(), null, null, 0.01f);
        if (currentVersion != null)
        {
            listing_Standard.Gap();
            GUI.contentColor = Color.gray;
            listing_Standard.Label("SpWo.CurrentModVersion".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listing_Standard.End();
        settings.Write();
        settings.ChangeDef();
    }
}