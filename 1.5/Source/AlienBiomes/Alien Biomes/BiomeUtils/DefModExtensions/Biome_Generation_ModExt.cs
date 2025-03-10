using RimWorld.Planet;
using RimWorld;
using System.Collections.Generic;
using JetBrains.Annotations;
using Verse;

namespace AlienBiomes
{
    [UsedImplicitly]
    public class Biome_Generation_ModExt : DefModExtension
    {
        #region Miscellaneous
        public int frequency = 100;
        public List<BiomeDef> spawnOnBiomes = [];
        public int? biomePriority;
        public bool allowOnWater;
        public bool allowOnLand = true;
        public bool needRiver;
        public bool setNotWaterCovered;
        public int minimumWaterNeighbors;
        public int minimumLandNeighbors;
        public bool forested;
        public bool uniqueHills;
        public float forestSnowyBelow = -9999f;
        public float forestSparseBelow = -9999f;
        public float forestDenseAbove = 9999f;
        #endregion
        
        #region WeatherLimits
        public float minRainfall = -9999f;
        public float maxRainfall = 9999f;
        public float minTemperature = -999f;
        public float maxTemperature = 999f;
        #endregion
        
        #region GeospatialLimits
        public float minNorthLatitude = -9999f;
        public float maxNorthLatitude = -9999f;
        public float minSouthLatitude = -9999f;
        public float maxSouthLatitude = -9999f;
        #endregion
        
        #region TopologyLimits
        public Hilliness minHilliness = Hilliness.Flat;
        public Hilliness maxHilliness = Hilliness.Impassable;
        public BiomeHilliness? spawnHills;
        public BiomeHilliness? setHills;
        public BiomeHilliness? minRandomHills = BiomeHilliness.Flat;
        public BiomeHilliness? maxRandomHills = BiomeHilliness.Mountainous;
        public bool randomizeHilliness;
        public float minElevation = -9999f;
        public float maxElevation = 9999f;
        public float? setElevation;
        public float smallHillSizeMultiplier = 1.5f;
        public float largeHillSizeMultiplier = 2f;
        public float mountainSizeMultiplier = 1.4f;
        public float impassableSizeMultiplier = 1.3f;
        #endregion
        
        #region WorldScatteringPerlinNoise
        public bool useAlternativePerlinSeedPreset;
        public bool usePerlin;
        public int? perlinCustomSeed;
        public float perlinCulling = 0.99f;
        public float perlinFrequency = 0.1f;
        public float perlinLacunarity = 10.0f;
        public float perlinPersistence = 0.6f;
        public int perlinOctaves = 12;
        #endregion
        
        #region TerrainOverrides
        public TerrainDef newSand;
        public TerrainDef newBeachSand;
        public TerrainDef newGravel;
        public TerrainDef newShallowWater;
        public TerrainDef newWaterMovingShallow;
        public TerrainDef newWaterOceanShallow;
        public TerrainDef newWaterDeep;
        public TerrainDef newWaterOceanDeep;
        public TerrainDef newWaterMovingChestDeep;
        #endregion
    }
}