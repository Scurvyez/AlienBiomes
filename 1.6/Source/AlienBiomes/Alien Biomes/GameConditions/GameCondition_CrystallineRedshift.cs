using RimWorld;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    public class GameCondition_CrystallineRedshift : GameCondition
    {
        private const float Saturation = 0.75f;
        private const float Glow = 0.25f;
        
        private static readonly Color SkyColor = new (0.9f, 0.1f, 0.1f);
        private static readonly Color ShadowColor = Color.white;
        private static readonly Color OverlayColor = new (1f, 1f, 1f);
        private static readonly SkyColorSet SkyColors = new (SkyColor, ShadowColor, OverlayColor, Saturation);

        public override int TransitionTicks => 360;

        public override void Init()
        {
            Duration = 720;
        }

        public override float SkyTargetLerpFactor(Map map)
        {
            return GameConditionUtility.LerpInOutValue(this, TransitionTicks);
        }
        
        public override SkyTarget? SkyTarget(Map map)
        {
            return new SkyTarget(Glow, SkyColors, 1f, 1f);
        }
        
        public override bool Expired
        {
            get
            {
                if (!Permanent)
                {
                    return Find.TickManager.TicksGame > startTick + Duration;
                }
                return false;
            }
        }
    }
}