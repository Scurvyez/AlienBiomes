using UnityEngine;
using Verse;

namespace AlienBiomes
{
    [StaticConstructorOnStartup]
    public static class ABTextureAssets
    {
        public static readonly Texture2D BioDistTex = ContentFinder<Texture2D>.Get("Things/Mote/SmokeTiled", true);
        public static readonly Texture2D BioTexA = ContentFinder<Texture2D>.Get("AlienBiomes/Things/Special/ShallowOceanWaterBioluminescence/ShallowOceanWaterBioluminescenceA", true);
    }
}