using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    public class Comp_Nastic : ThingComp
    {
        public CompProperties_Nastic Props => (CompProperties_Nastic)props;
        private List<Vector3> instanceOffsets = new ();
        private int texInstances = 4;
        Graphic extraGraphic;
        Material material;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            InitializeRandomOffsets();
        }

        private void InitializeRandomOffsets()
        {
            if (parent is not Plant_Nastic parentPlant) return;

            for (int i = 0; i < texInstances; i++)
            {
                float xOffset = Rand.Range(-0.5f, 0.5f);
                float zOffset = Rand.Range(-0.5f, 0.5f);

                Vector3 instancePos = parentPlant.TrueCenter();
                instancePos.x += xOffset;
                instancePos.z += zOffset;

                instanceOffsets.Add(instancePos);
            }
        }

        public override void PostDraw()
        {
            base.PostDraw();
            if (parent is not Plant_Nastic parentPlant) return;

            extraGraphic = Props.graphicElement.Graphic;
            material = extraGraphic.MatSingle;

            float pNasticGrowth = parentPlant.def.plant.visualSizeRange.LerpThroughRange(parentPlant.Growth);
            float iGraphicSize = parentPlant.def.graphicData.drawSize.x * pNasticGrowth;
            float fGraphicSize = extraGraphic.data.drawSize.y * extraGraphic.data.drawSize.y;
            float offset = (iGraphicSize - fGraphicSize) / 10f;
            Rot4 rotation = parentPlant.Rotation;

            for (int i = 0; i < instanceOffsets.Count; i++)
            {
                // Draw the mesh with the modified UV coordinates
                Vector3 drawPos = new Vector3(instanceOffsets[i].x, parentPlant.def.altitudeLayer.AltitudeFor(), instanceOffsets[i].z + offset);
                Matrix4x4 matrix = Matrix4x4.TRS(drawPos, rotation.AsQuat, new Vector3(parentPlant.currentScale, 1f, parentPlant.currentScale));
                Graphics.DrawMesh(MeshPool.plane10, matrix, material, 0, null, 0, null, false, false, false);
            }
        }
    }
}
