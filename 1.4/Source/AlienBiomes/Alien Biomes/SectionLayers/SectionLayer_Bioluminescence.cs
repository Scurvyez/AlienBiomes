using System.Collections.Generic;
using Verse;
using RimWorld;
using UnityEngine;

namespace AlienBiomes
{
    [StaticConstructorOnStartup]
    public class SectionLayer_Bioluminescence : SectionLayer
    {
        private static readonly Texture2D BioDistTex = ContentFinder<Texture2D>.Get("Things/Mote/SmokeTiled", true);
        private static readonly Texture2D BioTexA = ContentFinder<Texture2D>.Get("Things/Special/ShallowOceanWaterBioluminescence/ShallowOceanWaterBioluminescenceA", true);
        private static readonly Texture2D BioTexB = ContentFinder<Texture2D>.Get("Things/Special/ShallowOceanWaterBioluminescence/ShallowOceanWaterBioluminescenceB", true);
        private static readonly Texture2D BioTexC = ContentFinder<Texture2D>.Get("Things/Special/ShallowOceanWaterBioluminescence/ShallowOceanWaterBioluminescenceC", true);
        private static readonly Texture2D BioTexD = ContentFinder<Texture2D>.Get("Things/Special/ShallowOceanWaterBioluminescence/ShallowOceanWaterBioluminescenceD", true);
        private static readonly List<Texture2D> BioluminescenceTextures = new ();

        private MaterialPropertyBlock propertyBlock;
        private IEnumerable<IntVec3> affectedCells;
        private readonly int bioReach = 2; // how many cells out, from the shoreline, the bioluminescence displays
        private const int numberOfTextures = 4; // number of available textures to randomly pick for each cell

        // Dictionary to store pre-generated materials based on shoreline cell and texture combination
        private Dictionary<(IntVec3, Texture2D), Material> bioluminescenceMaterials = new ();

        public SectionLayer_Bioluminescence(Section section) : base(section)
        {
            propertyBlock = new MaterialPropertyBlock();
        }

        private Material GetBioluminescenceMaterial(IntVec3 cell, Texture2D texture)
        {
            // Check if the material already exists in the dictionary
            if (bioluminescenceMaterials.TryGetValue((cell, texture), out Material material))
            {
                return material;
            }

            // Create a new material and store it in the dictionary for future use
            material = new Material(ShaderDatabase.MoteGlowDistorted);
            material.SetTexture("_MainTex", texture);
            bioluminescenceMaterials[(cell, texture)] = material;

            return material;
        }

        public override void Regenerate()
        {
            affectedCells = AffectedCells();

            // Generate the list of available bioluminescence textures
            BioluminescenceTextures.Clear();
            BioluminescenceTextures.Add(BioTexA);
            BioluminescenceTextures.Add(BioTexB);
            BioluminescenceTextures.Add(BioTexC);
            BioluminescenceTextures.Add(BioTexD);
        }

        public override void DrawLayer()
        {
            // Draw the bioluminescent mesh on affected cells
            foreach (IntVec3 cell in affectedCells)
            {
                Texture2D randomTexture = BioluminescenceTextures[Mathf.Abs(cell.GetHashCode()) % numberOfTextures];
                Material bioluminescenceMaterial = GetBioluminescenceMaterial(cell, randomTexture);
                Vector3 center = new (cell.x + 0.5f, AltitudeLayer.Terrain.AltitudeFor(), cell.z + 0.5f);

                propertyBlock.SetTexture("_DistortionTex", BioDistTex);
                propertyBlock.SetFloat("_distortionScrollSpeed", 0.09f);
                propertyBlock.SetFloat("_distortionIntensity", 0.125f);
                propertyBlock.SetFloat("_distortionScale", 0.20f);

                Graphics.DrawMesh(MeshPool.plane10, center, Quaternion.identity, bioluminescenceMaterial, 0, null, 0, propertyBlock);
            }
        }

        private IEnumerable<IntVec3> AffectedCells()
        {
            HashSet<IntVec3> affectedCells = new HashSet<IntVec3>();
            BiomeDef radiantPlainsBiome = DefDatabase<BiomeDef>.GetNamed("SZ_RadiantPlains");
            TerrainGrid terrainGrid = Map.terrainGrid;

            if (Map.Biome == radiantPlainsBiome)
            {
                float noiseThreshold1 = 0.75f;

                foreach (IntVec3 cell in section.CellRect)
                {
                    TerrainDef terrain = terrainGrid.TerrainAt(cell);
                    if (terrain == AlienBiomes_TerrainDefOf.SZ_RadiantWaterOceanShallow)
                    {
                        float noiseValue1 = Mathf.PerlinNoise(cell.x * 0.1f, cell.z * 0.1f);
                        if (noiseValue1 <= noiseThreshold1)
                        {
                            bool isNearShoreline = false;

                            for (int x = -bioReach; x <= bioReach; x++)
                            {
                                for (int z = -bioReach; z <= bioReach; z++)
                                {
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

                                if (isNearShoreline)
                                    break;
                            }

                            float noiseThreshold2 = 0.8f;
                            float noiseValue2 = Random.value;

                            if (isNearShoreline && noiseValue2 <= noiseThreshold2)
                            {
                                affectedCells.Add(cell);
                            }
                        }
                    }
                }
            }
            return affectedCells;
        }
    }
}