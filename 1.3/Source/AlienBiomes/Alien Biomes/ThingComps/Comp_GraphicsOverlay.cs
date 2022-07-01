using RimWorld;
using Verse;

namespace AlienBiomes
{
    [StaticConstructorOnStartup]
    public class Comp_GraphicsOverlay : CompFireOverlayBase
    {
        public new CompProperties_GraphicsOverlay Props => (CompProperties_GraphicsOverlay)props;
        public Plant Plant = new();
        
        /// <summary>
        /// Renders additional graphics on a parent thing.
        /// XML, drawerType = RealtimeOnly || MapMeshAndRealTime.
        /// </summary>
        public override void PostDraw()
        {
            base.PostDraw();
            
            float dP = GenLocalDate.DayPercent(parent.Map); // Time of day as a %
            float vSR = parent.def.plant.visualSizeRange.LerpThroughRange((parent as Plant).Growth);
            float mG = parent.def.plant.visualSizeRange.TrueMax; // max growth stage of a plant
            CompProperties_GraphicsOverlay props = Props;

            for (int i = 0; i < props.graphicElements.Count; i++)
            {
                if ((dP > Props.timeRangeDisplayed.min && dP < 1f) 
                    || (dP < Props.timeRangeDisplayed.max && dP > 0f))
                {
                    props.graphicElements[i].Graphic.Draw(parent.DrawPos, parent.Rotation, parent);

                    // Extra step for crystals only.
                    if (parent.def.plant.visualSizeRange.max == mG)
                    {
                        if (parent.def.defName == "SZ_BlueColossalCrystalOne" 
                            || parent.def.defName == "SZ_GreenColossalCrystalOne")
                        {
                            float z2 = 0.83f;

                            props.graphicElements[0].drawOffset.z = vSR * z2;
                            props.graphicElements[1].drawOffset.z = (vSR * z2) + 0.75f;
                        }

                        if (parent.def.defName == "SZ_BlueColossalCrystalTwo" 
                            || parent.def.defName == "SZ_GreenColossalCrystalTwo")
                        {
                            float z2 = 0.67f;

                            props.graphicElements[0].drawOffset.z = vSR * z2;
                            props.graphicElements[1].drawOffset.z = (vSR * z2) + 0.20f;
                        }
                    }
                }
            }
        }
    }
}
