using Verse;
using RimWorld;

namespace AlienBiomes
{
    [DefOf]
    public class AB_FleckDefOf
    {
        public static FleckDef SZ_ChaliceFungusEffect;
        
        static AB_FleckDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(AB_FleckDefOf));
        }
    }
}
