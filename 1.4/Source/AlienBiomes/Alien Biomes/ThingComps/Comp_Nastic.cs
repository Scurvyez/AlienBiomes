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
        private Graphic extraGraphic;
        private Material material;

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

                Vector3 instancePos = parentPlant.DrawPos;
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

            for (int i = 0; i < instanceOffsets.Count; i++)
            {
                // Draw the mesh with the modified UV coordinates
                Vector3 drawPos = instanceOffsets[i];

                // Calculate the adjusted y-coordinate based on the change in scale
                float scaleY = Mathf.Lerp(0.5f, 1f, parentPlant.currentScale);  // Adjust the 0.5f value as needed
                drawPos.z += parentPlant.def.graphicData.drawSize.y * scaleY / 2f;

                Matrix4x4 matrix = Matrix4x4.TRS(drawPos, parentPlant.Rotation.AsQuat, new Vector3(parentPlant.currentScale, 0, parentPlant.currentScale));
                Graphics.DrawMesh(MeshPool.plane10, matrix, material, 0, null, 0, null, false, false, false);
            }
        }
    }
}
