using Verse;
using RimWorld;

namespace AlienBiomes
{
    public class CompProperties_TimedGlower : CompProperties_Glower
    {
        /// <summary>
        /// If not null, the time after which it will start glowing.
        /// </summary>
        public float startTime = 0.75f;
        /// <summary>
        /// If not null, the time before which it will stop glowing.
        /// </summary>
        public float stopTime = 0.20f;

        public CompProperties_TimedGlower()
        {
            compClass = typeof(Comp_TimedGlower);
        }
    }
}
