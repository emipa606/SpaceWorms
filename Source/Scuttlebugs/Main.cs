using System.Reflection;
using HarmonyLib;
using Verse;

namespace Scuttlebugs;

[StaticConstructorOnStartup]
public static class Main
{
    static Main()
    {
        new Harmony("Mlie.Scuttlebugs").PatchAll(Assembly.GetExecutingAssembly());
    }
}