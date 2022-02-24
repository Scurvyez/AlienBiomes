using HarmonyLib;
using RimWorld;
using Verse;

namespace AlienBiomes
{
    [HarmonyPatch(typeof(BeachMaker), "BeachTerrainAt")]
    public static class BeachTerrainAt_Patch
    {
        /// <summary>
        /// Checks to see if a biome has vanilla sand.
        /// If so, change all vanilla sand to soothing sand.
        /// </summary>
        [HarmonyPostfix]
        public static void ReplaceBeachTerrain(BiomeDef biome, ref TerrainDef __result)
        {
            if ((__result == TerrainDefOf.Sand) && (biome.defName == "SZ_RadiantPlains"))
            // Checks for any Sand terrain def and whether the current map is of biome type "SZ_RadiantPlains".
            {
                __result = TerrainDef.Named("SZ_SoothingSand");
                // If the above check is true, terrain def "SZ_SoothingSand" is used instead of Gravel.
            }
        }
    }
}
