using System.Collections.Generic;
using Verse;
using RimWorld;
using UnityEngine;

namespace AlienBiomes
{
    [StaticConstructorOnStartup]
    public class SectionLayer_Bioluminescence : SectionLayer
    {
        private Material bioluminescenceMaterial;
        private MaterialPropertyBlock propertyBlock;
        private HashSet<IntVec3> affectedCells;  // first pass of cells to have bioluminescence
        private HashSet<IntVec3> newAffectedCells; // all other passes
        private List<Color> randomColors;
        private float terrainAltitude;
        private Mesh mesh;
        private Bioluminescence_ModExtension bioluminescenceExt;

        public SectionLayer_Bioluminescence(Section section) : base(section)
        {
            bioluminescenceExt = Map.Biome.GetModExtension<Bioluminescence_ModExtension>();
            terrainAltitude = AltitudeLayer.Terrain.AltitudeFor();
            mesh = MeshPool.plane10;
            propertyBlock = new MaterialPropertyBlock();
            relevantChangeTypes = MapMeshFlag.Terrain;
            bioluminescenceMaterial = new Material(ShaderDatabase.MoteGlowDistorted);
            bioluminescenceMaterial.SetTexture("_MainTex", TextureAssets.BioTexA);

            randomColors = GenerateRandomColors(9);
        }

        public override void Regenerate()
        {
            if (affectedCells == null)
            {
                // Initial run, populate affectedCells for the first time
                affectedCells = AffectedCells();

                // Assign random colors to affected cells' materials
                foreach (IntVec3 cell in affectedCells)
                {
                    int randomIndex = Random.Range(0, randomColors.Count);
                    Color randomColor = randomColors[randomIndex];

                    // Set the color in the propertyBlock for the cell's material
                    propertyBlock.SetColor("_Color", randomColor);
                }
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

            propertyBlock.SetTexture("_DistortionTex", TextureAssets.BioDistTex);
            propertyBlock.SetFloat("_distortionScrollSpeed", 0.09f);
            propertyBlock.SetFloat("_distortionIntensity", 0.125f);
            propertyBlock.SetFloat("_distortionScale", 0.20f);

            // Iterate over each affected cell within the 8x8 area
            foreach (IntVec3 cell in affectedCells)
            {
                // Calculate the position of the current cell within the 8x8 area
                Vector3 position = new (cell.x + 0.5f, terrainAltitude, cell.z + 0.5f);

                // Draw the bioluminescent mesh at the position of the current cell
                Graphics.DrawMesh(mesh, position, Quaternion.identity, bioluminescenceMaterial, 0, null, 0, propertyBlock);
            }
        }

        private List<Color> GenerateRandomColors(int count)
        {
            List<Color> colors = new ();
            for (int i = 0; i < count; i++)
            {
                float r = Random.Range(0f, 0.1f);
                float g = Random.Range(0.8f, 1f);
                float b = Random.Range(0.5f, 0.7f);
                float a = Random.Range(0.2f, 0.9f);
                Color randomColor = new (r, g, b, a);
                colors.Add(randomColor);
            }
            return colors;
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
                    if (terrain == ABDefOf.SZ_RadiantWaterOceanShallow)
                    {
                        float chunkNoise = Mathf.PerlinNoise(cell.x * 0.1f, cell.z * 0.1f);
                        if (chunkNoise <= chunkNoiseThreshold)
                        {
                            bool isNearShoreline = false;

                            int cellRange = 2 * bioluminescenceExt.reachFromShore + 1; // Calculate the range of cells in each dimension
                            int cellCount = cellRange * cellRange; // Total number of cells in the range

                            for (int i = 0; i < cellCount; i++)
                            {
                                int x = i % cellRange - bioluminescenceExt.reachFromShore; // Calculate the x coordinate based on the linear index
                                int z = i / cellRange - bioluminescenceExt.reachFromShore; // Calculate the z coordinate based on the linear index

                                IntVec3 neighborCell = cell + new IntVec3(x, 0, z);
                                if (neighborCell.InBounds(Map) && neighborCell != cell)
                                {
                                    TerrainDef neighborTerrain = terrainGrid.TerrainAt(neighborCell);
                                    if (neighborTerrain == ABDefOf.SZ_SoothingSand)
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
