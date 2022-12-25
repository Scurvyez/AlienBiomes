using Verse;
using HarmonyLib;

namespace AlienBiomes
{
    [StaticConstructorOnStartup]
    public static class AlienBiomesMain
    {
        static AlienBiomesMain()
        {
            Log.Message("<color=white>[</color>" + "<color=#4494E3FF>Steve</color>" + "<color=white>]</color>" +
                "<color=white>[</color>" + "<color=#4494E3FF>Alien</color>" + "<color=#4494E3FF>Biomes</color>" + "<color=white>]</color>" + "<color=#4494E3FF>Welcome, enjoy the ride!</color>");

            var harmony = new Harmony("com.alienbiomes");
            harmony.PatchAll();
        }
    }
}
