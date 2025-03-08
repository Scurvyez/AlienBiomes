using JetBrains.Annotations;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    [UsedImplicitly]
    public class Plant_Nastic_ModExt : DefModExtension
    {
        #region States
        public bool isTouchSensitive;
        public bool isDamaging;
        public bool isAutochorous;
        public bool givesHediff;
        #endregion
        
        #region Visuals
        public bool isVisuallyReactive;
        public int texInstances = 4;
        public float minScale = 0.1f;
        public float scaleDeltaDecrease = 0.08f;
        public float scaleDeltaIncrease = 0.01f;
        public float[] scaleDeltaCache = null;
        public bool emitFlecks;
        public int fleckBurstCount = 1;
        public FleckDef fleckDef = null;
        public FloatRange fleckScale;
        public Color colorA = Color.white;
        public Color colorB = Color.white;
        #endregion
        
        #region Miscellaneous
        public float effectRadius = 2f;
        public int explosionReleaseCooldown = 2000;
        public DamageDef explosionDamageDef = null;
        public IntRange explosionDamage = new (1, 2);
        public float explosionDamageEffectRadius = 2f;
        public HediffDef hediffToGive = null;
        public float hediffChance = 1f;
        #endregion
    }
}