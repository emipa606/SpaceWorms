using RimWorld.Planet;
using Verse;

namespace Scuttlebugs
{
    // Token: 0x0200000F RID: 15
    internal class WorldComp : WorldComponent
    {
        public WorldComp(World world) : base(world)
        {
        }

        public override void FinalizeInit()
        {
            base.FinalizeInit();
            Log.Message("Space Worms - Settings loaded");
            ScuttlebugsSettings.ChangeDefPost();
        }
    }
}