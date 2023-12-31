using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    public class Comp_Nastic : ThingComp
    {
        [TweakValue("AB_Plant_OffsetY", 0f, 50f)]
        private static float AB_GlobalPlant_OffsetY = 0f;
        
        public CompProperties_Nastic Props => (CompProperties_Nastic)props;
        private List<Vector3> instanceOffsets = new ();
        private int texInstances = 4;

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

            Graphic extraGraphic = Props.graphicElement.Graphic;
            Material material = extraGraphic.MatSingle;
            float pGrowth = parent.def.plant.visualSizeRange.LerpThroughRange(parentPlant.Growth);

            for (int i = 0; i < instanceOffsets.Count; i++)
            {
                // Draw the mesh with the modified UV coordinates
                Vector3 drawPos = instanceOffsets[i];
                drawPos.y += AB_GlobalPlant_OffsetY;

                // Calculate the adjusted z-coordinate based on the change in scale
                // This ensures our individual textures on the mesh shrink down to their base and not into their center
                float scaleY = Mathf.Lerp(0.5f, 1f, parentPlant.currentScale);
                drawPos.z += parentPlant.def.graphicData.drawSize.y * scaleY / 2f;

                Matrix4x4 matrix = Matrix4x4.TRS(drawPos, parentPlant.Rotation.AsQuat, new Vector3(parentPlant.currentScale * pGrowth, 1, parentPlant.currentScale * pGrowth));
                Graphics.DrawMesh(MeshPool.plane10, matrix, material, 0, null, 0, null, false, false, false);
            }
        }
    }
}
