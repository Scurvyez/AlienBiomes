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
        /// drawerType = RealtimeOnly || MapMeshAndRealTime.
        /// </summary>
        public override void PostDraw()
        {
            base.PostDraw();
            
            float dayPercent = GenLocalDate.DayPercent(parent.Map);
            float vSR = parent.def.plant.visualSizeRange.LerpThroughRange((parent as Plant).Growth);
            float maxGrowth = parent.def.plant.visualSizeRange.TrueMax;
            CompProperties_GraphicsOverlay props = Props;

            for (int i = 0; i < props.graphicElements.Count; i++)
            {
                if ((dayPercent > Props.timeRangeDisplayed.min && dayPercent < 1f) || (dayPercent < Props.timeRangeDisplayed.max && dayPercent > 0f))
                {
                    props.graphicElements[i].Graphic.Draw(parent.DrawPos, parent.Rotation, parent);

                    // Extra step for crystals only.
                    if (parent.def.plant.visualSizeRange.max == maxGrowth)
                    {
                        if (parent.def.defName == "SZ_BlueColossalCrystalOne" || parent.def.defName == "SZ_GreenColossalCrystalOne")
                        {
                            float z2 = 0.83f;

                            props.graphicElements[0].drawOffset.z = vSR * z2;
                            props.graphicElements[1].drawOffset.z = (vSR * z2) + 0.75f;
                            props.graphicElements[2].drawOffset.z = vSR * z2;
                            props.graphicElements[3].drawOffset.z = vSR * z2;
                        }

                        if (parent.def.defName == "SZ_BlueColossalCrystalTwo" || parent.def.defName == "SZ_GreenColossalCrystalTwo")
                        {
                            float z2 = 0.67f;

                            props.graphicElements[0].drawOffset.z = vSR * z2;
                            props.graphicElements[1].drawOffset.z = (vSR * z2) + 0.20f;
                            props.graphicElements[2].drawOffset.z = vSR * z2;
                            props.graphicElements[3].drawOffset.z = vSR * z2;
                        }
                    }
                }
            }
        }
    }
}
