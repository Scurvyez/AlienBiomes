using Verse;
using RimWorld;

namespace AlienBiomes
{
    [DefOf]
    public static class AlienBiomes_TerrainDefOf
    {
        public static TerrainDef SZ_EnlightenedSoil;
        public static TerrainDef SZ_EnlightenedRichSoil;
        public static TerrainDef SZ_EnlightenedMud;
        public static TerrainDef SZ_EnlightenedStonySoil;
        public static TerrainDef SZ_SoothingSand;

        static AlienBiomes_TerrainDefOf() => DefOfHelper.EnsureInitializedInCtor(typeof(TerrainDefOf));
    }
}
