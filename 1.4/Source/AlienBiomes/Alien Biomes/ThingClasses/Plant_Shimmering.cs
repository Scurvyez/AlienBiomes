using UnityEngine;
using Verse;
using RimWorld;
using System;

namespace AlienBiomes
{
    public class Plant_Shimmering : Plant
    {
        private System.Random rand = new ();
        private float randFloat;

        public override void TickLong()
        {
            base.TickLong();
            randFloat = (float)rand.NextDouble();

            if (Graphic.data.shaderType.ToString() == "TransparentPlantShimmer")
            {
                Material mat = Graphic.data.Graphic.MatSingle;
                Log.Message("[<color=#4494E3FF>AlienBiomes</color>] thingHashOffset: " + randFloat);
                UpdateShaderParams(mat);
            }
        }

        private void UpdateShaderParams(Material mat)
        {
            mat.SetFloat("_HashOffset", randFloat);
        }
    }
}
