using RimWorld;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    public class Plant_Bioluminescence : Plant
    {
        private Color defaultColor;
        private Plant_Bioluminescence_ModExtension bioExt;

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            defaultColor = Graphic.Color;
            bioExt = def.GetModExtension<Plant_Bioluminescence_ModExtension>();
        }

        public override void Draw()
        {
            base.Draw();
            if (bioExt != null)
            {
                Color modifiedColor = defaultColor;
                float sunStrength = GenCelestial.CurCelestialSunGlow(Map);
                modifiedColor.a = Mathf.Clamp01((0.5f - sunStrength)) * bioExt.alphaMultiplier;
                Graphic.MatSingleFor(this).SetColor("_Color", modifiedColor);
            }
        }
    }
}
