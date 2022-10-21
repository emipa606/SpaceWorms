using System.Linq;
using RimWorld;
using Verse;

namespace Scuttlebugs;

public class Scuttlebugs_DamageWorker : DamageWorker
{
    //public override float Apply(DamageInfo dinfo, Thing thing)
    public override DamageResult Apply(DamageInfo dinfo, Thing thing)
    {
        if (!(thing is Pawn pawn))
        {
            return base.Apply(dinfo, thing);
        }

        //return base.Apply(dinfo, thing);
        var unused = pawn; //scuttlebug Pawn

        var result = base.Apply(dinfo, pawn);

        Log.Warning(pawn + "has been infected!");
        var torso = pawn.health.hediffSet.GetNotMissingParts().First(bpr => bpr.def == BodyPartDefOf.Torso);

        pawn.health.AddHediff(HediffDef.Named("ScuttlebugInfection"), torso);

        HealthUtility.DamageUntilDowned(pawn);

        if (dinfo.Instigator is ScuttleBugClass worm)
        {
            worm.shouldDie = true;
        }

        return result;
    }
}