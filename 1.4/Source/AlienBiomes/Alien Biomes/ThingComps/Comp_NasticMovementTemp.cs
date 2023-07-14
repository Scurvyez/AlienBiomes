using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    public class Comp_NasticMovementTemp : ThingComp
    {
        private GameConditionManager GameConditionManager;

        private CompProperties_NasticMovementTemp Props => (CompProperties_NasticMovementTemp)props;

        public override void CompTickLong()
        {
            base.CompTickLong();
            GameConditionManager = parent.Map.GameConditionManager;
        }

        public override void PostDraw()
        {
            base.PostDraw();

            Rot4 rot = parent.Rotation;
            Vector3 pos = parent.TrueCenter();
            pos.y += 0.1f;

            if (GameConditionManager != null)
            {
                if (GameConditionManager.ConditionIsActive(GameConditionDefOf.HeatWave))
                {
                    parent.Graphic.data = Props.nasticGraphicData;
                    parent.Graphic.Draw(pos, rot, parent);
                }
            }
        }
    }
}
