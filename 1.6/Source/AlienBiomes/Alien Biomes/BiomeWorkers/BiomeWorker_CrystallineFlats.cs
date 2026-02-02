using JetBrains.Annotations;
using RimWorld.Planet;

namespace AlienBiomes
{
    [UsedImplicitly]
    public class BiomeWorker_CrystallineFlats : BiomeWorker_AlienBase
    {
        protected override int NoiseSeedPart => 44319114;
        
        protected override float ComputeBaseScore(Tile tile, ModExt_BiomeGeneration ext)
        {
            return 15f + (0f - tile.temperature);
        }
    }
}