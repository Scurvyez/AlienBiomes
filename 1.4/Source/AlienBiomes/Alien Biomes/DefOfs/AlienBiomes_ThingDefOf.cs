using Verse;
using RimWorld;

namespace AlienBiomes
{
    /// <summary>
    /// This is a DefOf class with the DefOf attribute.
    /// All this does is create a direct reference to your specific xml def.
    /// Allows you to reference the def directly in c# without using strings.
    /// </summary>
    [DefOf]
    public static class AlienBiomes_ThingDefOf
    {
        // Plants
        public static ThingDef SZ_RadiantGrass;
        public static ThingDef SZ_RadiantTallGrass;
        public static ThingDef SZ_LushGrass;
        public static ThingDef SZ_BulbousSanitas;
        public static ThingDef SZ_Fiddlehead;
        public static ThingDef SZ_PyroclasticChaliceFungus;
        public static ThingDef SZ_SoothingStalk;
        public static ThingDef SZ_CopaceticCone;
        public static ThingDef SZ_LilyOfThePlains;
        public static ThingDef SZ_CeruleanHoneytree;
        public static ThingDef SZ_FiddleheadWall;
        //public static ThingDef SZ_ShallowOceanWaterBioluminescence;

        // Crystals
        /*
        public static ThingDef SZ_BlueColossalCrystalOne;
        public static ThingDef SZ_BlueColossalCrystalTwo;
        public static ThingDef SZ_GreenColossalCrystalOne;
        public static ThingDef SZ_GreenColossalCrystalTwo;
        public static ThingDef SZ_RedColossalCrystal;
        public static ThingDef SZ_InferiorCrystal;
        */

        // Food
        public static ThingDef SZ_RawSoothingHoney;
        public static ThingDef SZ_RawCopaceticHoney;
        public static ThingDef SZ_CeruleanHoney;

        // Medicine
        //public static ThingDef SZ_WindingFernAloe;

        // Structures
        //public static KCSG.StructureLayoutDef SZAB_OminousGrove;

        // Shaders
        public static ShaderTypeDef MoteGlowDistorted;
        public static ShaderTypeDef TransparentPlantShimmer;
        public static ShaderTypeDef TransparentPlantPulse;

        static AlienBiomes_ThingDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(AlienBiomes_ThingDefOf));
        }
    }
}
