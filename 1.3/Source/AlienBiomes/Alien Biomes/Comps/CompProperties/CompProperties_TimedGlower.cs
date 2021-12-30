using Verse;
using RimWorld;

namespace AlienBiomes
{
    public class CompProperties_TimedGlower : CompProperties_Glower
    {
        /// <summary>
        /// If not null, the time after which it will glow.
        /// </summary>
        public float startTime = 0.75f;
        /// <summary>
        /// If not null, the time before which it will glow.
        /// </summary>
        public float stopTime = 0.20f;

        public CompProperties_TimedGlower()
        {
            this.compClass = typeof(Comp_TimedGlower);
        }
    }
}
