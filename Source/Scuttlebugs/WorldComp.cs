using RimWorld.Planet;
using Verse;

namespace Scuttlebugs;

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