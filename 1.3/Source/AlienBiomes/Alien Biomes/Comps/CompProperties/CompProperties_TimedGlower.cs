using Verse;
using RimWorld;

namespace AlienBiomes
{
    public class CompProperties_TimedGlower : CompProperties_Glower
    {
        /// <summary>
        /// If not null, the time after which it will glow.
        /// </summary>
        public float? startTime = null;
        /// <summary>
        /// If not null, the time before which it will glow.
        /// </summary>
        public float? stopTime = null;

        public CompProperties_TimedGlower()
        {
            this.compClass = typeof(CompTimedGlower);
        }
    }
}
