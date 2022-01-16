using HarmonyLib;
using RimWorld;
using Verse;

namespace AlienBiomes
{
    // This patch changes terrain types for several biomes.

    [HarmonyPatch(typeof(GenStep_Terrain), "TerrainFrom")]
    public static class AlienBiomes_TerrainFrom_Patch
    {
        [HarmonyPostfix]
        public static void ReplaceTerrain(Map map, ref TerrainDef __result)
        {
            if ((__result == TerrainDefOf.Gravel) && (map.Biome.defName == "SZ_RadiantPlains"))
            // Checks for any Gravel terrain def and whether the current map is of biome type "SZ_RadiantPlains".
            {
                __result = TerrainDef.Named("SZ_RadiantSoil");
                // If the above check is true, terrain def "SZ_RadiantSoil" is used instead of Gravel.
            }
        }
    }
}
