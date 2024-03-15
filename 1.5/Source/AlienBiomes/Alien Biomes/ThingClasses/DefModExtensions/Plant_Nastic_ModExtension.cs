using UnityEngine;
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
        public int texInstances = 4;
        public float[] scaleDeltaCache = null;

        // effects
        public bool emitFlecks;
        public FleckDef fleckDef = null;
        public int fleckBurstCount = 1;
        public FloatRange fleckScale;
        public Color colorA = Color.white;
        public Color colorB = Color.white;
    }
}
