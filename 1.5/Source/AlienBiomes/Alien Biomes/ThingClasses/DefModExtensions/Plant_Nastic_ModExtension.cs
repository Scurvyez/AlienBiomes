using UnityEngine;
using Verse;
using System.Collections.Generic;

namespace AlienBiomes
{
    public class PlantNastic_ModExtension : DefModExtension
    {
        // states
        public bool isTouchSensitive;
        public bool isDamaging;
        public bool isAutochorous;

        // graphics
        public bool isVisuallyReactive;
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

        // all other fields
        public float effectRadius = 2f;
        public int gasReleaseCooldown = 2000;
        public IntRange gasDamageRange = new IntRange(1, 2);
    }
}
