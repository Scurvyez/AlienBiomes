using Verse;

namespace AlienBiomes
{
    public class ModExt_PlantAcidSplash : DefModExtension
    {
        public float splashChance = 0.65f;
        public IntRange splashHits = new(1, 2);
        public FloatRange splashDamageFrac = new(0.15f, 0.35f);

        public float splashWeightAtLowCoverage = 1.4f;
        public float splashWeightAtHighCoverage = 0.7f;
        public float coverageScale = 2.5f;
    }
}