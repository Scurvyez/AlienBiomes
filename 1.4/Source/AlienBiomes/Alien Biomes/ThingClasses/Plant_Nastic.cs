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

        public float currentScale {  get; set; }
        public float minScale = 0.1f;
        public float scaleChangeRate = 0.05f;
        public bool touchSensitiveSwitch;
        public int delayTimer = 180;

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
            if (touchSensitiveSwitch) // check if pawn in range
            {
                delayTimer--; // start counting down from 180
                if (delayTimer > 0)
                {
                    currentScale = Mathf.Max(currentScale - scaleChangeRate, minScale); // scale down
                }
                else
                {
                    currentScale = Mathf.Min(currentScale + scaleChangeRate, 1.0f); // scale back up
                }
            }
            touchSensitiveSwitch = false;
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
                        var randSize = Random.Range(0.05f, 0.35f);
                        randGraphic.drawSize = new Vector2(randSize, randSize);
                    }
                    FleckMaker.AttachedOverlay(this, plantExt.nasticEffectDef, new Vector3(Rand.Value, 1f, Rand.Value), 1f, -1f);
                }
            }
        }
    }
}
