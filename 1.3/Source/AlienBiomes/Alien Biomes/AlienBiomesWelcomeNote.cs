using Verse;
using HarmonyLib;

namespace AlienBiomes
{
    [StaticConstructorOnStartup]
    public static class AlienBiomesMain
    {
        static AlienBiomesMain()
        {
            Log.Message("<color=white>[</color>" +
                "<color=cyan>Alien</color>" +
                "<color=cyan>Biomes</color>" +
                "<color=white>]</color>" +
                " <color=cyan>Welcome, enjoy the ride!</color>");

            var harmony = new Harmony("com.alienbiomes");
            harmony.PatchAll();
        }
    }
}
