using RimWorld;
using Verse;

namespace Scuttlebugs
{
    //Scuttlebug HediffDefOf - need this to be able to check if the pawn has the hediff
    [DefOf]
    public static class Scuttlebugs_HediffDefOf
    {
        public static HediffDef ScuttlebugInfection;
        public static HediffDef ScuttlebugQueenInfection;
    }

    //Kill the Scuttlebug once it's transmitted the virus

    //Scuttlebug PawnKindDef - need this to be able to spawn it later

    //The Impregnation Hediff - runs down on a 24hr tick and then births another Scuttlebug
}