using JetBrains.Annotations;
using RimWorld;
using RimWorld.Planet;

namespace AlienBiomes
{
    [UsedImplicitly]
    public class BiomeWorker_CrystallineFlats : BiomeWorker_AlienBase
    {
        protected override int NoiseSeedPart => 44319114;
        
        protected override float ComputeBaseScore(BiomeDef biome, Tile tile, 
            PlanetTile planetTile, ModExt_BiomeGeneration ext, WorldGrid worldGrid)
        {
            return 15f + (0f - tile.temperature);
        }
    }
}