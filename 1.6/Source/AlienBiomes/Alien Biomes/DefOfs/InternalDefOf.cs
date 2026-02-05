using RimWorld;
using Verse;

namespace AlienBiomes
{
    [DefOf]
    public class InternalDefOf
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
        public static TerrainDef SZ_DeliriousRichBlackSand;
        public static TerrainDef SZ_DeliriousBlackSand;
        #endregion
        
        #region PlantDefs
        public static ThingDef SZ_ChaliceFungus;
        #endregion
        
        #region ShaderTypeDefs
        public static ShaderTypeDef AB_MoteGlowDistortedVertex;
        #endregion
        
        #region DamageDefs
        public static DamageDef SZ_PlantAcidSplash;
        #endregion
        
        static InternalDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(InternalDefOf));
        }
    }
}