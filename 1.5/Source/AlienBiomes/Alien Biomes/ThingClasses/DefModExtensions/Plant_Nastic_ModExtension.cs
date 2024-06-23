using UnityEngine;
using Verse;

namespace AlienBiomes
{
    public class PlantNastic_ModExtension : DefModExtension
    {
        // states
        public bool isTouchSensitive;
        public bool isDamaging;
        public bool isAutochorous;
        public bool givesHediff;

        // graphics
        public bool isVisuallyReactive;
        public int texInstances = 4;
        public float minScale = 0.1f;
        public float scaleDeltaDecrease = 0.08f;
        public float scaleDeltaIncrease = 0.01f;
        public float[] scaleDeltaCache = null;

        // effects
        public bool emitFlecks;
        public int fleckBurstCount = 1;
        public FleckDef fleckDef = null;
        public FloatRange fleckScale;
        public Color colorA = Color.white;
        public Color colorB = Color.white;

        // all other fields
        public int gasReleaseCooldown = 2000;
        public float effectRadius = 2f;
        public IntRange gasDamageRange = new IntRange(1, 2);
        public float gasDamageEffectRadius = 2f;
        public HediffDef hediffToGive = null;
    }
}
