using RimWorld;
using Verse;
using UnityEngine;

namespace AlienBiomes
{
    [StaticConstructorOnStartup]
    public class Comp_GraphicsOverlay : ThingComp
    {
        public CompProperties_GraphicsOverlay Props => (CompProperties_GraphicsOverlay)props;

        private float curWindSpeed; // initialize current speed to default value

        /// <summary>
        /// Renders additional graphics on a parent thing.
        /// XML, drawerType = RealtimeOnly || MapMeshAndRealTime.
        /// </summary>
        public override void PostDraw()
        {
            base.PostDraw();

            var parentPlant = parent as Plant;
            if (parentPlant == null) return;

            float dP = GenLocalDate.DayPercent(parent.Map); // Time of day as a %
            float pGrowth = parent.def.plant.visualSizeRange.LerpThroughRange(parentPlant.Growth);
            float maxG = parent.def.plant.visualSizeRange.TrueMax; // max growth stage of a plant

            for (int i = 0; i < Props.graphicElements.Count; i++)
            {
                if ((dP > Props.timeRangeDisplayed.min && dP < 1f) || (dP < Props.timeRangeDisplayed.max && dP > 0f))
                {
                    float plantSize = parentPlant.def.graphicData.drawSize.x * pGrowth;
                    float graphicSize = Props.graphicElements[i].Graphic.data.drawSize.y * Props.graphicElements[i].Graphic.data.drawSize.y;
                    float offset = (plantSize - graphicSize) / 2f;
                    Material mat = Props.graphicElements[i].Graphic.data.Graphic.MatSingle;
                    Props.graphicElements[i].Graphic.data.DrawOffsetForRot(parent.Rotation);

                    Vector3 pos = parentPlant.TrueCenter();
                    pos.y += 0.01f;
                    pos.z += offset;
                    Props.graphicElements[i].Graphic.drawSize = new Vector2(plantSize, plantSize);

                    if (Props.graphicElements[i].Graphic.data.shaderType.ToString() == "MoteGlowDistorted")
                    {
                        curWindSpeed = parent.Map.windManager.WindSpeed;
                        var propOffset = parent.HashOffset() / -1.47e9f;
                        mat.SetFloat("_distortionScrollSpeed", (curWindSpeed / 5f) * propOffset);
                        mat.SetFloat("_distortionScale", (curWindSpeed / 8f) * propOffset);
                        mat.SetFloat("_distortionIntensity", (curWindSpeed / 10f) * propOffset);

                        Log.Message("<color=#4494E3FF>Plant_Mobile at: </color>" + "parent hash = " + propOffset);
                    }

                    Props.graphicElements[i].Graphic.Draw(pos, parent.Rotation, parent);

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
