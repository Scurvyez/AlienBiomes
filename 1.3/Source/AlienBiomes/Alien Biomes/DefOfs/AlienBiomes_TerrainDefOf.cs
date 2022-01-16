using Verse;
using RimWorld;

namespace AlienBiomes
{
    [DefOf]
    public static class AlienBiomes_TerrainDefOf
    {
        public static TerrainDef SZ_RadiantSoil;
        public static TerrainDef SZ_RadiantRichSoil;
        public static TerrainDef SZ_RadiantMud;
        public static TerrainDef SZ_RadiantStonySoil;
        public static TerrainDef SZ_SoothingSand;

        static AlienBiomes_TerrainDefOf() => DefOfHelper.EnsureInitializedInCtor(typeof(TerrainDefOf));
    }
}
