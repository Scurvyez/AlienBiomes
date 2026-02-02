using RimWorld;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    public class GameCondition_DesertBloomWeatherShift : GameCondition
    {
        private const float Saturation = 0.85f;
        private const float Glow = 0.25f;
        
        private static readonly Color SkyColor = new (0.4f, 0.85f, 1.0f);
        private static readonly Color ShadowColor = new (0.4f, 0.85f, 1.0f);
        private static readonly Color OverlayColor = new (0f, 0.75f, 1.0f);
        private static readonly SkyColorSet SkyColors = new (SkyColor, ShadowColor, OverlayColor, Saturation);
        
        public override int TransitionTicks => 360;
        
        public override float SkyTargetLerpFactor(Map map)
        {
            return GameConditionUtility.LerpInOutValue(this, TransitionTicks);
        }
        
        public override SkyTarget? SkyTarget(Map map)
        {
            return new SkyTarget(Glow, SkyColors, 1f, 1f);
        }
        
        public override WeatherDef ForcedWeather()
        {
            return def.weatherDef;
        }
        
        public override void End()
        {
            base.End();
            foreach (Map map in AffectedMaps)
            {
                map.weatherManager.TransitionTo(WeatherDefOf.Clear);
            }
        }
    }
}