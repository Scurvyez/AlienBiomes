using Verse;
using RimWorld;

namespace AlienBiomes
{
    [DefOf]
    public class AlienBiomes_LetterDefOf
    {
        public static LetterDef SZ_PawnCrystallizing;
        public static LetterDef SZ_PawnCrystallized;

        static AlienBiomes_LetterDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(AlienBiomes_LetterDefOf));
        }
    }
}
