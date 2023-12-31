using RimWorld;
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
        public Color fleckEmissionColor;

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
            if (plantExt != null && plantExt.emitFlecks && plantExt.fleckDef != null)
            {
                for (int i = 0; i < plantExt.fleckBurstCount; ++i)
                {
                    fleckEmissionColor = Color.Lerp(plantExt.colorA, plantExt.colorB, Rand.Value);
                    Vector3 drawPos = new Vector3(DrawPos.x + Rand.InsideUnitCircleVec3.x, DrawPos.y, DrawPos.z + Rand.InsideUnitCircleVec3.z);

                    FleckCreationData fCD = FleckMaker.GetDataStatic(drawPos, Map, plantExt.fleckDef, plantExt.fleckScale.RandomInRange);
                    fCD.rotationRate = Rand.RangeInclusive(-240, 240);
                    fCD.instanceColor = new Color?(fleckEmissionColor);
                    Map.flecks.CreateFleck(fCD);
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
