using RimWorld;
using Verse;
using UnityEngine;
using System.Collections.Generic;

namespace AlienBiomes
{
    public class Plant_Shimmering : Plant
    {
        private MaterialPropertyBlock MPB;
        private List<Color> randomColors;

        public override void Print(SectionLayer layer)
        {
            base.Print(layer);

            if (Graphic.data.shaderType == ABDefOf.TransparentPlantShimmer)
            {
                float randomShimmerSpeed = Random.Range(0.0f, 1.0f);
                MPB = new MaterialPropertyBlock();
                MPB.SetFloat("_ShimmerSpeed", randomShimmerSpeed);
            }
            else if (Graphic.data.shaderType == ABDefOf.TransparentPlantPulse)
            {
                float randomShimmerSpeed = Random.Range(0.0f, 10.0f);
                MPB = new MaterialPropertyBlock();
                MPB.SetFloat("_PulseSpeed", randomShimmerSpeed);
                MPB.SetFloat("_PulseLength", randomShimmerSpeed);

                randomColors = GenerateRandomColors(10);
                int randomIndex = Random.Range(0, randomColors.Count);
                Color randomColor = randomColors[randomIndex];
                MPB.SetColor("_Color", randomColor);
            }
        }

        private List<Color> GenerateRandomColors(int count)
        {
            List<Color> colors = new();
            for (int i = 0; i < count; i++)
            {
                Color randomColor = new(Random.value, Random.value, Random.value);
                colors.Add(randomColor);
            }
            return colors;
        }
    }
}
