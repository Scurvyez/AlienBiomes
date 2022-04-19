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
            if (map.Biome.defName == "SZ_CrystallineFlats")
            {
                if (__result == TerrainDefOf.Gravel && AlienBiomesSettings.UseVanillaGravel == false) {
                    __result = AlienBiomes_TerrainDefOf.SZ_CrystallineStonySoil;
                }
            }
            if (map.Biome.defName == "SZ_RadiantPlains")
            {
                if (__result == TerrainDefOf.Gravel && AlienBiomesSettings.UseVanillaGravel == false) {
                    __result = AlienBiomes_TerrainDefOf.SZ_RadiantStonySoil;
                }
            }

            // Sand checks.
            // Per Biome.
            // Additional mod check required since GL patches the same method with vanilla sand.
            if (ModsConfig.IsActive("m00nl1ght.geologicallandforms"))
            {
                if (map.Biome.defName == "SZ_CrystallineFlats")
                {
                    if (__result == TerrainDefOf.Sand && AlienBiomesSettings.UseVanillaSand == false) {
                        __result = AlienBiomes_TerrainDefOf.SZ_CrystallineSand;
                    }
                }
                if (map.Biome.defName == "SZ_RadiantPlains")
                {
                    if (__result == TerrainDefOf.Sand && AlienBiomesSettings.UseVanillaSand == false) {
                        __result = AlienBiomes_TerrainDefOf.SZ_SoothingSand;
                    }
                }
            }

            // Water checks.
            // Per Biome.
            if (map.Biome.defName == "SZ_CrystallineFlats")
            {
                if (__result == TerrainDefOf.WaterShallow && AlienBiomesSettings.UseVanillaWater == false) {
                    __result = AlienBiomes_TerrainDefOf.SZ_CrystallineWaterShallow;
                }
                if (__result == TerrainDefOf.WaterOceanShallow && AlienBiomesSettings.UseVanillaWater == false) {
                    __result = AlienBiomes_TerrainDefOf.SZ_CrystallineWaterOceanShallow;
                }
                if (__result == TerrainDefOf.WaterOceanDeep && AlienBiomesSettings.UseVanillaWater == false) {
                    __result = AlienBiomes_TerrainDefOf.SZ_CrystallineWaterOceanDeep;
                }
            }
            if (map.Biome.defName == "SZ_RadiantPlains")
            {
                if (__result == TerrainDefOf.WaterShallow && AlienBiomesSettings.UseVanillaWater == false) {
                    __result = AlienBiomes_TerrainDefOf.SZ_RadiantWaterShallow;
                }
                if (__result == TerrainDefOf.WaterOceanShallow && AlienBiomesSettings.UseVanillaWater == false) {
                    __result = AlienBiomes_TerrainDefOf.SZ_RadiantWaterOceanShallow;
                }
                if (__result == TerrainDefOf.WaterOceanDeep && AlienBiomesSettings.UseVanillaWater == false) {
                    __result = AlienBiomes_TerrainDefOf.SZ_RadiantWaterOceanDeep;
                }
            }
        }
    }
}
