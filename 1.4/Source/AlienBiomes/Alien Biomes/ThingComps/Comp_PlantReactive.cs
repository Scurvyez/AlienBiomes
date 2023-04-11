using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using Verse;
using Verse.Sound;
using RimWorld;

namespace AlienBiomes
{
    public class Comp_PlantReactive : ThingComp
    {
        private CompProperties_PlantReactive Props => (CompProperties_PlantReactive)props;
        private Dictionary<Pawn, float> pawnEnterTimes = new Dictionary<Pawn, float>();
        private float curWindSpeed;

        private void UpdateShaderParams(Material mat, float curWindSpeed, GraphicDataAB extraGraphicProp)
        {
            mat.SetFloat("_distortionScrollSpeed", extraGraphicProp.shaderScrollSpeedFactor + (curWindSpeed / 10f));
            mat.SetFloat("_distortionScale", extraGraphicProp.shaderScaleFactor + (curWindSpeed / 10f));
            mat.SetFloat("_distortionIntensity", extraGraphicProp.shaderIntensityFactor + (curWindSpeed / 10f));
        }

        public override void CompTickLong()
        {
            IEnumerable<Pawn> pawns = parent.Position.GetThingList(parent.Map).OfType<Pawn>();
            foreach (Pawn pawn in pawns)
            {
                if (!pawnEnterTimes.ContainsKey(pawn))
                {
                    pawnEnterTimes[pawn] = Time.time;
                }

                if (parent is not Plant parentPlant) return;

                float pGrowth = parent.def.plant.visualSizeRange.LerpThroughRange(parentPlant.Growth);

                for (int i = 0; i < Props.graphicElements.Count; i++)
                {
                    Graphic extraGraphic = Props.graphicElements[i].Graphic;
                    GraphicDataAB extraGraphicProp = Props.graphicElements[i];

                    float plantSize = parentPlant.def.graphicData.drawSize.x * pGrowth;
                    float graphicSize = extraGraphic.data.drawSize.y * extraGraphic.data.drawSize.y;
                    float offset = (plantSize - graphicSize) / 2f;
                    Material mat = extraGraphic.data.Graphic.MatSingle;

                    Vector3 pos = parentPlant.TrueCenter();
                    pos.y += 0.01f;
                    pos.z += offset;
                    extraGraphic.drawSize = new Vector2(plantSize, plantSize);

                    curWindSpeed = parent.Map.windManager.WindSpeed;
                    UpdateShaderParams(mat, curWindSpeed, extraGraphicProp);

                    // Get the time passed since the pawn entered the cell
                    float timePassed = Time.time - pawnEnterTimes[pawn];

                    // Calculate the alpha value based on the time passed
                    float alpha = 1.0f - Mathf.Clamp01(timePassed / 360);

                    // Set the alpha value of the material color
                    mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, alpha);

                    // Draw the extra graphic
                    extraGraphic.Draw(pos, parent.Rotation, parent);
                }
            }

            // Remove pawns that are no longer in the cell
            List<Pawn> pawnsToRemove = new List<Pawn>();
            foreach (var kvp in pawnEnterTimes)
            {
                Pawn pawn = kvp.Key;
                if (!pawn.Spawned || pawn.Position != parent.Position)
                {
                    pawnsToRemove.Add(pawn);
                }
            }

            foreach (Pawn pawn in pawnsToRemove)
            {
                pawnEnterTimes.Remove(pawn);
            }
        }
    }

}
