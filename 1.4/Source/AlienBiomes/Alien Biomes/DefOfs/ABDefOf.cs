using RimWorld;
using Verse;

namespace AlienBiomes
{
    [DefOf]
    public class ABDefOf
    {
        public static BiomeDef SZ_RadiantPlains;
        //public static BiomeDef SZ_CrystallineFlats;

        public static FleckDef SZ_ChaliceFungusEffect;

        //public static HediffDef SZ_Crystallize;

        //public static LetterDef SZ_PawnCrystallizing;
        //public static LetterDef SZ_PawnCrystallized;

        public static TerrainDef SZ_RadiantSoil;
        public static TerrainDef SZ_RadiantRichSoil;
        public static TerrainDef SZ_RadiantMud;
        public static TerrainDef SZ_RadiantStonySoil;
        public static TerrainDef SZ_SoothingSand;

        /*
        public static TerrainDef SZ_CrystallineSoil;
        public static TerrainDef SZ_CrystallineRichSoil;
        public static TerrainDef SZ_CrystallineStonySoil;
        public static TerrainDef SZ_CrystallineSand;
        public static TerrainDef SZ_CrystallineSand_Grove;
        */

        /*
        public static TerrainDef SZ_BloodWaterMovingShallow;
        public static TerrainDef SZ_BloodWaterShallow;
        public static TerrainDef SZ_BloodWaterMovingChestDeep;
        public static TerrainDef SZ_BloodWaterChestDeep;
        */

        /*
        public static TerrainDef SZ_CrystallineWaterMovingShallow;
        public static TerrainDef SZ_CrystallineWaterShallow;
        public static TerrainDef SZ_CrystallineWaterMovingChestDeep;
        public static TerrainDef SZ_CrystallineWaterChestDeep;
        public static TerrainDef SZ_CrystallineWaterOceanShallow;
        public static TerrainDef SZ_CrystallineWaterOceanDeep;
        */

        public static TerrainDef SZ_RadiantWaterMovingShallow;
        public static TerrainDef SZ_RadiantWaterShallow;
        public static TerrainDef SZ_RadiantWaterMovingChestDeep;
        public static TerrainDef SZ_RadiantWaterChestDeep;
        public static TerrainDef SZ_RadiantWaterOceanShallow;
        public static TerrainDef SZ_RadiantWaterOceanDeep;

        public static ThingDef SZ_RadiantGrass;
        public static ThingDef SZ_RadiantTallGrass;
        //public static ThingDef SZ_LushGrass;
        public static ThingDef SZ_BulbousSanitas;
        public static ThingDef SZ_Fiddlehead;
        public static ThingDef SZ_ChaliceFungus;
        public static ThingDef SZ_SoothingStalk;
        public static ThingDef SZ_CopaceticCone;
        public static ThingDef SZ_LilyOfThePlains;
        public static ThingDef SZ_CeruleanHoneyBush;
        public static ThingDef SZ_FiddleheadWall;
        //public static ThingDef SZ_ShallowOceanWaterBioluminescence;

        /*
        public static ThingDef SZ_BlueColossalCrystalOne;
        public static ThingDef SZ_BlueColossalCrystalTwo;
        public static ThingDef SZ_GreenColossalCrystalOne;
        public static ThingDef SZ_GreenColossalCrystalTwo;
        public static ThingDef SZ_RedColossalCrystal;
        public static ThingDef SZ_InferiorCrystal;
        */

        public static ThingDef SZ_RawSoothingHoney;
        public static ThingDef SZ_RawCopaceticHoney;
        public static ThingDef SZ_CeruleanHoney;

        //public static ThingDef SZ_WindingFernAloe;

        //public static KCSG.StructureLayoutDef SZAB_OminousGrove;

        public static ShaderTypeDef MoteGlowDistorted;
        public static ShaderTypeDef TransparentPlantShimmer;
        public static ShaderTypeDef TransparentPlantPulse;

        static ABDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(ABDefOf));
        }
    }
}
