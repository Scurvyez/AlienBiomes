using RimWorld;

namespace AlienBiomes
{
    [DefOf]
    public class AB_BiomeDefOf
    {
        public static BiomeDef SZ_RadiantPlains;
        //public static BiomeDef SZ_CrystallineFlats;

        static AB_BiomeDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(AB_BiomeDefOf));
        }
    }
}
