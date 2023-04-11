using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;
using Verse;
using UnityEngine;

namespace AlienBiomes
{
    public class CompProperties_PlantReactive : CompProperties
    {
        public List<GraphicDataAB> graphicElements;

        public CompProperties_PlantReactive() => compClass = typeof(Comp_PlantReactive);
    }
}
