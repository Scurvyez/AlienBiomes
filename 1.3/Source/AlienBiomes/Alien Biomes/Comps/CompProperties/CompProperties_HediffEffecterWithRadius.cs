using Verse;
using UnityEngine;

namespace AlienBiomes
{
    public class CompProperties_HediffEffecterWithRadius : CompProperties
    {
        public FleckDef fleckReleased;
        public int releaseRadius;
        public HediffDef appliedHediff;
        public SoundDef soundOnRelease;
        public int tickInterval = 2500; // 2500 = a check every in-game hour.
        public Color radiusOutlineColor; // (r, g, b, a)
        public bool onlyAffectHumanlike = true;

        public CompProperties_HediffEffecterWithRadius() => compClass = typeof(Comp_HediffEffecterWithRadius);

    }
}
