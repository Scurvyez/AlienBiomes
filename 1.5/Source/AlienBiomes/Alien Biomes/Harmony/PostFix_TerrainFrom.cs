using HarmonyLib;
using RimWorld;
using Verse;

namespace AlienBiomes
{
    /*
    [HarmonyPatch(typeof(GenStep_Terrain), "TerrainFrom")]
    public static class TerrainFrom_Patch
    {
        /// <summary>
        /// Checks the current biomes' terrain and makes changes accordingly on tile gen.
        /// </summary>
        [HarmonyPostfix]
        public static void ReplaceTerrain(Map map, ref TerrainDef __result)
        {
            if (map.Biome.HasModExtension<BiomeControls>())
            {
                BiomeControls ext = map.Biome.GetModExtension<BiomeControls>();

                // gravel
                if (ext.newGravel != null && __result == TerrainDefOf.Gravel)
                {
                    __result = ext.newGravel;
                }

                // sand
                if (ext.newSand != null && __result == TerrainDefOf.Sand)
                {
                    __result = ext.newSand;
                }

                // water
                if (ext.newShallowWater != null && __result == TerrainDefOf.WaterShallow)
                {
                    __result = ext.newShallowWater;
                }
                else if (ext.newWaterMovingShallow != null && __result == TerrainDefOf.WaterMovingShallow)
                {
                    __result = ext.newWaterMovingShallow;
                }
                else if (ext.newWaterOceanShallow != null && __result == TerrainDefOf.WaterOceanShallow)
                {
                    __result = ext.newWaterOceanShallow;
                }
                else if (ext.newWaterDeep != null && __result == TerrainDefOf.WaterDeep)
                {
                    __result = ext.newWaterDeep;
                }
                else if (ext.newWaterOceanDeep != null && __result == TerrainDefOf.WaterOceanDeep)
                {
                    __result = ext.newWaterOceanDeep;
                }
                else if (ext.newWaterMovingChestDeep != null && __result == TerrainDefOf.WaterMovingChestDeep)
                {
                    __result = ext.newWaterMovingChestDeep;
                }
            }
        }
    }
    */
}
