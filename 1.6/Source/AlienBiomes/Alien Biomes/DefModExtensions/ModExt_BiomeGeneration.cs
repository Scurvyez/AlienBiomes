using JetBrains.Annotations;
using Verse;

namespace AlienBiomes
{
    [UsedImplicitly]
    public class ModExt_BiomeGeneration : DefModExtension
    {
        #region Weather Restraints
        public float minRainfall = -9999f;
        public float maxRainfall = 9999f;
        public float minTemperature = -999f;
        public float maxTemperature = 999f;
        #endregion
        
        #region Topology Restraints
        public float hillsFlatChance = 0.5f;
        public float hillsSmallChance = 0.235f;
        public float hillsLargeChance = 0.1f;
        public float hillsMountainChance = 0.05f;
        public float hillsImpassableChance = 0.01f;
        public float hillsMaxScoreBias = 8f;
        #endregion
        
        #region Perlin Noise Restraints
        public bool usePerlinNoise = false;
        public float noiseThreshold = 0.9f;
        public float noiseFrequency = 0.3f;
        public float noiseLacunarity = 0.4f;
        public float noisePersistence = 0.5f;
        public int noiseOctaves = 5;
        #endregion
    }
}