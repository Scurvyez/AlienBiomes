using JetBrains.Annotations;
using RimWorld;

namespace AlienBiomes
{
    [UsedImplicitly]
    public class CompProperties_TimedGlower : CompProperties_Glower
    {
        public float glowStartTime = 0.75f;
        public float glowStopTime = 0.20f;
        
        public CompProperties_TimedGlower() => compClass = typeof(Comp_TimedGlower);
    }
}