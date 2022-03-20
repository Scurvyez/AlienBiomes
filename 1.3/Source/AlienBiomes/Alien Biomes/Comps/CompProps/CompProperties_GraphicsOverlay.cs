using System.Collections.Generic;
using RimWorld;
using Verse;

namespace AlienBiomes
{
    public class CompProperties_GraphicsOverlay : CompProperties_FireOverlay
    {
        public List<GraphicData> graphicElements;
        public FloatRange timeRangeDisplayed; // XML = someFloat~someFloat

        public CompProperties_GraphicsOverlay()
        {
            compClass = typeof(Comp_GraphicsOverlay);
        }
    }
}
