using JetBrains.Annotations;
using Verse;

namespace AlienBiomes
{
    [UsedImplicitly]
    public class HediffCompProperties_Crystallize : HediffCompProperties
    {
        public string targetCrystal = "";

        public HediffCompProperties_Crystallize()
        {
            compClass = typeof(HediffComp_Crystallize);
        }
    }
}