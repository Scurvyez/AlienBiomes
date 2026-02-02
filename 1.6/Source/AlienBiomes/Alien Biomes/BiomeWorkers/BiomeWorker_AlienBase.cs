using RimWorld;
using RimWorld.Planet;
using Verse;
using Verse.Noise;

namespace AlienBiomes
{
    public abstract class BiomeWorker_AlienBase : BiomeWorker
    {
        private const float GetScoreLowestLimit = -100f;
        
        protected abstract int NoiseSeedPart { get; }
        
        public override float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile)
        {
            WorldGrid worldGrid = Find.WorldGrid;
            ModExt_BiomeGeneration ext = biome.GetModExtension<ModExt_BiomeGeneration>();

            if (ext == null || worldGrid == null)
                return GetScoreLowestLimit;

            if (tile.WaterCovered || tile.hilliness == Hilliness.Impassable)
                return GetScoreLowestLimit;

            if (!AllowedByEnvironment(tile, ext))
                return GetScoreLowestLimit;

            if (ext.usePerlinNoise && !AllowedByNoise(worldGrid, planetTile, ext))
                return GetScoreLowestLimit;

            if (!AdditionalAllowed(biome, tile, planetTile, ext, worldGrid))
                return GetScoreLowestLimit;

            float score = ComputeBaseScore(tile, ext);

            score += BiomeGenHelper.HillinessScoreBias(tile, ext, maxAbsBias: ext.hillsMaxScoreBias);

            return score;
        }
        
        // if we feel like adding more conditions, this is the place to do it :)
        protected virtual bool AdditionalAllowed(BiomeDef biome, Tile tile, 
            PlanetTile planetTile, ModExt_BiomeGeneration ext, WorldGrid worldGrid) => true;

        protected abstract float ComputeBaseScore(Tile tile, ModExt_BiomeGeneration ext);

        private static bool AllowedByEnvironment(Tile tile, ModExt_BiomeGeneration ext)
        {
            if (!BiomeGenHelper.IsTemperatureInRange(tile.temperature, ext.minTemperature, ext.maxTemperature))
                return false;

            if (!BiomeGenHelper.IsRainfallInRange(tile.rainfall, ext.minRainfall, ext.maxRainfall))
                return false;

            return true;
        }

        private bool AllowedByNoise(WorldGrid worldGrid, PlanetTile planetTile, ModExt_BiomeGeneration ext)
        {
            return BiomeNoiseHelper.PerlinPassesThreshold(
                worldGrid, planetTile,
                ext.noiseThreshold,
                ext.noiseFrequency, ext.noiseLacunarity, ext.noisePersistence, ext.noiseOctaves,
                normalized: true,
                invert: false,
                seedPart: NoiseSeedPart,
                quality: QualityMode.Medium);
        }
    }
}