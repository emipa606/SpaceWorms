using RimWorld.Planet;

namespace Scuttlebugs;

internal class WorldComp(World world) : WorldComponent(world)
{
    public override void FinalizeInit(bool fromLoad)
    {
        base.FinalizeInit(fromLoad);
        ScuttlebugsMod.Instance.Settings.ChangeDef();
    }
}