using System;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Scuttlebugs;

[HarmonyPatch(typeof(PawnGenerator), nameof(PawnGenerator.GeneratePawn), typeof(PawnGenerationRequest))]
public class PawnGenerator_GeneratePawn
{
    private static bool ShouldApply(Pawn pawn)
    {
        if (pawn == null || Current.Game == null || pawn.IsPlayerControlled || pawn.Dead || pawn.IsAwokenCorpse ||
            pawn.RaceProps is { IsMechanoid: true })
        {
            return false; // Skip invalid, during setup, and player controlled pawns to avoid fucking up game starts
        }

        if (ScuttlebugsMod.Instance.Settings.ApplyToOnlyHumanlike && pawn.RaceProps is { Humanlike: false })
        {
            return false; // Dont infect non humanlike
        }

        if (pawn.health?.hediffSet == null ||
            pawn.health.hediffSet.hediffs.Any(x => x.def == Scuttlebugs_DefOf.ScuttlebugInfection))
        {
            return false; // already infected don't re-infect
        }

        if (pawn.HostileTo(Find.FactionManager.OfPlayer)) // Enemy Pawns
        {
            if (!ScuttlebugsMod.Instance.Settings.ApplyToAllSpawnedPawnsEnemy &&
                ScuttlebugsMod.Instance.Settings.IncidentChanceForSpanwedPawnEnemy > 0)
            {
                return false;
            }

            return Rand.Chance(Math.Max(0.01f,
                ScuttlebugsMod.Instance.Settings.IncidentChanceForSpanwedPawnEnemy / 100f));
        }

        // Allied or friendly pawns
        if (!ScuttlebugsMod.Instance.Settings.ApplyToAllSpawnedPawnsAlly &&
            ScuttlebugsMod.Instance.Settings.IncidentChanceForSpanwedPawnAlly > 0)
        {
            return false;
        }

        return Rand.Chance(Math.Max(0.01f, ScuttlebugsMod.Instance.Settings.IncidentChanceForSpanwedPawnAlly / 100f));
    }

    public static void Postfix(ref Pawn __result)
    {
        if (!ShouldApply(__result))
        {
            return;
        }

        var bodyPartToInfect = __result.health.hediffSet.GetNotMissingParts()
            .FirstOrDefault(bpr => bpr.def == BodyPartDefOf.Torso);
        if (bodyPartToInfect != null)
        {
            __result.health.AddHediff(Scuttlebugs_DefOf.ScuttlebugInfection, bodyPartToInfect);
        }
    }
}