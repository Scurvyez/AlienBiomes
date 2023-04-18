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
        public static ThingDef SZ_BulbusSanitas;
        public static ThingDef SZ_Fiddlehead;
        public static ThingDef SZ_CupheadMold;
        public static ThingDef SZ_SoothingStalk;
        public static ThingDef SZ_CopaceticCone;
        public static ThingDef SZ_LilyOfThePlains;
        public static ThingDef SZ_ElderBlushingTree;
        public static ThingDef SZ_FiddleheadWall;

        // Crystals
        public static ThingDef SZ_BlueColossalCrystalOne;
        public static ThingDef SZ_BlueColossalCrystalTwo;
        public static ThingDef SZ_GreenColossalCrystalOne;
        public static ThingDef SZ_GreenColossalCrystalTwo;
        public static ThingDef SZ_RedColossalCrystal;
        public static ThingDef SZ_InferiorCrystal;

        // Medicine
        public static ThingDef SZ_WindingFernAloe;

        // Structures
        public static KCSG.StructureLayoutDef SZAB_OminousGrove;

        static AlienBiomes_ThingDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(AlienBiomes_ThingDefOf));
        }
    }
}
