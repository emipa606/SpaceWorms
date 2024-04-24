using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace Scuttlebugs;

public class ScuttlebugsHediff : HediffWithComps
{
    private int iCounter;

    public override bool Visible => Scuttlebugs_DefOf.ScuttlebugsBiology.IsFinished;

    public static void SpawnScuttlebug(Pawn pawn)
    {
        var torso = pawn.health.hediffSet.GetNotMissingParts().First(bpr => bpr.def == BodyPartDefOf.Torso);

        var noWorms = Random.Range(1, 3);

        if (pawn.MapHeld != null)
        {
            for (var i = 0; i < noWorms; i++)
            {
                var scuttlebug = Scuttlebugs_DefOf.Scuttlebug;
                var newPawn = PawnGenerator.GeneratePawn(scuttlebug);
                if (newPawn is ScuttleBugClass worm) // Set the "Source" of this bug
                {
                    worm.cause = pawn;
                }


                GenSpawn.Spawn(newPawn, pawn.Position, pawn.MapHeld);
                if (newPawn is ScuttleBugClass
                    {
                        cause: null
                    } worm2) // Fuck knows, sometimes the first one does not actually keep the value >.> maybe cause before spawn?
                {
                    worm2.cause = pawn;
                }

                newPawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent);
            }
        }

        //remove the parasite infection before hurting the colonist
        pawn.health.RemoveHediff(
            pawn.health.hediffSet.GetFirstHediffOfDef(Scuttlebugs_DefOf.ScuttlebugInfection));
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