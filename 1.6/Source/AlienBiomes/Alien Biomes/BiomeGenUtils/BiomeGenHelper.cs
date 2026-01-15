using RimWorld.Planet;
using UnityEngine;

namespace AlienBiomes
{
    public static class BiomeGenHelper
    {
        private const float UnrestrictedMin = -9999f;
        private const float UnrestrictedMax =  9999f;
        
        public static bool IsRainfallInRange(float rainfall, float minRainfall, float maxRainfall)
        {
            bool hasRainfallRestrictions =
                !Mathf.Approximately(minRainfall, UnrestrictedMin) ||
                !Mathf.Approximately(maxRainfall, UnrestrictedMax);
            
            if (!hasRainfallRestrictions)
                return true;
            
            return rainfall >= minRainfall && rainfall <= maxRainfall;
        }
        
        public static bool IsTemperatureInRange(float temperature, float minTemperature, float maxTemperature)
        {
            bool hasRainfallRestrictions =
                !Mathf.Approximately(minTemperature, UnrestrictedMin) ||
                !Mathf.Approximately(maxTemperature, UnrestrictedMax);
            
            if (!hasRainfallRestrictions)
                return true;
            
            return temperature >= minTemperature && temperature <= maxTemperature;
        }

        public static float HillinessChanceFactor(Tile tile, ModExt_BiomeGeneration ext)
        {
            float factor = tile.hilliness switch
            {
                Hilliness.Flat => ext.hillsFlatChance,
                Hilliness.SmallHills => ext.hillsSmallChance,
                Hilliness.LargeHills => ext.hillsLargeChance,
                Hilliness.Mountainous => ext.hillsMountainChance,
                Hilliness.Impassable => ext.hillsImpassableChance,
                _ => 1f
            };
            
            return Mathf.Clamp01(factor);
        }

        public static float HillinessScoreBias(Tile tile, ModExt_BiomeGeneration ext, float maxAbsBias = 8f)
        {
            float pref = HillinessChanceFactor(tile, ext);
            float centered = (pref - 0.5f) * 2f;

            return centered * maxAbsBias;
        }
    }
}