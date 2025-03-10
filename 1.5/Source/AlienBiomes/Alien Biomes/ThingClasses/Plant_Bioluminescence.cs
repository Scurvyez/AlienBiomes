using JetBrains.Annotations;
using RimWorld;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    [UsedImplicitly]
    public class Plant_Bioluminescence : Plant
    {
        private static readonly int Color1 = Shader.PropertyToID("_Color");
        
        private Color defaultColor;
        private Color modifiedColor;
        private Plant_Bioluminescence_ModExt bioExt;
        private MapComponent_PlantGetter plantGetter;

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            defaultColor = Graphic.Color;
            modifiedColor = defaultColor;
            bioExt = def.GetModExtension<Plant_Bioluminescence_ModExt>();
            plantGetter = map.GetComponent<MapComponent_PlantGetter>();
        }
        
        public override void Print(SectionLayer layer)
        {
            base.Print(layer);
            Rand.PushState();
            Rand.Seed = Position.GetHashCode();
            
            if (bioExt != null && plantGetter.SunStrength is > 0 and < 1)
            {
                modifiedColor.a = Mathf.Clamp01(0.5f - plantGetter.SunStrength) * bioExt.alphaMultiplier;
                Graphic.MatSingleFor(this).SetColor(Color1, modifiedColor);
            }
            Rand.PopState();
        }
    }
}