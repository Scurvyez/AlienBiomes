using System.Collections.Generic;
using System.Linq;
using Verse;
using UnityEngine;
using System.IO;

namespace AlienBiomes
{
    [StaticConstructorOnStartup]
    public class SectionLayer_Bioluminescence : SectionLayer
    {
        //private static readonly Material BioluminescenceMat = MaterialPool.MatFrom("Things/Special/ShallowOceanWaterBioluminescence/ShallowOceanWaterBioluminescenceA", ShaderDatabase.MoteGlowDistorted);
        //private static readonly string BioluminescenceTexFolder = "Mods/AlienBiomes/Textures/Things/Special/ShallowOceanWaterBioluminescence";
        private static readonly Texture2D BioluminescenceDistortionTex = ContentFinder<Texture2D>.Get("Things/Mote/SmokeTiled", true);
        private static readonly Texture2D BioTexA = ContentFinder<Texture2D>.Get("Things/Special/ShallowOceanWaterBioluminescence/ShallowOceanWaterBioluminescenceA", true);
        private static readonly Texture2D BioTexB = ContentFinder<Texture2D>.Get("Things/Special/ShallowOceanWaterBioluminescence/ShallowOceanWaterBioluminescenceB", true);
        private static readonly Texture2D BioTexC = ContentFinder<Texture2D>.Get("Things/Special/ShallowOceanWaterBioluminescence/ShallowOceanWaterBioluminescenceC", true);
        private static readonly Texture2D BioTexD = ContentFinder<Texture2D>.Get("Things/Special/ShallowOceanWaterBioluminescence/ShallowOceanWaterBioluminescenceD", true);
        private MaterialPropertyBlock propertyBlock;
        private IEnumerable<IntVec3> affectedCells;
        private readonly int bioReach = 3; // how many cells out, from the shoreline, the bioluminescence displays

        private static readonly List<Texture2D> BioluminescenceTextures = new ();
        private Material BioluminescenceMat;

        public SectionLayer_Bioluminescence(Section section) : base(section)
        {
            propertyBlock = new MaterialPropertyBlock();
        }

        private static Material GetRandomBioluminescenceMaterial()
        {
            if (BioluminescenceTextures.Count > 0)
            {
                // Randomly select a texture from the list
                Texture2D randomTexture = BioluminescenceTextures[UnityEngine.Random.Range(0, BioluminescenceTextures.Count)];

                Log.Message("[<color=#4494E3FF>AlienBiomes</color>] <color=#e36c45FF>randomTexture is:</color> "
                + randomTexture.ToString().Colorize(Color.green));

                // Create a material using the random texture
                Material material = new (ShaderDatabase.MoteGlowDistorted);
                material.SetTexture("_MainTex", randomTexture);

                return material;
            }

            // If no textures found, fallback to a default material
            return MaterialPool.MatFrom("Things/Special/ShallowOceanWaterBioluminescence/ShallowOceanWaterBioluminescenceA", ShaderDatabase.MoteGlowDistorted);
        }

        /// <summary>
        /// Updates the collection of cells of x terrain type to display bioluminescence.
        /// </summary>
        public override void Regenerate()
        {
            affectedCells = AffectedCells();

            BioluminescenceTextures.Clear();
            BioluminescenceTextures.Add(BioTexA);
            BioluminescenceTextures.Add(BioTexB);
            BioluminescenceTextures.Add(BioTexC);
            BioluminescenceTextures.Add(BioTexD);

            Log.Message("[<color=#4494E3FF>AlienBiomes</color>] <color=#e36c45FF>BioluminescenceTextures contains:</color> "
                + BioluminescenceTextures.Count.ToString().Colorize(Color.green)
                + " <color=#e36c45FF>textures.</color>");

            BioluminescenceMat = GetRandomBioluminescenceMaterial();
        }

        /// <summary>
        /// Draws the bioluminescence.
        /// </summary>
        public override void DrawLayer()
        {
            // Draw the bioluminescent mesh on affected cells
            foreach (IntVec3 cell in affectedCells)
            {
                Vector3 center = new (cell.x + 0.5f, AltitudeLayer.Terrain.AltitudeFor(), cell.z + 0.5f);
                //Graphics.DrawMesh(MeshPool.plane10, center, Quaternion.identity, Mat, 0);

                propertyBlock.SetTexture("_DistortionTex", BioluminescenceDistortionTex);
                propertyBlock.SetFloat("_distortionScrollSpeed", 0.09f);
                propertyBlock.SetFloat("_distortionIntensity", 0.125f);
                propertyBlock.SetFloat("_distortionScale", 0.20f);

                Graphics.DrawMesh(MeshPool.plane10, center, Quaternion.identity, BioluminescenceMat, 0, null, 0, propertyBlock);
            }
        }

        /// <summary>
        /// Grabs all the cells of x terrain type to be used for displaying bioluminescence.
        /// </summary>
        private IEnumerable<IntVec3> AffectedCells()
        {
            List<IntVec3> affectedCells = new ();
            
            foreach (IntVec3 cell in section.CellRect)
            {
                TerrainDef terrain = cell.GetTerrain(Map);
                if (terrain == AlienBiomes_TerrainDefOf.SZ_RadiantWaterOceanShallow)
                {
                    bool isNearOtherTerrain = false;

                    for (int x = -bioReach; x <= bioReach; x++)
                    {
                        for (int z = -bioReach; z <= bioReach; z++)
                        {
                            IntVec3 neighborCell = cell + new IntVec3(x, 0, z);
                            if (neighborCell.InBounds(Map) && neighborCell != cell)
                            {
                                TerrainDef neighborTerrain = neighborCell.GetTerrain(Map);
                                if (neighborTerrain == AlienBiomes_TerrainDefOf.SZ_SoothingSand)
                                {
                                    isNearOtherTerrain = true;
                                    break;
                                }
                            }
                        }

                        if (isNearOtherTerrain)
                            break;
                    }

                    float noiseThreshold = 0.8f;
                    float noiseValue = Random.value;

                    if (isNearOtherTerrain && noiseValue <= noiseThreshold)
                    {
                        affectedCells.Add(cell);
                    }
                }
            }
            return affectedCells;
        }
    }
}
