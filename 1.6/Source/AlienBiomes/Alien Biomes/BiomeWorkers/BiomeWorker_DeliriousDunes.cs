using JetBrains.Annotations;
using RimWorld.Planet;

namespace AlienBiomes
{
    [UsedImplicitly]
    public class BiomeWorker_DeliriousDunes : BiomeWorker_AlienBase
    {
        protected override int NoiseSeedPart => 43123466;

        protected override float ComputeBaseScore(Tile tile, ModExt_BiomeGeneration ext)
        {
            return tile.temperature * 2.7f - 13f - tile.rainfall * 0.14f;
        }
    }
}