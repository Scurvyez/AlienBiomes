using HarmonyLib;
using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Linq;

namespace AlienBiomes
{
    [HarmonyPatch(typeof(GenStep_Terrain), "TerrainFrom")]
    public static class TerrainFrom_Patch
    {
        /// <summary>
        /// Checks to see if a biome has vanilla gravel.
        /// If so, change the current maps gravel to something else.
        /// </summary>
        [HarmonyPostfix]
        public static void ReplaceTerrain(Map map, ref TerrainDef __result)
        {
            // Checks for any Gravel terrain def and whether the current map is of biome type "SZ_RadiantPlains".
            if (__result == TerrainDefOf.Gravel && map.Biome.defName == "SZ_RadiantPlains" 
                && AlienBiomesSettings.AllowReplacingOfGravel == true)
            {
                // If the above check is true, terrain def "SZ_RadiantSoil" is used instead of Gravel.
                __result = TerrainDef.Named("SZ_RadiantSoil");
            }
            if (__result == TerrainDefOf.Gravel && map.Biome.defName == "SZ_CrystallineFlats" 
                && AlienBiomesSettings.AllowReplacingOfGravel == true)
            {
                __result = TerrainDef.Named("SZ_CrystallineStonySoil");
            }
        }
    }
}
