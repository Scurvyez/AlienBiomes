using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    public class Plant_Nastic : Plant
    {
        private Plant_Nastic_ModExtension plantExt;

        public float currentScale = 1f;
        public int touchSensitiveStartTime;

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);

            currentScale = def.graphicData.drawSize.x;
            plantExt = def.GetModExtension<Plant_Nastic_ModExtension>();
            IntVec3[] cells = GenRadial.RadialCellsAround(Position, plantExt.effectRadius, useCenter: true).ToArray();
            MapComponent_PlantGetter plantGetter = map.GetComponent<MapComponent_PlantGetter>();
            foreach (IntVec3 cell in cells)
            {
                if (!plantGetter.ActiveLocationTriggers.ContainsKey(cell))
                {
                    plantGetter.ActiveLocationTriggers[cell] = new HashSet<Plant_Nastic>();
                }
                plantGetter.ActiveLocationTriggers[cell].Add(this);
            }
        }

        public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
        {
            base.DeSpawn(mode);

            MapComponent_PlantGetter plantGetter = Map?.GetComponent<MapComponent_PlantGetter>();
            if (plantGetter != null)
            {
                foreach (HashSet<Plant_Nastic> set in plantGetter.ActiveLocationTriggers.Values)
                {
                    set.Remove(this);
                }
            }
        }

        public override void Tick()
        {
            base.Tick();
            if (plantExt != null)
            {
                int timeSinceLastStep = Find.TickManager.TicksGame - touchSensitiveStartTime;
                if (timeSinceLastStep < 720)
                {
                    float easedTime = (float)timeSinceLastStep / 360f;

                    // Use linear interpolation for scaleDeltaDecrease (no easing)
                    float scaleDeltaDecreaseEased = easedTime;
                    float scaleChangeRateDecrease = Mathf.Lerp(-plantExt.scaleDeltaDecrease, 0f, scaleDeltaDecreaseEased);

                    // Use InOutCubic for scaleDeltaIncrease (slow and smooth)
                    float scaleDeltaIncreaseEased = ABEasingFunctions.EaseOutQuad(easedTime);
                    float scaleChangeRateIncrease = Mathf.Lerp(0f, plantExt.scaleDeltaIncrease, scaleDeltaIncreaseEased);

                    float scaleChangeRate = scaleChangeRateDecrease + scaleChangeRateIncrease;
                    currentScale = Mathf.Clamp(currentScale + scaleChangeRate, plantExt.minScale, 1);
                }
            }
        }

        public void DrawEffects()
        {
            if (plantExt != null)
            {
                if (plantExt.emitFlecks && plantExt.nasticEffectDef != null)
                {
                    for (int i = 0; i < plantExt.nasticEffectDef.randomGraphics.Count; i++)
                    {
                        GraphicData randGraphic = plantExt.nasticEffectDef.randomGraphics[i];
                        var randSize = UnityEngine.Random.Range(0.05f, 0.35f);
                        randGraphic.drawSize = new Vector2(randSize, randSize);
                    }
                    FleckMaker.AttachedOverlay(this, plantExt.nasticEffectDef, new Vector3(Rand.Value, 1f, Rand.Value), 1f, -1f);
                }
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref currentScale, "currentScale", 1f);
            Scribe_Values.Look(ref touchSensitiveStartTime, "touchSensitiveStartTime", 0);
        }
    }
}
