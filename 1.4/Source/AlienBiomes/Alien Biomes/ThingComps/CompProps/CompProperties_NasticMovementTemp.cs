using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    public class CompProperties_NasticMovementTemp : CompProperties
    {
        public bool reactsToCold = false;
        public bool reactsToHeat = false;
        //public float shaderScrollSpeedFactor = 0;
        //public float shaderScaleFactor = 0;
        //public float shaderIntensityFactor = 0;
        //public Texture _DistortionTex;

        public GraphicData nasticGraphicData;

        public CompProperties_NasticMovementTemp() => compClass = typeof(Comp_NasticMovementTemp);
    }
}
