using System.Linq;
using RimWorld;
using Verse;

namespace Scuttlebugs
{
    /// <summary>
    ///     Definition of the settings for the mod
    /// </summary>
    internal class ScuttlebugsSettings : ModSettings
    {
        public static float IncidentChance = 1.5f;


        public void ChangeDef()
        {
            var list = DefDatabase<IncidentDef>.AllDefs.ToList();
            foreach (var incidentDef in list)
            {
                if (incidentDef.defName != "ScuttlebugPodCrash")
                {
                    continue;
                }

                incidentDef.baseChance = IncidentChance;
                return;
            }
        }

        public static void ChangeDefPost()
        {
            var list = DefDatabase<IncidentDef>.AllDefs.ToList();
            foreach (var incidentDef in list)
            {
                if (incidentDef.defName != "ScuttlebugPodCrash")
                {
                    continue;
                }

                incidentDef.baseChance = IncidentChance;
                return;
            }
        }

        /// <summary>
        ///     Saving and loading the values
        /// </summary>
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref IncidentChance, "IncidentChance", 1.5f);
        }
    }
}