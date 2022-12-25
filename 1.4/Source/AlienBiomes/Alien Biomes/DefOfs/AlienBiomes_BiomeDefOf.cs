using Verse;
using RimWorld;

namespace AlienBiomes
{
    [DefOf]
    public class AlienBiomes_BiomeDefOf
    {
        public static BiomeDef SZ_RadiantPlains;
        public static BiomeDef SZ_CrystallineFlats;

        static AlienBiomes_BiomeDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(AlienBiomes_BiomeDefOf));
        }
    }
}
