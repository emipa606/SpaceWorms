using System.Linq;
using RimWorld;
using Verse;

namespace Scuttlebugs;

/// <summary>
///     Definition of the settings for the mod
/// </summary>
internal class ScuttlebugsSettings : ModSettings
{
    public bool ApplyToAllSpawnedPawnsAlly = true;
    public bool ApplyToAllSpawnedPawnsEnemy = true;
    public bool ApplyToOnlyHumanlike = true;
    public bool BlockReInfectingTheSource = true;
    public float ChanceOfInfectionBite = 25;
    public float IncidentChance = 1.5f;
    public float IncidentChanceForSpanwedPawnAlly = 2.5f;
    public float IncidentChanceForSpanwedPawnEnemy = 5.0f;


    public void ChangeDef()
    {
        Scuttlebugs_DefOf.ScuttlebugPodCrash.baseChance = IncidentChance;

        var incidentDef =
            DefDatabase<IncidentDef>.AllDefs.FirstOrDefault(x => x.defName.EqualsIgnoreCase("ScuttlebugPodCrash"));
        if (incidentDef != null)
        {
            incidentDef.baseChance = IncidentChance;
        }
    }

    /// <summary>
    ///     Saving and loading the values
    /// </summary>
    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref IncidentChance, "IncidentChance", 1.5f);
        Scribe_Values.Look(ref IncidentChanceForSpanwedPawnEnemy, "IncidentChanceForSpanwedPawnEnemy", 15f);
        Scribe_Values.Look(ref IncidentChanceForSpanwedPawnAlly, "IncidentChanceForSpanwedPawnAlly", 15f);
        Scribe_Values.Look(ref ChanceOfInfectionBite, "ChanceOfInfectionBite", 25f);
        Scribe_Values.Look(ref BlockReInfectingTheSource, "BlockReInfectingTheSource", true);
        Scribe_Values.Look(ref ApplyToAllSpawnedPawnsEnemy, "ApplyToAllSpawnedPawnsEnemy", true);
        Scribe_Values.Look(ref ApplyToAllSpawnedPawnsAlly, "ApplyToAllSpawnedPawnsAlly", true);
        Scribe_Values.Look(ref ApplyToOnlyHumanlike, "ApplyToOnlyHumanlike", true);
    }
}