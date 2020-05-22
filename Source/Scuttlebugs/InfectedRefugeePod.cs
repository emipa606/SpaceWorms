using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace Scuttlebugs
{
    public class IncidentWorker_ScuttlebugPodCrash : IncidentWorker
    {
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            List<Thing> things = ThingSetMakerDefOf.RefugeePod.root.Generate();
            IntVec3 intVec = DropCellFinder.RandomDropSpot(map);
            Pawn pawn = this.FindPawn(things);
            pawn.guest.getRescuedThoughtOnUndownedBecauseOfPlayer = true;
            TaggedString label = "LetterLabelRefugeePodCrash".Translate();
            TaggedString text = "RefugeePodCrash".Translate(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN");
            text += "\n\n";
            if (pawn.Faction == null)
            {
                text += "RefugeePodCrash_Factionless".Translate(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN");
            }
            else if (pawn.Faction.HostileTo(Faction.OfPlayer))
            {
                text += "RefugeePodCrash_Hostile".Translate(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN");
            }
            else
            {
                text += "RefugeePodCrash_NonHostile".Translate(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN");
            }
            PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref text, ref label, pawn);
            Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NeutralEvent, new TargetInfo(intVec, map, false), null, null);
            ActiveDropPodInfo activeDropPodInfo = new ActiveDropPodInfo();
            activeDropPodInfo.innerContainer.TryAddRangeOrTransfer(things, true, false);
            activeDropPodInfo.openDelay = 180;
            activeDropPodInfo.leaveSlag = true;
            DropPodUtility.MakeDropPodAt(intVec, map, activeDropPodInfo);

            //Give the pawn the Scuttlebug Infection
            BodyPartRecord torso = pawn.health.hediffSet.GetNotMissingParts().First(bpr => bpr.def == BodyPartDefOf.Torso);
            BodyPartRecord head = pawn.health.hediffSet.GetNotMissingParts().First(bpr => bpr.def == BodyPartDefOf.Head);
            pawn.health.AddHediff(HediffDef.Named("ScuttlebugInfection"), torso, null);

            return true;
        }

        private Pawn FindPawn(List<Thing> things)
        {
            for (int i = 0; i < things.Count; i++)
            {
                Pawn pawn = things[i] as Pawn;
                if (pawn != null)
                {
                    return pawn;
                }
                Corpse corpse = things[i] as Corpse;
                if (corpse != null)
                {
                    return corpse.InnerPawn;
                }
            }
            return null;
        }

    }

}
