using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;
using Verse.Noise;

namespace AlienBiomes
{
    /// <summary>
    /// Generates a single oasis patch on the map.
    /// Make sure to use the correct order in the map gen process to avoid things like
    /// generating steam geysers in the middle of the oasis dumbass. lol
    /// </summary>
    public class GenStep_DeliriousDunesOasis : GenStep
    {
        public TerrainDef spawnOnTerDef = null;
        public TerrainDef waterDef = null;
        public TerrainDef sandDef = null;
        public List<ThingDef> plantsToGen = [];
        public float plantGenChance = 1f;
        public float plantGenRadius = 10f;
        public List<IntRange> waterRadius = [];
        public IntRange sandRadius;
        public List<IntRange> mapSizeRadiusAdjust = [];

        private Perlin _perlin;
        private readonly HashSet<IntVec3> _waterCells = [];
        
        public override int SeedPart => 931457342;
        
        public override void Generate(Map map, GenStepParams parms)
        {
            _waterCells.Clear();
            _perlin = new Perlin(0.1, 2.0, 0.5,
                6, map.Tile, QualityMode.Medium);
            
            if (map.Biome != InternalDefOf.SZ_DeliriousDunes) return;
            IntVec3 center = ValidCentralSpawnCell(map);

            if (!center.IsValid) return;
            int mapAdjustmentFactor = AdjustedRadiusByMapSize(map, 
                mapSizeRadiusAdjust).RandomInRange;
            int waterRadiusFinal = AdjustedRadiusByMapSize(map, 
                waterRadius).RandomInRange + mapAdjustmentFactor;
            
            GenerateWaterPatch(map, center, waterRadiusFinal);
            GenerateSandAroundWater(map);
            GenerateThings(map, center);
        }
        
        private static IntRange AdjustedRadiusByMapSize(Map map, List<IntRange> range)
        {
            int mapSizeIndex = Mathf.FloorToInt(map.Size.x / 100.0f);
            return mapSizeIndex < range.Count 
                ? range[mapSizeIndex] 
                : new IntRange(1, 1);
        }
        
        private void GenerateWaterPatch(Map map, IntVec3 center, int radius)
        {
            foreach (IntVec3 cell in GenRadial
                         .RadialCellsAround(center, radius, true))
            {
                if (!cell.InBounds(map) || cell.GetTerrain(map) != spawnOnTerDef) 
                    continue;
                
                float x = (float)cell.x / map.Size.x * 10f;
                float z = (float)cell.z / map.Size.z * 10f;
                double noiseValue = _perlin.GetValue(x, 0, z);
                float distFactor = 1f - (cell.DistanceTo(center) / radius);
                double threshold = -0.5 + (0.5 * distFactor); // central cells are more likely to be water
                
                if (!(noiseValue > threshold)) continue;
                map.terrainGrid.SetTerrain(cell, waterDef);
                _waterCells.Add(cell);
            }
        }
        
        private void GenerateSandAroundWater(Map map)
        {
            foreach (IntVec3 waterCell in _waterCells)
            {
                int sandRadiusFinal = sandRadius.RandomInRange;
                foreach (IntVec3 cell in GenRadial.RadialCellsAround(waterCell, sandRadiusFinal, true))
                {
                    if (!cell.InBounds(map) || cell.GetTerrain(map) == waterDef) 
                        continue;
                    
                    float x = (float)cell.x / map.Size.x * 10f;
                    float z = (float)cell.z / map.Size.z * 10f;
                    double noiseValue = _perlin.GetValue(x, 0, z);
                    float distFactor = 1f - (cell.DistanceTo(waterCell) / sandRadiusFinal);
                    double threshold = 0.3 * distFactor; // adjust for smoother blending

                    if (noiseValue > threshold)
                    {
                        map.terrainGrid.SetTerrain(cell, sandDef);
                    }
                }
            }
        }
        
        private void GenerateThings(Map map, IntVec3 center)
        {
            HashSet<IntVec3> usedCells = [];
            foreach (IntVec3 cell in GenRadial
                         .RadialCellsAround(center, plantGenRadius, true))
            {
                if (cell.GetTerrain(map) != sandDef 
                    || !cell.Standable(map) || usedCells.Contains(cell)) continue;
                
                if (plantsToGen == null || !Rand.Chance(plantGenChance)) continue;
                ThingDef plantDef = plantsToGen.RandomElement();
                Plant plant = (Plant)ThingMaker.MakeThing(plantDef);
                plant.Growth = Rand.Value;
                GenSpawn.Spawn(plant, cell, map);
                usedCells.Add(cell);
            }
        }
        
        private IntVec3 ValidCentralSpawnCell(Map map)
        {
            IntVec3 mapCenter = map.Center;
            int maxDistFromCenter = Mathf
                .RoundToInt(Mathf
                    .Sqrt(map.Size.x * map.Size.x + map.Size.z * map.Size.z) / 5f);

            return CellFinderLoose.RandomCellWith(cell =>
                cell.InBounds(map) &&
                cell.DistanceTo(mapCenter) <= maxDistFromCenter &&
                cell.Standable(map) &&
                !cell.Fogged(map) &&
                cell.GetTerrain(map) == spawnOnTerDef, map);
        }
    }
}