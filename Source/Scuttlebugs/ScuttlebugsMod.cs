using Mlie;
using UnityEngine;
using Verse;

namespace Scuttlebugs;

[StaticConstructorOnStartup]
internal class ScuttlebugsMod : Mod
{
    private static string currentVersion;
    public static ScuttlebugsMod Instance;

    /// <summary>
    ///     Cunstructor
    /// </summary>
    /// <param name="content"></param>
    public ScuttlebugsMod(ModContentPack content) : base(content)
    {
        Instance = this;
        Settings = GetSettings<ScuttlebugsSettings>();
        currentVersion = VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    /// <summary>
    ///     The private settings
    /// </summary>
    internal ScuttlebugsSettings Settings { get; }


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
        var listing_Standard = new Listing_Standard();
        listing_Standard.Begin(rect);
        listing_Standard.Label("SpWo.BaseChance".Translate(Settings.IncidentChance), -1,
            "SpWo.BaseChanceInfo".Translate());
        Settings.IncidentChance = Widgets.HorizontalSlider(listing_Standard.GetRect(20),
            Settings.IncidentChance, 0, 10f, false, "SpWo.Chance".Translate(), null, null, 0.01f);

        listing_Standard.Gap();
        listing_Standard.CheckboxLabeled("SpWo.SpawnPawnEnemy".Translate(),
            ref Settings.ApplyToAllSpawnedPawnsEnemy, "SpWo.SpawnPawnEnemyInfo".Translate());
        if (Settings.ApplyToAllSpawnedPawnsEnemy)
        {
            listing_Standard.Label(
                "SpWo.SpawnPawnEnemyChance".Translate(Settings.IncidentChanceForSpanwedPawnEnemy), -1,
                "SpWo.SpawnPawnEnemyChanceInfo".Translate());
            Settings.IncidentChanceForSpanwedPawnEnemy = Widgets.HorizontalSlider(listing_Standard.GetRect(20),
                Settings.IncidentChanceForSpanwedPawnEnemy, 0, 100f, false, "SpWo.Chance".Translate(), null,
                null, 0.01f);
        }

        listing_Standard.Gap();
        listing_Standard.CheckboxLabeled("SpWo.SpawnPawnAlly".Translate(),
            ref Settings.ApplyToAllSpawnedPawnsAlly, "SpWo.SpawnPawnAllyInfo".Translate());
        if (Settings.ApplyToAllSpawnedPawnsAlly)
        {
            listing_Standard.Label(
                "SpWo.SpawnPawnAllyChance".Translate(Settings.IncidentChanceForSpanwedPawnAlly), -1,
                "SpWo.SpawnPawnAllyChanceInfo".Translate());
            Settings.IncidentChanceForSpanwedPawnAlly = Widgets.HorizontalSlider(listing_Standard.GetRect(20),
                Settings.IncidentChanceForSpanwedPawnAlly, 0, 100f, false, "SpWo.Chance".Translate(), null, null,
                0.01f);
        }

        listing_Standard.Gap();
        listing_Standard.CheckboxLabeled("SpWo.ApplyToOnlyHumanlike".Translate(),
            ref Settings.ApplyToOnlyHumanlike, "SpWo.ApplyToOnlyHumanlikeInfo".Translate());


        listing_Standard.Gap();
        listing_Standard.CheckboxLabeled("SpWo.PreventInfectSource".Translate(),
            ref Settings.BlockReInfectingTheSource, "SpWo.PreventInfectSourceInfo".Translate());

        listing_Standard.Gap();
        listing_Standard.Label("SpWo.ChanceOfInfectionBite".Translate(Settings.ChanceOfInfectionBite), -1,
            "SpWo.ChanceOfInfectionBiteInfo".Translate());
        Settings.ChanceOfInfectionBite = Widgets.HorizontalSlider(listing_Standard.GetRect(20),
            Settings.ChanceOfInfectionBite, 0, 100f, false, "SpWo.Chance".Translate(), null, null, 0.01f);


        if (currentVersion != null)
        {
            listing_Standard.Gap();
            GUI.contentColor = Color.gray;
            listing_Standard.Label("SpWo.CurrentModVersion".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listing_Standard.End();
    }

    public override void WriteSettings()
    {
        base.WriteSettings();
        Settings.ChangeDef();
    }
}