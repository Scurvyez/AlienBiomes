using System.Collections.Generic;
using Verse;
using UnityEngine;

namespace AlienBiomes
{
    /*
    public class SectionLayer_PlantShimmer : SectionLayer
    {
        private readonly List<Matrix4x4> matrices = new ();
        private Material mat;
        private List<float> offsets;
        private readonly MaterialPropertyBlock props = new ();

        public SectionLayer_PlantShimmer(Section section) : base(section)
        {
            relevantChangeTypes = MapMeshFlag.Things;
            offsets = new List<float>();
            mat = null;
            mat.enableInstancing = true;
        }

        public override void Regenerate()
        {
            matrices.Clear();
            offsets.Clear();

            foreach (IntVec3 c in section.CellRect)
            {
                foreach (Thing thing in c.GetThingList(Map))
                {
                    if (thing is Plant_Shimmering)
                    {
                        mat ??= thing.Graphic.MatSingle;
                        matrices.Add(Matrix4x4.TRS(thing.DrawPos, thing.Rotation.AsQuat, Vector3.one));
                        offsets.Add(thing.thingIDNumber);
                    }
                }
            }

            if (offsets.Count == 0)
            {
                // No shimmering plants in this section, so there's nothing to render
                return;
            }

            props.Clear();
            props.SetFloatArray("_HashOffset", offsets);
        }

        public override void DrawLayer()
        {
            Graphics.DrawMeshInstanced(MeshPool.plane10, 0, mat, matrices, props);
        }
    }
    */
}
