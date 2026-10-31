using System;
using System.Linq;
using RimWorld;
using Verse;

namespace Scuttlebugs;

public class Scuttlebugs_DamageWorker : DamageWorker
{
    //public override float Apply(DamageInfo dinfo, Thing thing)
    public override DamageResult Apply(DamageInfo dinfo, Thing victim)
    {
        if (victim is not Pawn pawn)
        {
            return base.Apply(dinfo, victim);
        }

        _ = pawn;

        var result = base.Apply(dinfo, pawn);
        if (dinfo.Instigator is not ScuttleBugClass worm)
        {
            return result;
        }

        // Don't re-infect the original cause of this worm!
        if (worm.cause == null)
        {
            return result;
        }

        if (ScuttlebugsMod.Instance.Settings.BlockReInfectingTheSource && worm.cause == victim)
        {
            return result;
        }

        if (ScuttlebugsMod.Instance.Settings.ApplyToOnlyHumanlike && pawn.RaceProps is { Humanlike: false })
        {
            return result; // Dont infect non humanlike
        }

        if (pawn.Dead || pawn.IsAwokenCorpse || pawn.RaceProps is { IsMechanoid: true })
        {
            return result; // AI can do some stupid shit + also block mechanoids from being infected >.>
        }


        // Change re-infect to chance based not instant
        if (ScuttlebugsMod.Instance.Settings.ChanceOfInfectionBite < 100 &&
            !Rand.Chance(Math.Max(0.01f, ScuttlebugsMod.Instance.Settings.ChanceOfInfectionBite / 100f)))
        {
            return result;
        }


        var torso = pawn.health.hediffSet.GetNotMissingParts()
            .FirstOrDefault(bpr => bpr.def == BodyPartDefOf.Torso);
        if (torso == null) // This can be null due to some mods doing some super hacky shit, safety first
        {
            return result;
        }

        // If we are here, kill the worm and infect the target
        worm.shouldDie = true;
        pawn.health.AddHediff(Scuttlebugs_DefOf.ScuttlebugInfection, torso);
        if (!pawn.Downed)
        {
            HealthUtility.DamageUntilDowned(pawn); // Don't re-down the player if not needed waste of cpu cycles
        }

        return result;
    }
}