using RimWorld;
using Verse;
using UnityEngine;

namespace AlienBiomes
{
    /*[StaticConstructorOnStartup]
    public class Comp_GraphicsOverlay : ThingComp
    {
        public CompProperties_GraphicsOverlay Props => (CompProperties_GraphicsOverlay)props;
        
        private static readonly int DistortionScrollSpeed = Shader.PropertyToID("_distortionScrollSpeed");
        private static readonly int DistortionScale = Shader.PropertyToID("_distortionScale");
        private static readonly int DistortionIntensity = Shader.PropertyToID("_distortionIntensity");
        
        private float _curWindSpeed;
        
        /// <summary>
        /// Updates each shader parameter attached to each additional graphic.
        /// </summary>
        private static void UpdateShaderParams(Material mat, float curWindSpeed, GraphicDataAB extraGraphicProp)
        {
            float windFactor = curWindSpeed / 10f;
            
            mat.SetFloat(DistortionScrollSpeed, extraGraphicProp.shaderScrollSpeedFactor + windFactor);
            mat.SetFloat(DistortionScale, extraGraphicProp.shaderScaleFactor + windFactor);
            mat.SetFloat(DistortionIntensity, extraGraphicProp.shaderIntensityFactor + windFactor);
        }

        public override void CompTickLong()
        {
            base.CompTickLong();
            _curWindSpeed = parent.Map.windManager.WindSpeed;
        }

        /// <summary>
        /// Renders additional graphics on a parent thing.
        /// XML, drawerType = RealtimeOnly || MapMeshAndRealTime.
        /// </summary>
        public override void PostDraw()
        {
            base.PostDraw();

            if (parent is not Plant parentPlant) return;
            
            float dP = GenLocalDate.DayPercent(parent.Map); // Time of day as a %
            float pGrowth = parent.def.plant.visualSizeRange.LerpThroughRange(parentPlant.Growth);
            float maxG = parent.def.plant.visualSizeRange.TrueMax; // max growth stage of a plant

            for (int i = 0; i < Props.graphicElements.Count; i++)
            {
                Graphic extraGraphic = Props.graphicElements[i].Graphic;
                GraphicDataAB extraGraphicProp = Props.graphicElements[i];

                if ((dP > extraGraphicProp.timeRangeDisplayed.min && dP < 1f) || (dP < extraGraphicProp.timeRangeDisplayed.max && dP > 0f))
                {
                    float plantSize = parentPlant.def.graphicData.drawSize.x * pGrowth;
                    float graphicSize = extraGraphic.data.drawSize.y * extraGraphic.data.drawSize.y;
                    float offset = (plantSize - graphicSize) / 2f;
                    Material mat = extraGraphic.data.Graphic.MatSingle;
                    Rot4 rotation = parent.Rotation;
                    
                    Vector3 pos = parentPlant.TrueCenter();
                    //pos.y += 0.01f;
                    pos.z += offset;
                    extraGraphic.drawSize = new Vector2(plantSize, plantSize);

                    if (extraGraphic.data.shaderType.ToString() == "MoteGlowDistorted" || extraGraphic.data.shaderType.ToString() == "MoteDistorted")
                    {
                        _curWindSpeed = parent.Map.windManager.WindSpeed;
                        UpdateShaderParams(mat, _curWindSpeed, extraGraphicProp);
                    }

                    extraGraphic.Draw(pos, rotation, parent);

                    // Extra step for crystals only.
                    if (Mathf.Approximately(parent.def.plant.visualSizeRange.max, maxG))
                    {
                        if (parent.def.defName == "SZ_BlueColossalCrystalOne" || parent.def.defName == "SZ_GreenColossalCrystalOne")
                        {
                            float z2 = 0.83f;

                            Props.graphicElements[0].drawOffset.z = pGrowth * z2;
                            Props.graphicElements[1].drawOffset.z = (pGrowth * z2) + 0.75f;
                        }

                        if (parent.def.defName == "SZ_BlueColossalCrystalTwo" || parent.def.defName == "SZ_GreenColossalCrystalTwo")
                        {
                            float z2 = 0.67f;

                            Props.graphicElements[0].drawOffset.z = pGrowth * z2;
                            Props.graphicElements[1].drawOffset.z = (pGrowth * z2) + 0.20f;
                        }
                    }
                }
            }
        }
    }*/
}