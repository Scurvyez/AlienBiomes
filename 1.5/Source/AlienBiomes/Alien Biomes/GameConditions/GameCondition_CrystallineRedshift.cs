using RimWorld;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    public class GameCondition_CrystallineRedshift : GameCondition
    {
        private static readonly Color _skyColor = new (0.9f, 0.1f, 0.1f);
        private static readonly Color _shadowColor = Color.white;
        private static readonly Color _overlayColor = new (1f, 1f, 1f);
        private const float SATURATION = 0.75f;
        private const float GLOW = 0.25f;
        private const int DURATION = 720;
        
        public static readonly SkyColorSet _skyColors = new (_skyColor, _shadowColor, _overlayColor, SATURATION);

        public override int TransitionTicks => 360;

        public override float SkyTargetLerpFactor(Map map)
        {
            return GameConditionUtility.LerpInOutValue(this, TransitionTicks);
        }

        public override SkyTarget? SkyTarget(Map map)
        {
            return new SkyTarget(GLOW, _skyColors, 1f, 1f);
        }

        public override bool Expired
        {
            get
            {
                if (!Permanent)
                {
                    return Find.TickManager.TicksGame > startTick + DURATION;
                }
                return false;
            }
        }
    }
}
