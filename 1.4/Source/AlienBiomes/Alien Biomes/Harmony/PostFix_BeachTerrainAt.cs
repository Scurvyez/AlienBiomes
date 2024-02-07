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
        /// If so, changes the current maps' sand to something else.
        /// </summary>
        [HarmonyPostfix]
        public static void ReplaceBeachTerrain(BiomeDef biome, ref TerrainDef __result)
        {
            if (!ModsConfig.IsActive("m00nl1ght.geologicallandforms"))
            {
                if (__result == TerrainDefOf.Sand && biome == ABDefOf.SZ_RadiantPlains
                && AlienBiomesSettings.UseAlienSand == true)
                // Checks for any Sand terrain def and whether the current map is of biome type "SZ_RadiantPlains".
                {
                    __result = TerrainDef.Named("SZ_SoothingSand");
                    // If the above check is true, terrain def "SZ_SoothingSand" is used instead of vanilla sand.
                }
                /*
                if (__result == TerrainDefOf.Sand && biome == ABDefOf.SZ_CrystallineFlats
                    && AlienBiomesSettings.UseAlienSand == true)
                {
                    __result = TerrainDef.Named("SZ_CrystallineSand");
                }
                */
            }
        }
    }
}
