using RimWorld;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    public class GameCondition_DesertBloomWeatherShift : GameCondition
    {
        public override int TransitionTicks => 60;
        
        private static readonly Color _skyColor = new (0.4f, 0.85f, 1.0f);
        private static readonly Color _shadowColor = new (0.4f, 0.85f, 1.0f);
        private static readonly Color _overlayColor = new (0f, 0.75f, 1.0f);
        private const float SATURATION = 0.85f;
        private const float GLOW = 0.25f;
        private static readonly SkyColorSet _skyColors = new (_skyColor, _shadowColor, _overlayColor, SATURATION);

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
        
        public override float SkyTargetLerpFactor(Map map)
        {
            return GameConditionUtility.LerpInOutValue(this, TransitionTicks);
        }

        public override SkyTarget? SkyTarget(Map map)
        {
            return new SkyTarget(GLOW, _skyColors, 1f, 1f);
        }
    }
}