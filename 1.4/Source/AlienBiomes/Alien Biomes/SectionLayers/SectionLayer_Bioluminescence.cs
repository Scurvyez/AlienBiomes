using System.Collections.Generic;
using Verse;
using RimWorld;
using UnityEngine;
using System.Diagnostics;
using System.Linq;

namespace AlienBiomes
{
    [StaticConstructorOnStartup]
    public class SectionLayer_Bioluminescence : SectionLayer
    {
        private static readonly Texture2D BioDistTex = ContentFinder<Texture2D>.Get("Things/Mote/SmokeTiled", true);
        private static readonly Texture2D BioTexA = ContentFinder<Texture2D>.Get("Things/Special/ShallowOceanWaterBioluminescence/ShallowOceanWaterBioluminescenceA", true);

        private Material bioluminescenceMaterial; // bioluminescent material (contains our Texture2D), what we see in each cell
        private MaterialPropertyBlock propertyBlock; // shader property access
        private HashSet<IntVec3> affectedCells;  // first pass of cells to have bioluminescence
        private HashSet<IntVec3> newAffectedCells; // all other passes
        private readonly int bioReach = 1; // how many cells out, from the shoreline, the bioluminescence displays
        public SectionLayer_Bioluminescence(Section section) : base(section)
        {
            propertyBlock = new MaterialPropertyBlock();
            relevantChangeTypes = MapMeshFlag.Terrain;

            // Create the bioluminescent material with BioTexA as the texture
            bioluminescenceMaterial = new Material(ShaderDatabase.MoteGlowDistorted);
            bioluminescenceMaterial.SetTexture("_MainTex", BioTexA);
        }

        public override void Regenerate()
        {
            if (affectedCells == null)
            {
                // Initial run, populate affectedCells for the first time
                affectedCells = AffectedCells();
            }

            newAffectedCells = AffectedCells();

            if (!affectedCells.SetEquals(newAffectedCells))
            {
                // The affected cells have changed, update the affectedCells HashSet
                affectedCells = newAffectedCells;
            }
        }

        public override void DrawLayer()
        {
            // Check if affectedCells is empty, return early if true
            if (affectedCells.Count == 0)
            {
                return;
            }

            propertyBlock.SetTexture("_DistortionTex", BioDistTex);
            propertyBlock.SetFloat("_distortionScrollSpeed", 0.09f);
            propertyBlock.SetFloat("_distortionIntensity", 0.125f);
            propertyBlock.SetFloat("_distortionScale", 0.20f);

            // Calculate altitude once before the loop
            float terrainAltitude = AltitudeLayer.Terrain.AltitudeFor();

            // Iterate over each affected cell within the 8x8 area
            foreach (IntVec3 cell in affectedCells)
            {
                // Calculate the position of the current cell within the 8x8 area
                Vector3 position = new (cell.x + 0.5f, terrainAltitude, cell.z + 0.5f);

                // Draw the bioluminescent mesh at the position of the current cell
                Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, bioluminescenceMaterial, 0, null, 0, propertyBlock);
            }
        }

        private HashSet<IntVec3> AffectedCells()
        {
            HashSet<IntVec3> cells = new ();
            BiomeDef radiantPlainsBiome = DefDatabase<BiomeDef>.GetNamed("SZ_RadiantPlains");
            TerrainGrid terrainGrid = Map.terrainGrid;

            float chunkNoiseThreshold = 0.875f;
            float scatterNoiseThreshold = 0.875f;

            if (Map.Biome == radiantPlainsBiome)
            {
                foreach (IntVec3 cell in section.CellRect)
                {
                    TerrainDef terrain = terrainGrid.TerrainAt(cell);
                    if (terrain == AlienBiomes_TerrainDefOf.SZ_RadiantWaterOceanShallow)
                    {
                        float chunkNoise = Mathf.PerlinNoise(cell.x * 0.1f, cell.z * 0.1f);
                        if (chunkNoise <= chunkNoiseThreshold)
                        {
                            bool isNearShoreline = false;

                            int cellRange = 2 * bioReach + 1; // Calculate the range of cells in each dimension
                            int cellCount = cellRange * cellRange; // Total number of cells in the range

                            for (int i = 0; i < cellCount; i++)
                            {
                                int x = i % cellRange - bioReach; // Calculate the x coordinate based on the linear index
                                int z = i / cellRange - bioReach; // Calculate the z coordinate based on the linear index

                                IntVec3 neighborCell = cell + new IntVec3(x, 0, z);
                                if (neighborCell.InBounds(Map) && neighborCell != cell)
                                {
                                    TerrainDef neighborTerrain = terrainGrid.TerrainAt(neighborCell);
                                    if (neighborTerrain == AlienBiomes_TerrainDefOf.SZ_SoothingSand)
                                    {
                                        isNearShoreline = true;
                                        break;
                                    }
                                }
                            }

                            float scatterNoise = Random.value;
                            if (isNearShoreline && scatterNoise <= scatterNoiseThreshold)
                            {
                                cells.Add(cell);
                            }
                        }
                    }
                }
            }
            return cells;
        }
    }
}
