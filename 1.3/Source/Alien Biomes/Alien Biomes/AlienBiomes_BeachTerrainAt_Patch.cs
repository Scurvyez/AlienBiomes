using HarmonyLib;
using RimWorld;
using Verse;

namespace AlienBiomes
{
    // This patch changes terrain types of beaches for several biomes.

    [HarmonyPatch(typeof(BeachMaker), "BeachTerrainAt")]
    public static class AlienBiomes_BeachTerrainAt_Patch
    {
        [HarmonyPostfix]
        public static void ReplaceBeachTerrain(BiomeDef biome, ref TerrainDef __result)
        {
            if ((__result == TerrainDefOf.Sand) && (biome.defName == "SZ_EnlightenedPlains"))
            // Checks for any Sand terrain def and whether the current map is of biome type "SZ_EnlightenedPlains".
            {
                __result = TerrainDef.Named("SZ_SoothingSand");
                // If the above check is true, terrain def "SZ_SoothingSand" is used instead of Gravel.
            }
        }
    }
}
