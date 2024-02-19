using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    /*
    [StaticConstructorOnStartup]
    public class MapComponent_Bioluminescence : MapComponent
    {
        private Bioluminescence_ModExtension bioluminescenceExt;
        private float terrainAltitude;
        private Mesh mesh;
        private Vector3 position;

        private HashSet<IntVec3> affectedCells = new ();  // first pass of cells to have bioluminescence
        private HashSet<IntVec3> newAffectedCells = new (); // all other passes

        private Material bioluminescenceMaterial;
        private MaterialPropertyBlock propertyBlock;
        private List<Color> randomColors;

        public MapComponent_Bioluminescence(Map map) : base(map) { }

        public override void FinalizeInit()
        {
            base.FinalizeInit();

            bioluminescenceExt = map.Biome.GetModExtension<Bioluminescence_ModExtension>();
            terrainAltitude = AltitudeLayer.Terrain.AltitudeFor();
            mesh = MeshPool.plane10;
            propertyBlock = new MaterialPropertyBlock();
            bioluminescenceMaterial = new Material(ShaderDatabase.MoteGlowDistorted);
            bioluminescenceMaterial.SetTexture("_MainTex", TextureAssets.BioTexA);
            propertyBlock.SetTexture("_DistortionTex", TextureAssets.BioDistTex);
            propertyBlock.SetFloat("_distortionScrollSpeed", 0.09f);
            propertyBlock.SetFloat("_distortionIntensity", 0.125f);
            propertyBlock.SetFloat("_distortionScale", 0.20f);
            randomColors = GenerateRandomColors(16);

            Log.Message($"affectedCells: {affectedCells}");
            Log.Message($"newAffectedCells: {newAffectedCells}");

            Generate();
        }

        public override void MapComponentTick()
        {
            base.MapComponentTick();

            foreach (IntVec3 cell in affectedCells)
            {
                // Calculate the position of the current cell within the 8x8 area
                position = new(cell.x + 0.5f, terrainAltitude, cell.z + 0.5f);
            }
        }

        public override void MapComponentUpdate()
        {
            base.MapComponentUpdate();

            // Check if affectedCells is empty, return early if true
            if (affectedCells.Count == 0)
            {
                return;
            }

            // Iterate over each affected cell within the 8x8 area
            foreach (IntVec3 cell in affectedCells)
            {
                // Draw the bioluminescent mesh at the position of the current cell
                Graphics.DrawMesh(mesh, position, Quaternion.identity, bioluminescenceMaterial, 0, null, 0, propertyBlock);
            }
        }

        private List<Color> GenerateRandomColors(int count)
        {
            List<Color> colors = new();
            for (int i = 0; i < count; i++)
            {
                float r = Random.Range(0f, 0.1f);
                float g = Random.Range(0.8f, 1f);
                float b = Random.Range(0.5f, 0.7f);
                float a = Random.Range(0.2f, 0.9f);
                Color randomColor = new(r, g, b, a);
                colors.Add(randomColor);
            }
            return colors;
        }

        private void Generate()
        {
            // Populate newAffectedCells regardless of biome
            newAffectedCells = AffectedCells();

            // Check if newAffectedCells is empty
            if (newAffectedCells.Count > 0 && !affectedCells.SetEquals(newAffectedCells))
            {
                // Update affectedCells
                affectedCells = new HashSet<IntVec3>(newAffectedCells);
            }
        }

        private HashSet<IntVec3> AffectedCells()
        {
            HashSet<IntVec3> cells = new();
            BiomeDef radiantPlainsBiome = ABDefOf.SZ_RadiantPlains;
            TerrainGrid terrainGrid = map.terrainGrid;

            float chunkNoiseThreshold = 0.875f;
            float scatterNoiseThreshold = 0.875f;

            if (map.Biome == radiantPlainsBiome)
            {
                foreach (IntVec3 cell in map.AllCells)
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
                                if (neighborCell.InBounds(map) && neighborCell != cell)
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
    */
}
