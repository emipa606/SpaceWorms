using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace Scuttlebugs;

public class ScuttlebugsHediff : HediffWithComps
{
    private int iCounter;


    public override bool Visible => ResearchProjectDef.Named("ScuttlebugsBiology").IsFinished;

    public static void SpawnScuttlebug(Pawn pawn)
    {
        var ScuttlebugInfection = DefDatabase<HediffDef>.GetNamed("ScuttlebugInfection");
        var torso = pawn.health.hediffSet.GetNotMissingParts().First(bpr => bpr.def == BodyPartDefOf.Torso);

        //Log.Warning(pawn + "has spawned a Scuttlebug!");

        var noWorms = Random.Range(1, 3);

        for (var i = 0; i < noWorms; i++)
        {
            var scuttlebug = Scuttlebugs_DefOf.Scuttlebug;
            var newPawn = PawnGenerator.GeneratePawn(scuttlebug);

            GenSpawn.Spawn(newPawn, pawn.Position, pawn.MapHeld);
            newPawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent);
        }

        //remove the parasite infection before killing the colonist
        pawn.health.RemoveHediff(pawn.health.hediffSet.GetFirstHediffOfDef(ScuttlebugInfection));
        pawn.health.DropBloodFilth();

        pawn.TakeDamage(new DamageInfo(DamageDefOf.Bite, 50, 100, -1, null, torso));
    }

    public override void Tick()
    {
        //ensure larvae spawns if colonist is killed
        ++iCounter;
        if (iCounter > 120000)
        {
            SpawnScuttlebug(pawn);
        }

        base.Tick();
    }
}