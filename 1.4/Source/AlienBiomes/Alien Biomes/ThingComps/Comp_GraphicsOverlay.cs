using RimWorld;
using Verse;
using UnityEngine;

namespace AlienBiomes
{
    [StaticConstructorOnStartup]
    public class Comp_GraphicsOverlay : ThingComp
    {
        public CompProperties_GraphicsOverlay Props => (CompProperties_GraphicsOverlay)props;
        
        /// <summary>
        /// Renders additional graphics on a parent thing.
        /// XML, drawerType = RealtimeOnly || MapMeshAndRealTime.
        /// </summary>
        public override void PostDraw()
        {
            base.PostDraw();

            var parentPlant = parent as Plant;
            float dP = GenLocalDate.DayPercent(parent.Map); // Time of day as a %
            float pGrowth = parent.def.plant.visualSizeRange.LerpThroughRange(parentPlant.Growth);
            float maxG = parent.def.plant.visualSizeRange.TrueMax; // max growth stage of a plant

            for (int i = 0; i < Props.graphicElements.Count; i++)
            {
                if ((dP > Props.timeRangeDisplayed.min && dP < 1f) || (dP < Props.timeRangeDisplayed.max && dP > 0f))
                {
                    Props.graphicElements[i].Graphic.drawSize = new Vector2(pGrowth, pGrowth);
                    Props.graphicElements[i].Graphic.data.drawOffset.z = pGrowth / 2.5f;
                    Props.graphicElements[i].Graphic.Draw(parent.DrawPos, parent.Rotation, parent);

                    // Extra step for crystals only.
                    if (parent.def.plant.visualSizeRange.max == maxG)
                    {
                        if (parent.def.defName == "SZ_BlueColossalCrystalOne" 
                            || parent.def.defName == "SZ_GreenColossalCrystalOne")
                        {
                            float z2 = 0.83f;

                            Props.graphicElements[0].drawOffset.z = pGrowth * z2;
                            Props.graphicElements[1].drawOffset.z = (pGrowth * z2) + 0.75f;
                        }

                        if (parent.def.defName == "SZ_BlueColossalCrystalTwo" 
                            || parent.def.defName == "SZ_GreenColossalCrystalTwo")
                        {
                            float z2 = 0.67f;

                            Props.graphicElements[0].drawOffset.z = pGrowth * z2;
                            Props.graphicElements[1].drawOffset.z = (pGrowth * z2) + 0.20f;
                        }
                    }
                }
            }
        }
    }
}
