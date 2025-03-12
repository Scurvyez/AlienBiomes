using JetBrains.Annotations;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    [UsedImplicitly]
    public class Plant_Nastic_ModExt : DefModExtension
    {
        #region Miscellaneous
        public float effectRadius = 2f;
        #endregion
        
        #region States
        public bool isTouchSensitive;
        public bool isDamaging;
        public bool isAutochorous;
        #endregion
        
        #region VFX
        public bool isVisuallyReactive;
        public float visuallyReactiveThreshold = 0f;
        public int texInstances = 4;
        public float minScale = 0.1f;
        public float scaleDeltaDecrease = 0.08f;
        public float scaleDeltaIncrease = 0.01f;
        public float[] scaleDeltaCache = null;
        public int fleckBurstCount = 1;
        public FleckDef fleckDef = null;
        public FloatRange fleckScale = FloatRange.One;
        #endregion
        
        #region SFX
        public SoundDef touchSFX = null;
        #endregion
        
        #region GivesHediff
        public float givesHediffRadius = 2f;
        public float givesHediffGrowthThreshold = 0f;
        public HediffDef hediffToGive = null;
        public float hediffChance = 1f;
        public Color hediffEffectRadiusColor = Color.white;
        #endregion
        
        #region Explosions
        public float explosionGrowthThreshold = 0f;
        public int explosionReleaseCooldown = 2000;
        public DamageDef explosionDamageDef = null;
        public IntRange explosionDamage = new (1, 2);
        public float explosionDamageEffectRadius = 2f;
        public Color explosionEffectRadiusColor = Color.white;
        #endregion
    }
}