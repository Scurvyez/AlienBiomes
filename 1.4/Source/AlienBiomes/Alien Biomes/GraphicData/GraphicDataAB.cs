using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Verse;
using RimWorld;
using UnityEngine;

namespace AlienBiomes
{
    public class GraphicDataAB : GraphicData
    {
        public float shaderScrollSpeedFactor; // _distortionScrollSpeed
        public float shaderScaleFactor; // _distortionScale
        public float shaderIntensityFactor; // _distortionIntensity

        public FloatRange timeRangeDisplayed; // XML = someFloat~someFloat

        public GraphicDataAB() : base()
        {

        }
    }
}
