using Verse;
using HarmonyLib;

namespace AlienBiomes
{
    [StaticConstructorOnStartup]
    public static class AlienBiomesMain
    {
        static AlienBiomesMain()
        {
            Log.Message("<color=blue>[</color>" +
                "<color=cyan>Alien</color>" +
                "<color=cyan>Biomes</color>" +
                "<color=blue>]</color>" +
                " <color=cyan>Welcome, enjoy the ride!</color>");

            var harmony = new Harmony("com.alienbiomes");
            harmony.PatchAll();
        }
    }
}
