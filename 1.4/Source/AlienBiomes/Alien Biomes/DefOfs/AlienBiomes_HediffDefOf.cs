using Verse;
using RimWorld;

namespace AlienBiomes
{
    [DefOf]
    public class AlienBiomes_HediffDefOf
    {
        public static HediffDef SZ_Crystallize;

        static AlienBiomes_HediffDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(AlienBiomes_HediffDefOf));
        }
    }
}
