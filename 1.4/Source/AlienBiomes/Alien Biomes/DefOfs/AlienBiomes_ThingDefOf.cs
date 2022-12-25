using Verse;
using RimWorld;

namespace AlienBiomes
{
    [DefOf]
    public static class AlienBiomes_ThingDefOf
    {
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
