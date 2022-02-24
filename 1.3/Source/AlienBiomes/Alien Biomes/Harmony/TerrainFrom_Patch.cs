using HarmonyLib;
using RimWorld;
using Verse;

namespace AlienBiomes
{
    [HarmonyPatch(typeof(GenStep_Terrain), "TerrainFrom")]
    public static class TerrainFrom_Patch
    {
        /// <summary>
        /// Checks to see if a biome has vanilla gravel.
        /// If so, change all vanilla gravel to radiant soil.
        /// </summary>
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
