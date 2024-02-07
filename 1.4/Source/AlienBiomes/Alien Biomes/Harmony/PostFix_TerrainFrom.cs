using HarmonyLib;
using RimWorld;
using Verse;

namespace AlienBiomes
{
    [HarmonyPatch(typeof(GenStep_Terrain), "TerrainFrom")]
    public static class TerrainFrom_Patch
    {
        /// <summary>
        /// Checks the current biomes' terrain and makes changes accordingly on tile gen.
        /// </summary>
        [HarmonyPostfix]
        public static void ReplaceTerrain(Map map, ref TerrainDef __result)
        {
            // Gravel checks.
            // Per Biome.
            /*
            if (map.Biome == ABDefOf.SZ_CrystallineFlats)
            {
                if (__result == TerrainDefOf.Gravel && AlienBiomesSettings.UseAlienGravel == true) {
                    __result = ABDefOf.SZ_CrystallineStonySoil;
                }
            }
            */
            if (map.Biome == ABDefOf.SZ_RadiantPlains)
            {
                if (__result == TerrainDefOf.Gravel && AlienBiomesSettings.UseAlienGravel == true) {
                    __result = ABDefOf.SZ_RadiantStonySoil;
                }
            }

            // Sand checks.
            // Per Biome.
            // Additional mod check required since GL patches the same method with vanilla sand.
            if (ModsConfig.IsActive("m00nl1ght.geologicallandforms"))
            {
                /*
                if (map.Biome == ABDefOf.SZ_CrystallineFlats)
                {
                    if (__result == TerrainDefOf.Sand && AlienBiomesSettings.UseAlienSand == true) {
                        __result = ABDefOf.SZ_CrystallineSand;
                    }
                }
                */
                if (map.Biome == ABDefOf.SZ_RadiantPlains)
                {
                    if (__result == TerrainDefOf.Sand && AlienBiomesSettings.UseAlienSand == true) {
                        __result = ABDefOf.SZ_SoothingSand;
                    }
                }
            }

            // Water checks.
            // Per Biome.
            /*
            if (map.Biome == ABDefOf.SZ_CrystallineFlats)
            {
                if (__result == TerrainDefOf.WaterShallow && AlienBiomesSettings.UseAlienWater == true) {
                    __result = ABDefOf.SZ_CrystallineWaterShallow;
                }
                else if (__result == TerrainDefOf.WaterOceanShallow && AlienBiomesSettings.UseAlienWater == true) {
                    __result = ABDefOf.SZ_CrystallineWaterOceanShallow;
                }
                else if (__result == TerrainDefOf.WaterOceanDeep && AlienBiomesSettings.UseAlienWater == true) {
                    __result = ABDefOf.SZ_CrystallineWaterOceanDeep;
                }
                else if (__result == TerrainDefOf.WaterMovingShallow && AlienBiomesSettings.UseAlienWater == true) {
                    __result = ABDefOf.SZ_CrystallineWaterMovingShallow;
                }
                else if (__result == TerrainDefOf.WaterMovingChestDeep && AlienBiomesSettings.UseAlienWater == true) {
                    __result = ABDefOf.SZ_CrystallineWaterMovingChestDeep;
                }
            }
            */
            if (map.Biome == ABDefOf.SZ_RadiantPlains)
            {
                if (__result == TerrainDefOf.WaterShallow && AlienBiomesSettings.UseAlienWater == true) {
                    __result = ABDefOf.SZ_RadiantWaterShallow;
                }
                else if (__result == TerrainDefOf.WaterOceanShallow && AlienBiomesSettings.UseAlienWater == true) {
                    __result = ABDefOf.SZ_RadiantWaterOceanShallow;
                }
                else if (__result == TerrainDefOf.WaterOceanDeep && AlienBiomesSettings.UseAlienWater == true) {
                    __result = ABDefOf.SZ_RadiantWaterOceanDeep;
                }
                else if (__result == TerrainDefOf.WaterMovingShallow && AlienBiomesSettings.UseAlienWater == true) {
                    __result = ABDefOf.SZ_RadiantWaterMovingShallow;
                }
                else if (__result == TerrainDefOf.WaterMovingChestDeep && AlienBiomesSettings.UseAlienWater == true) {
                    __result = ABDefOf.SZ_RadiantWaterMovingChestDeep;
                }
            }
        }
    }
}
