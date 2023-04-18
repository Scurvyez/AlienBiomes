using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using RimWorld;

namespace AlienBiomes
{
    public class Comp_PlantReactive : ThingComp
    {
        private CompProperties_PlantReactive Props => (CompProperties_PlantReactive)props;

        public override void CompTick()
        {
            Plant parentPlant = parent as Plant;

            List<Thing> thingsInCell = parentPlant.Map.thingGrid.ThingsListAt(parentPlant.Position);
            bool pawnInCell = thingsInCell.Any(t => t is Pawn);

            if (pawnInCell)
            {
                float pGrowth = parent.def.plant.visualSizeRange.LerpThroughRange(parentPlant.Growth);

                for (int i = 0; i < Props.graphicElements.Count; i++)
                {
                    Graphic extraGraphic = Props.graphicElements[i].Graphic;
                    GraphicData extraGraphicProp = Props.graphicElements[i];

                    float plantSize = parentPlant.def.graphicData.drawSize.x * pGrowth;
                    float graphicSize = extraGraphic.data.drawSize.y * extraGraphic.data.drawSize.y;
                    float offset = (plantSize - graphicSize) / 2f;
                    Material mat = extraGraphic.data.Graphic.MatSingle;

                    Vector3 pos = parentPlant.TrueCenter();
                    pos.y += 0.01f;
                    pos.z += offset;
                    extraGraphic.drawSize = new Vector2(plantSize, plantSize);

                    // Draw the extra graphic
                    extraGraphic.Draw(pos, parent.Rotation, parent);
                }
            }
        }
    }
}
