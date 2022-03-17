using System.Collections.Generic;
using RimWorld;
using UnityEngine;
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

        public override void DrawGhost(IntVec3 center, Rot4 rot, ThingDef thingDef, Color ghostCol, AltitudeLayer drawAltitude, Thing thing = null)
        {
            Vector3 centerVec = center.ToVector3ShiftedWithAltitude(drawAltitude);
            for (int i = 0; i < graphicElements.Count; i++)
            {
                GhostUtility.GhostGraphicFor(graphicElements[i].Graphic, thingDef, ghostCol).DrawFromDef(centerVec, rot, thingDef);
                centerVec.y += 0.04054054f;
            }
        }
    }
}
