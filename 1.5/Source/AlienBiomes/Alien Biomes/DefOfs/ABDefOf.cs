using RimWorld;
using Verse;

namespace AlienBiomes
{
    [DefOf]
    public class ABDefOf
    {
        #region BiomeDefs
        public static BiomeDef SZ_RadiantPlains;
        public static BiomeDef SZ_DeliriousDunes;
        public static BiomeDef SZ_CrystallineFlats;
        #endregion
        
        #region HediffDefs
        public static HediffDef SZ_Crystallize;
        #endregion
        
        #region LetterDefs
        public static LetterDef SZ_DesertBloomLetter;
        public static LetterDef SZ_PawnCrystallizingLetter;
        public static LetterDef SZ_PawnCrystallizedLetter;
        #endregion
        
        #region TerrainDefs
        public static TerrainDef SZ_CrystallineSoil;
        public static TerrainDef SZ_BloodWaterMovingShallow;
        public static TerrainDef SZ_BloodWaterMovingChestDeep;
        #endregion
        
        #region ShaderTypeDefs
        public static ShaderTypeDef TransparentPlantShimmer;
        public static ShaderTypeDef TransparentPlantPulse;
        #endregion
        
        static ABDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(ABDefOf));
        }
    }
}