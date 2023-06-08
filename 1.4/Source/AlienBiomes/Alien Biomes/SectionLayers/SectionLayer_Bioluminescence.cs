using System.Collections.Generic;
using Verse;
using RimWorld;
using UnityEngine;
using System.Diagnostics;

namespace AlienBiomes
{
    [StaticConstructorOnStartup]
    public class SectionLayer_Bioluminescence : SectionLayer
    {
        private static readonly Texture2D BioDistTex = ContentFinder<Texture2D>.Get("Things/Mote/SmokeTiled", true);
        private static readonly Texture2D BioTexA = ContentFinder<Texture2D>.Get("Things/Special/ShallowOceanWaterBioluminescence/ShallowOceanWaterBioluminescenceA", true);
        //private static readonly Texture2D BioTexB = ContentFinder<Texture2D>.Get("Things/Special/ShallowOceanWaterBioluminescence/ShallowOceanWaterBioluminescenceB", true);
        //private static readonly Texture2D BioTexC = ContentFinder<Texture2D>.Get("Things/Special/ShallowOceanWaterBioluminescence/ShallowOceanWaterBioluminescenceC", true);
        //private static readonly Texture2D BioTexD = ContentFinder<Texture2D>.Get("Things/Special/ShallowOceanWaterBioluminescence/ShallowOceanWaterBioluminescenceD", true);
        //private static readonly List<Texture2D> BioluminescenceTextures = new ();

        private Material bioluminescenceMaterial; // Single shared material
        private MaterialPropertyBlock propertyBlock;
        private HashSet<IntVec3> affectedCells;
        private HashSet<IntVec3> newAffectedCells;
        private readonly int bioReach = 2; // how many cells out, from the shoreline, the bioluminescence displays
        //private const int numberOfTextures = 4; // number of available textures to randomly pick for each cell

        // Dictionary to store pre-generated materials based on shoreline cell and texture combination
        //private Dictionary<(IntVec3, Texture2D), Material> bioluminescenceMaterials = new ();
        //private List<Material> bioluminescenceMaterials = new (); // Store created materials

        public SectionLayer_Bioluminescence(Section section) : base(section)
        {
            propertyBlock = new MaterialPropertyBlock();
            relevantChangeTypes = MapMeshFlag.Terrain;

            // Create the bioluminescent material with BioTexA as the texture
            bioluminescenceMaterial = new Material(ShaderDatabase.MoteGlowDistorted);
            bioluminescenceMaterial.SetTexture("_MainTex", BioTexA);
        }

        /*
        private Material GetBioluminescenceMaterial(Texture2D texture)
        {
            // Check if the material already exists
            for (int i = 0; i < bioluminescenceMaterials.Count; i++)
            {
                Material material = bioluminescenceMaterials[i];
                if (material.mainTexture == texture)
                {
                    return material;
                }
            }

            // Create a new material and store it for future use
            Material newMaterial = new (ShaderDatabase.MoteGlowDistorted);
            newMaterial.SetTexture("_MainTex", texture);
            bioluminescenceMaterials.Add(newMaterial);

            return newMaterial;
        }
        */

        public override void Regenerate()
        {
            if (affectedCells == null)
            {
                // Initial run, populate affectedCells for the first time
                affectedCells = AffectedCells();

                // Generate the list of available bioluminescence textures
                //BioluminescenceTextures.Clear();
                //BioluminescenceTextures.Add(BioTexA);
                //BioluminescenceTextures.Add(BioTexB);
                //BioluminescenceTextures.Add(BioTexC);
                //BioluminescenceTextures.Add(BioTexD);
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
            // Pre-generate random textures for all affected cells
            //List<IntVec3> cellList = new (newAffectedCells);
            //List<Texture2D> cellTextures = new (newAffectedCells.Count);

            /*
            for (int i = 0; i < cellList.Count; i++)
            {
                IntVec3 cell = cellList[i];
                Texture2D randomTexture = BioluminescenceTextures[Mathf.Abs(cell.GetHashCode()) % numberOfTextures];
                cellTextures.Add(randomTexture);
            }
            */

            propertyBlock.SetTexture("_DistortionTex", BioDistTex);
            propertyBlock.SetFloat("_distortionScrollSpeed", 0.09f);
            propertyBlock.SetFloat("_distortionIntensity", 0.125f);
            propertyBlock.SetFloat("_distortionScale", 0.20f);

            // Calculate altitude once before the loop
            float terrainAltitude = AltitudeLayer.Terrain.AltitudeFor();

            // Draw the bioluminescent mesh on affected cells
            /*
            for (int i = 0; i < cellList.Count; i++)
            {
                IntVec3 cell = cellList[i];
                Texture2D randomTexture = cellTextures[i];
                Material bioluminescenceMaterial = GetBioluminescenceMaterial(randomTexture);
                Vector3 center = new (cell.x + 0.5f, terrainAltitude, cell.z + 0.5f);

                Graphics.DrawMesh(MeshPool.plane10, center, Quaternion.identity, bioluminescenceMaterial, 0, null, 0, propertyBlock);
            }
            */

            Stopwatch loopTimer = Stopwatch.StartNew();

            // Draw the bioluminescent mesh on affected cells
            foreach (IntVec3 cell in newAffectedCells)
            {
                IntVec3 baseCell = cell - new IntVec3(cell.x % 8, 0, cell.z % 8); // Get the base cell of the 8x8 area

                // Draw the bioluminescent mesh for the entire 8x8 area
                for (int xOffset = 0; xOffset < 8; xOffset++)
                {
                    for (int zOffset = 0; zOffset < 8; zOffset++)
                    {
                        IntVec3 currentCell = baseCell + new IntVec3(xOffset, 0, zOffset);
                        Vector3 center = new (currentCell.x + 0.5f, terrainAltitude, currentCell.z + 0.5f);

                        Graphics.DrawMesh(MeshPool.plane10, center, Quaternion.identity, bioluminescenceMaterial, 0, null, 0, propertyBlock);
                    }
                }
            }

            loopTimer.Stop();
            float loopTime = loopTimer.ElapsedTicks;
            Log.Message($"Bioluminescence Draw Time: {loopTime:F0}ms");
        }

        private HashSet<IntVec3> AffectedCells()
        {
            HashSet<IntVec3> cells = new ();
            BiomeDef radiantPlainsBiome = DefDatabase<BiomeDef>.GetNamed("SZ_RadiantPlains");
            TerrainGrid terrainGrid = Map.terrainGrid;

            float chunkNoiseThreshold = 0.5f;
            float scatterNoiseThreshold = 0.8f;

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