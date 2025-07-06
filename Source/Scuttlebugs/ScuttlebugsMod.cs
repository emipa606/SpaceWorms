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
        var listingStandard = new Listing_Standard();
        listingStandard.Begin(rect);
        listingStandard.Label("SpWo.BaseChance".Translate(Settings.IncidentChance), -1,
            "SpWo.BaseChanceInfo".Translate());
        Settings.IncidentChance = Widgets.HorizontalSlider(listingStandard.GetRect(20),
            Settings.IncidentChance, 0, 10f, false, "SpWo.Chance".Translate(), null, null, 0.01f);

        listingStandard.Gap();
        listingStandard.CheckboxLabeled("SpWo.SpawnPawnEnemy".Translate(),
            ref Settings.ApplyToAllSpawnedPawnsEnemy, "SpWo.SpawnPawnEnemyInfo".Translate());
        if (Settings.ApplyToAllSpawnedPawnsEnemy)
        {
            listingStandard.Label(
                "SpWo.SpawnPawnEnemyChance".Translate(Settings.IncidentChanceForSpawnedPawnEnemy), -1,
                "SpWo.SpawnPawnEnemyChanceInfo".Translate());
            Settings.IncidentChanceForSpawnedPawnEnemy = Widgets.HorizontalSlider(listingStandard.GetRect(20),
                Settings.IncidentChanceForSpawnedPawnEnemy, 0, 100f, false, "SpWo.Chance".Translate(), null,
                null, 0.01f);
        }

        listingStandard.Gap();
        listingStandard.CheckboxLabeled("SpWo.SpawnPawnAlly".Translate(),
            ref Settings.ApplyToAllSpawnedPawnsAlly, "SpWo.SpawnPawnAllyInfo".Translate());
        if (Settings.ApplyToAllSpawnedPawnsAlly)
        {
            listingStandard.Label(
                "SpWo.SpawnPawnAllyChance".Translate(Settings.IncidentChanceForSpawnedPawnAlly), -1,
                "SpWo.SpawnPawnAllyChanceInfo".Translate());
            Settings.IncidentChanceForSpawnedPawnAlly = Widgets.HorizontalSlider(listingStandard.GetRect(20),
                Settings.IncidentChanceForSpawnedPawnAlly, 0, 100f, false, "SpWo.Chance".Translate(), null, null,
                0.01f);
        }

        listingStandard.Gap();
        listingStandard.CheckboxLabeled("SpWo.ApplyToOnlyHumanlike".Translate(),
            ref Settings.ApplyToOnlyHumanlike, "SpWo.ApplyToOnlyHumanlikeInfo".Translate());


        listingStandard.Gap();
        listingStandard.CheckboxLabeled("SpWo.PreventInfectSource".Translate(),
            ref Settings.BlockReInfectingTheSource, "SpWo.PreventInfectSourceInfo".Translate());

        listingStandard.Gap();
        listingStandard.Label("SpWo.ChanceOfInfectionBite".Translate(Settings.ChanceOfInfectionBite), -1,
            "SpWo.ChanceOfInfectionBiteInfo".Translate());
        Settings.ChanceOfInfectionBite = Widgets.HorizontalSlider(listingStandard.GetRect(20),
            Settings.ChanceOfInfectionBite, 0, 100f, false, "SpWo.Chance".Translate(), null, null, 0.01f);


        if (currentVersion != null)
        {
            listingStandard.Gap();
            GUI.contentColor = Color.gray;
            listingStandard.Label("SpWo.CurrentModVersion".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listingStandard.End();
    }

    public override void WriteSettings()
    {
        base.WriteSettings();
        Settings.ChangeDef();
    }
}