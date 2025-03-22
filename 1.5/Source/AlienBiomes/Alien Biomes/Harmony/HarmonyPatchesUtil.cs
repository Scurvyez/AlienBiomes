using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace AlienBiomes
{
    public static class HarmonyPatchesUtil
    {
        public const string WATER = "Water";
        
        public static readonly Dictionary<TerrainDef, 
            Func<Biome_Generation_ModExt, TerrainDef>> TerrainReplacements = new()
            {
                { TerrainDefOf.Gravel, ext => ext.newGravel },
                { TerrainDefOf.Sand, ext => ext.newSand },
                { TerrainDefOf.Soil, ext => ext.newSoil },
                { TerrainDefOf.SoilRich, ext => ext.newSoilRich },
                { TerrainDefOf.WaterShallow, ext => ext.newShallowWater },
                { TerrainDefOf.WaterMovingShallow, ext => ext.newWaterMovingShallow },
                { TerrainDefOf.WaterOceanShallow, ext => ext.newWaterOceanShallow },
                { TerrainDefOf.WaterDeep, ext => ext.newWaterDeep },
                { TerrainDefOf.WaterOceanDeep, ext => ext.newWaterOceanDeep },
                { TerrainDefOf.WaterMovingChestDeep, ext => ext.newWaterMovingChestDeep }
            };
    }
}