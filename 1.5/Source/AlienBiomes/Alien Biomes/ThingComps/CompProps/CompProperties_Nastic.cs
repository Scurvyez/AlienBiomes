using System.Collections.Generic;
using Verse;

namespace AlienBiomes
{
    public class CompProperties_Nastic : CompProperties
    {
        public FleckDef nasticEffectDef;
        public GraphicDataAB graphicElement;

        public CompProperties_Nastic() => compClass = typeof(Comp_Nastic);

        public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
        {
            foreach (string error in base.ConfigErrors(parentDef))
            {
                yield return error;
            }
            if (graphicElement == null)
            {
                yield return "[<color=#4494E3FF>AlienBiomes</color>] <color=#e36c45FF>Oops! No texture found for <graphicElement>, please provide at least one.</color>";
            }
        }
    }
}
