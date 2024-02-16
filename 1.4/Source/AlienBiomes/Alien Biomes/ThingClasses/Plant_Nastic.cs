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

        public float CurrentScale = 1f;
        public int TouchSensitiveStartTime;
        public Color FleckEmissionColor;
        private List<Vector3> InstanceOffsets = new ();
        private int timeSinceLastStep;
        private const int MaxTicks = 720;
        private float CurPlantGrowth;
        private Vector3 drawPos = new Vector3 (0, 0, 0);
        private Mesh mesh = MeshPool.plane10;
        private float scaleY;
        private float drawSizeY;
        private Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            plantExt = def.GetModExtension<Plant_Nastic_ModExtension>();
            drawSizeY = def.graphicData.drawSize.y;

            if (plantExt != null)
            {
                InitializeRandomOffsets();
                PrecomputeScaleDeltas();

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
            CurPlantGrowth = def.plant.visualSizeRange.LerpThroughRange(Growth);

            if (plantExt != null)
            {
                timeSinceLastStep = Find.TickManager.TicksGame - TouchSensitiveStartTime;
                if (timeSinceLastStep < MaxTicks)
                {
                    float scaleChangeRate = plantExt.scaleDeltaCache[timeSinceLastStep];
                    CurrentScale = Mathf.Clamp(CurrentScale + scaleChangeRate, plantExt.minScale, 1);
                }
            }
        }

        public override void DrawAt(Vector3 drawLoc, bool flip = false)
        {
            for (int i = 0; i < InstanceOffsets.Count; i++)
            {
                // Draw the mesh with the modified UV coordinates
                drawPos = InstanceOffsets[i];

                // Calculate the adjusted z-coordinate based on the change in scale
                // This ensures our individual textures on the mesh shrink down to their base and not into their center
                scaleY = Mathf.Lerp(-1f, 0.5f, CurrentScale);
                drawPos.z += drawSizeY * scaleY / 2f;

                matrix = Matrix4x4.TRS(drawPos, Rotation.AsQuat, new Vector3(CurrentScale * CurPlantGrowth, 1, CurrentScale * CurPlantGrowth));
                Graphics.DrawMesh(mesh, matrix, Graphic.MatSingle, 0, null, 0, null, false, false, false);
            }
        }

        /// <summary>
        /// Calculates the initial position of each texture on our mesh on spawn.
        /// </summary>
        private void InitializeRandomOffsets()
        {
            for (int i = 0; i < plantExt.texInstances; i++)
            {
                float xOffset = Rand.Range(-0.5f, 0.5f);
                float zOffset = Rand.Range(-0.5f, 0.5f);

                Vector3 instancePos = DrawPos;
                instancePos.x += xOffset;
                instancePos.z += zOffset;

                InstanceOffsets.Add(instancePos);
            }
        }

        /// <summary>
        /// Calculates one full scale cycle for the plant on spawn.
        /// </summary>
        private void PrecomputeScaleDeltas()
        {
            int cycleTicks = MaxTicks; // Assuming one full cycle
            plantExt.scaleDeltaCache = new float[cycleTicks];

            for (int i = 0; i < cycleTicks; i++)
            {
                float easedTime = i / 360f;

                float scaleDeltaDecreaseEased = easedTime;
                float scaleChangeRateDecrease = Mathf.Lerp(-plantExt.scaleDeltaDecrease, 0f, scaleDeltaDecreaseEased);

                float scaleDeltaIncreaseEased = ABEasingFunctions.EaseOutQuad(easedTime);
                float scaleChangeRateIncrease = Mathf.Lerp(0f, plantExt.scaleDeltaIncrease, scaleDeltaIncreaseEased);

                float scaleChangeRate = scaleChangeRateDecrease + scaleChangeRateIncrease;
                plantExt.scaleDeltaCache[i] = scaleChangeRate;
            }
        }

        public void DrawEffects()
        {
            if (plantExt != null && plantExt.emitFlecks && plantExt.fleckDef != null)
            {
                for (int i = 0; i < plantExt.fleckBurstCount; ++i)
                {
                    FleckEmissionColor = Color.Lerp(plantExt.colorA, plantExt.colorB, Rand.Value);
                    Vector3 drawPos = new Vector3(DrawPos.x + Rand.InsideUnitCircleVec3.x, DrawPos.y, DrawPos.z + Rand.InsideUnitCircleVec3.z);

                    FleckCreationData fCD = FleckMaker.GetDataStatic(drawPos, Map, plantExt.fleckDef, plantExt.fleckScale.RandomInRange);
                    fCD.rotationRate = Rand.RangeInclusive(-240, 240);
                    fCD.instanceColor = new Color?(FleckEmissionColor);
                    Map.flecks.CreateFleck(fCD);
                }
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref CurrentScale, "CurrentScale", 1f);
            Scribe_Values.Look(ref TouchSensitiveStartTime, "TouchSensitiveStartTime", 0);
        }
    }
}
