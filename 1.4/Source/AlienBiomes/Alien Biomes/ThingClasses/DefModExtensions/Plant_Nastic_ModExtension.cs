using Verse;

namespace AlienBiomes
{
    public class Plant_Nastic_ModExtension : DefModExtension
    {
        // states
        public bool isTouchSensitive;
        public bool isDamaging;
        public bool isAutochorous;

        // activation radius for all
        public float effectRadius = 2f;

        // graphics
        public float minScale = 0.1f;
        public float scaleDeltaDecrease = 0.08f;
        public float scaleDeltaIncrease = 0.01f;

        // effects
        public bool emitFlecks;
        public FleckDef nasticEffectDef = null;
    }
}
