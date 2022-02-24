using Verse;
using RimWorld;

namespace AlienBiomes
{
    [DefOf]
    public static class AlienBiomes_NaturalBuildingDefOf
    {
        public static ThingDef SZ_SteamGeyserRadiantSoil;
        public static ThingDef SZ_SteamGeyserRadiantRichSoil;

        static AlienBiomes_NaturalBuildingDefOf() => DefOfHelper.EnsureInitializedInCtor(typeof(ThingDefOf));
    }
}
