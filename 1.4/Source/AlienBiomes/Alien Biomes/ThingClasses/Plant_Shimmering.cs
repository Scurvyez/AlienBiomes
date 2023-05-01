using UnityEngine;
using Verse;
using RimWorld;
using System;

namespace AlienBiomes
{
    public class Plant_Shimmering : Plant
    {
        public override void PostPostMake()
        {
            base.PostPostMake();

            if (Graphic.data.shaderType.ToString() == "TransparentPlantShimmer")
            {
                Log.Message("[<color=#4494E3FF>AlienBiomes</color>] hashOffset: " + thingIDNumber);
                UpdateShaderParams();
            }
        }

        private void UpdateShaderParams()
        {
            MaterialPropertyBlock props = new ();
            props.SetFloat("_HashOffset", thingIDNumber);
        }
    }
}
