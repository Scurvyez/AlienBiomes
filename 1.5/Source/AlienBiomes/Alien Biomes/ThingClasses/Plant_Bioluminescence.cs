using RimWorld;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    public class Plant_Bioluminescence : Plant
    {
        private Color defaultColor;
        private Color modifiedColor;
        //private float sunStrength;
        private Plant_Bioluminescence_ModExtension bioExt;
        private MapComponent_PlantGetter plantGetter;

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            defaultColor = Graphic.Color;
            modifiedColor = defaultColor;
            bioExt = def.GetModExtension<Plant_Bioluminescence_ModExtension>();
            plantGetter = map.GetComponent<MapComponent_PlantGetter>();
        }

        public override void Print(SectionLayer layer)
        {
            base.Print(layer);
            if (bioExt != null && (plantGetter.SunStrength > 0 && plantGetter.SunStrength < 1))
            {
                modifiedColor.a = Mathf.Clamp01(0.5f - plantGetter.SunStrength) * bioExt.alphaMultiplier;
                Graphic.MatSingleFor(this).SetColor("_Color", modifiedColor);
            }
        }
    }
}
