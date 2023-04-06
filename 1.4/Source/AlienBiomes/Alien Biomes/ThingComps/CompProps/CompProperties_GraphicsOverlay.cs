using System.Collections.Generic;
using RimWorld;
using Verse;

namespace AlienBiomes
{
    public class CompProperties_GraphicsOverlay : CompProperties
    {
        public List<GraphicData> graphicElements;
        public FloatRange timeRangeDisplayed; // XML = someFloat~someFloat

        public CompProperties_GraphicsOverlay()
        {
            compClass = typeof(Comp_GraphicsOverlay);
        }

        public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
        {
            foreach (string error in base.ConfigErrors(parentDef))
            {
                yield return error;
            }
            if (graphicElements == null)
            {
                yield return "Oops! We couldn't find a texture for <graphicElements>, please provide at least one.";
            }
        }
    }
}
