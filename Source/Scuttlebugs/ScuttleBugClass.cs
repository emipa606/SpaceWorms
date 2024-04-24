using Verse;

namespace Scuttlebugs;

public class ScuttleBugClass : Pawn
{
    public bool shouldDie;

    public Pawn cause = null; // Added the cause, (this can be null) of the bug used for preventing re-infection of the source

    public override void Tick()
    {
        base.Tick();
        if (shouldDie)
        {
            Kill(null);
        }
    }
}
