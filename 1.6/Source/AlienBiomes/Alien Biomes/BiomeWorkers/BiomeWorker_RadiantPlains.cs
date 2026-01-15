using JetBrains.Annotations;
using RimWorld;
using RimWorld.Planet;

namespace AlienBiomes
{
    [UsedImplicitly]
    public class BiomeWorker_RadiantPlains : BiomeWorker_AlienBase
    {
        protected override int NoiseSeedPart => 17393466;

        protected override float ComputeBaseScore(BiomeDef biome, Tile tile, 
            PlanetTile planetTile, ModExt_BiomeGeneration ext, WorldGrid worldGrid)
        {
            return 15f + (tile.temperature - 7f) + (tile.rainfall - ext.minRainfall) / 180f;
        }
    }
}