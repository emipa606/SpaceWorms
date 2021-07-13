using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace Scuttlebugs
{
    public class IncidentWorker_ScuttlebugPodCrash : IncidentWorker
    {
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            var map = (Map) parms.target;
            var things = ThingSetMakerDefOf.RefugeePod.root.Generate();
            var intVec = DropCellFinder.RandomDropSpot(map);
            var pawn = FindPawn(things);
            pawn.guest.getRescuedThoughtOnUndownedBecauseOfPlayer = true;
            var label = "LetterLabelRefugeePodCrash".Translate();
            var text = "RefugeePodCrash".Translate(pawn.Named("PAWN")).AdjustedFor(pawn);
            text += "\n\n";
            if (pawn.Faction == null)
            {
                text += "RefugeePodCrash_Factionless".Translate(pawn.Named("PAWN")).AdjustedFor(pawn);
            }
            else if (pawn.Faction.HostileTo(Faction.OfPlayer))
            {
                text += "RefugeePodCrash_Hostile".Translate(pawn.Named("PAWN")).AdjustedFor(pawn);
            }
            else
            {
                text += "RefugeePodCrash_NonHostile".Translate(pawn.Named("PAWN")).AdjustedFor(pawn);
            }

            PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref text, ref label, pawn);
            Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NeutralEvent, new TargetInfo(intVec, map));
            var activeDropPodInfo = new ActiveDropPodInfo();
            activeDropPodInfo.innerContainer.TryAddRangeOrTransfer(things);
            activeDropPodInfo.openDelay = 180;
            activeDropPodInfo.leaveSlag = true;
            DropPodUtility.MakeDropPodAt(intVec, map, activeDropPodInfo);

            //Give the pawn the Scuttlebug Infection
            var torso = pawn.health.hediffSet.GetNotMissingParts().First(bpr => bpr.def == BodyPartDefOf.Torso);
            var unused = pawn.health.hediffSet.GetNotMissingParts().First(bpr => bpr.def == BodyPartDefOf.Head);
            pawn.health.AddHediff(HediffDef.Named("ScuttlebugInfection"), torso);

            return true;
        }

        private Pawn FindPawn(List<Thing> things)
        {
            foreach (var thing in things)
            {
                if (thing is Pawn pawn)
                {
                    return pawn;
                }

                if (thing is Corpse corpse)
                {
                    return corpse.InnerPawn;
                }
            }

            return null;
        }
    }
}