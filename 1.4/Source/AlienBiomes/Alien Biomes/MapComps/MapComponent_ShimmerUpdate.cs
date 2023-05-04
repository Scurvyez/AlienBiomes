using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Verse;
using UnityEngine;
using RimWorld;
using RimWorld.Planet;

namespace AlienBiomes
{
    public class MapComponent_ShimmerUpdate : MapComponent
    {
        private MaterialPropertyBlock mPB = new ();
        private List<Thing> grasses = new ();
        private List<Matrix4x4> matrices = new ();

        public MapComponent_ShimmerUpdate(Map map) : base(map)
        {
            
        }

        public override void MapComponentUpdate()
        {
            base.MapComponentUpdate();

            if (!WorldRendererUtility.WorldRenderedNow && Find.CurrentMap == map)
            {
                if (map.Biome.defName == "SZ_RadiantPlains")
                {
                    // add all grass things to the grasses list
                    grasses = map.listerThings.ThingsOfDef(AlienBiomes_ThingDefOf.SZ_RadiantGrass);

                    // clear our list of Matrix4x4's
                    matrices.Clear();

                    foreach (Thing grass in grasses)
                    {
                        // add a new Matrix4x4 w/ grass position, grass rotation, and scale of 1
                        matrices.Add(Matrix4x4.TRS(grass.DrawPos, grass.Rotation.AsQuat, Vector3.one));
                    }

                    // ensure grasses is not empty
                    if (grasses.Count > 0)
                    {
                        // set to grass thingIDNumber as an array
                        mPB.Clear();
                        mPB.SetFloatArray("_HashOffset", grasses.Select(grass => (float)grass.thingIDNumber).ToArray());
                    }

                    // set material to the material of each grass
                    Material[] materials = grasses.Select(grass => grass.Graphic.MatSingle).ToArray();

                    foreach (Material material in materials)
                    {
                        // enable instancing for the materials
                        material.enableInstancing = true;
                    }

                    // finally, draw the mesh
                    // there's too many matrices so limit it to 1023
                    Graphics.DrawMeshInstanced(MeshPool.plane10, 0, materials[0], matrices.Take(1023).ToArray(), matrices.Count, mPB);
                }
            }
        }
    }
}
