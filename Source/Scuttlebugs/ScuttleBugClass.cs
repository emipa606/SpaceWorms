using Verse;

namespace Scuttlebugs;

public class ScuttleBugClass : Pawn
{
    public bool shouldDie;

    public override void Tick()
    {
        base.Tick();
        if (shouldDie)
        {
            Kill(null);
        }
    }
}