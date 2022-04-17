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
            // Gravel checks.
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

            // Sand checks.
            if (ModsConfig.IsActive("m00nl1ght.geologicallandforms"))
            {
                if (__result == TerrainDefOf.Sand && map.Biome.defName == "SZ_RadiantPlains"
                && AlienBiomesSettings.AllowReplacingOfSand == true)
                {
                    __result = TerrainDef.Named("SZ_SoothingSand");
                }
                if (__result == TerrainDefOf.Sand && map.Biome.defName == "SZ_CrystallineFlats"
                    && AlienBiomesSettings.AllowReplacingOfSand == true)
                {
                    __result = TerrainDef.Named("SZ_CrystallineSand");
                }
            }

            // Water checks.
            if (map.Biome.defName == "SZ_CrystallineFlats")
            {
                if (__result == TerrainDefOf.WaterShallow)
                {
                    __result = TerrainDef.Named("SZ_CrystallineWaterShallow");
                }
                if (__result == TerrainDefOf.WaterOceanShallow)
                {
                    __result = TerrainDef.Named("SZ_CrystallineWaterOceanShallow");
                }
                if (__result == TerrainDefOf.WaterOceanDeep)
                {
                    __result = TerrainDef.Named("SZ_CrystallineWaterOceanDeep");
                }
            }
            if (map.Biome.defName == "SZ_RadiantPlains")
            {
                if (__result == TerrainDefOf.WaterShallow)
                {
                    __result = TerrainDef.Named("SZ_RadiantWaterShallow");
                }
                if (__result == TerrainDefOf.WaterOceanShallow)
                {
                    __result = TerrainDef.Named("SZ_RadiantWaterOceanShallow");
                }
                if (__result == TerrainDefOf.WaterOceanDeep)
                {
                    __result = TerrainDef.Named("SZ_RadiantWaterOceanDeep");
                }
            }
        }
    }
}
