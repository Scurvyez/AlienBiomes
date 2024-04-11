using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    public class GameCondition_CrystallineRedshift : GameCondition
    {
        private List<SkyOverlay> overlays = new List<SkyOverlay>();
        private static Color skyColor = new Color(0.9f, 0.1f, 0.1f);
        private static Color shadowColor = Color.white;
        private static Color overlayColor = new Color(1f, 1f, 1f);
        private static float saturation = 0.75f;
        private float glow = 0.25f;
        private int duration = 720;

        public override int TransitionTicks => 360;

        public override float SkyTargetLerpFactor(Map map)
        {
            return GameConditionUtility.LerpInOutValue(this, TransitionTicks);
        }

        public static readonly SkyColorSet TestSkyColors = new SkyColorSet(skyColor, shadowColor, overlayColor, saturation);

        public override SkyTarget? SkyTarget(Map map)
        {
            return new SkyTarget(glow, TestSkyColors, 1f, 1f);
        }

        public override bool Expired
        {
            get
            {
                if (!Permanent)
                {
                    return Find.TickManager.TicksGame > startTick + duration;
                }
                return false;
            }
        }

        public override void GameConditionTick()
        {
            base.GameConditionTick();
            List<Map> affectedMaps = base.AffectedMaps;
            for (int i = 0; i < overlays.Count; i++)
            {
                for (int j = 0; j < affectedMaps.Count; j++)
                {
                    overlays[i].TickOverlay(affectedMaps[j]);
                }
            }
        }

        public override void GameConditionDraw(Map map)
        {
            for (int i = 0; i < overlays.Count; i++)
            {
                overlays[i].DrawOverlay(map);
            }
        }
    }
}
