using Verse;
using RimWorld;

namespace AlienBiomes
{
    [DefOf]
    public static class AlienBiomes_TerrainDefOf
    {
        // Solid.
        public static TerrainDef SZ_RadiantSoil;
        public static TerrainDef SZ_RadiantRichSoil;
        public static TerrainDef SZ_RadiantMud;
        public static TerrainDef SZ_RadiantStonySoil;
        public static TerrainDef SZ_SoothingSand;

        public static TerrainDef SZ_CrystallineSoil;
        public static TerrainDef SZ_CrystallineRichSoil;
        public static TerrainDef SZ_CrystallineStonySoil;
        public static TerrainDef SZ_CrystallineSand;
        public static TerrainDef SZ_CrystallineSand_Grove;

        // Liquid.
        public static TerrainDef SZ_BloodWaterMovingShallow;
        public static TerrainDef SZ_BloodWaterShallow;
        public static TerrainDef SZ_BloodWaterMovingChestDeep;
        public static TerrainDef SZ_BloodWaterChestDeep;

        public static TerrainDef SZ_CrystallineWaterMovingShallow;
        public static TerrainDef SZ_CrystallineWaterShallow;
        public static TerrainDef SZ_CrystallineWaterMovingChestDeep;
        public static TerrainDef SZ_CrystallineWaterChestDeep;
        public static TerrainDef SZ_CrystallineWaterOceanShallow;
        public static TerrainDef SZ_CrystallineWaterOceanDeep;

        public static TerrainDef SZ_RadiantWaterMovingShallow;
        public static TerrainDef SZ_RadiantWaterShallow;
        public static TerrainDef SZ_RadiantWaterMovingChestDeep;
        public static TerrainDef SZ_RadiantWaterChestDeep;
        public static TerrainDef SZ_RadiantWaterOceanShallow;
        public static TerrainDef SZ_RadiantWaterOceanDeep;

        static AlienBiomes_TerrainDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(AlienBiomes_TerrainDefOf));
        }
    }
}
