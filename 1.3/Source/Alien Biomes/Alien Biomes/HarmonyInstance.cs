using HarmonyLib;
using System.Reflection;
using Verse;

namespace AlienBiomes
{
    [StaticConstructorOnStartup]
    public class Main
    {
        static Main()
        {
            var harmony = new Harmony("com.alienbiomes");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
