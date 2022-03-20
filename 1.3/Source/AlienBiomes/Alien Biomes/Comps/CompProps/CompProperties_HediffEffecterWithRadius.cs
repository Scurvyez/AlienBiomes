using Verse;
using RimWorld;
using UnityEngine;

namespace AlienBiomes
{
    public class CompProperties_HediffEffectorWithRadius : CompProperties
    {
        public FleckDef fleckReleased;
        public int releaseRadius;
        public HediffDef appliedHediff = null;
        public ThoughtDef appliedThought = null;
        public SoundDef soundOnRelease;
        public int ?tickInterval = 2500; // 2500 = a check every in-game hour.
        public Color radiusOutlineColor; // (r, g, b, a)
        public bool onlyAffectHumanlike = true;

        public CompProperties_HediffEffectorWithRadius() => compClass = typeof(Comp_HediffEffectorWithRadius);

    }
}
