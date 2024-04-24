using RimWorld.Planet;
using Verse;

namespace Scuttlebugs;

internal class WorldComp(World world) : WorldComponent(world)
{
    public override void FinalizeInit()
    {
        base.FinalizeInit();
        Log.Message("Space Worms - Settings loaded");
        ScuttlebugsMod.Instance.Settings.ChangeDef();
    }
}