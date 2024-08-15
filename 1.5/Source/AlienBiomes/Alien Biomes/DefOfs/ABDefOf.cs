using RimWorld;
using Verse;

namespace AlienBiomes
{
    [DefOf]
    public class ABDefOf
    {
        public static BiomeDef SZ_RadiantPlains;
        public static BiomeDef SZ_DeliriousDunes;
        public static BiomeDef SZ_CrystallineFlats;

        public static HediffDef SZ_Crystallize;

        public static LetterDef SZ_DesertBloomLetter;
        public static LetterDef SZ_PawnCrystallizingLetter;
        public static LetterDef SZ_PawnCrystallizedLetter;

        public static TerrainDef SZ_CrystallineSoil;
        public static TerrainDef SZ_BloodWaterMovingShallow;
        public static TerrainDef SZ_BloodWaterMovingChestDeep;
        
        public static ShaderTypeDef TransparentPlantShimmer;
        public static ShaderTypeDef TransparentPlantPulse;

        static ABDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(ABDefOf));
        }
    }
}
