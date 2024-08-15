using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;

namespace AlienBiomes
{
    /// <summary>
    /// Generates a single oasis patch on the map.
    /// Make sure to use the correct order in the map gen process to avoid things like
    /// generating steam geysers in the middle of the oasis dumbass. lol
    /// </summary>
    public class GenStep_DeliriousDunesOasis : GenStep
    {
        public override int SeedPart => 931457342;
        public TerrainDef spawnOnTerDef = null;
        public TerrainDef waterDef = null;
        public TerrainDef sandDef = null;
        public List<ThingDef> plantsToGen = new();
        public float plantGenChance = 1f;
        public float plantGenRadius = 10f;
        public List<IntRange> waterRadius = new();
        public IntRange sandRadius = new();
        public List<IntRange> mapSizeRadiusAdjust = new();

        private HashSet<IntVec3> waterCells = new HashSet<IntVec3>();

        public override void Generate(Map map, GenStepParams parms)
        {
            waterCells.Clear();
            if (map.Biome != ABDefOf.SZ_DeliriousDunes) return;
            IntVec3 center = ValidCentralSpawnCell(map);

            if (!center.IsValid) return;
            int mapAdjustmentFactor = AdjustedRadiusByMapSize(map, mapSizeRadiusAdjust).RandomInRange;
            int waterRadiusFinal = AdjustedRadiusByMapSize(map, waterRadius).RandomInRange + mapAdjustmentFactor;
            GenerateWaterPatch(map, center, waterRadiusFinal);
            GenerateSandAroundWater(map);
            GenerateThings(map, center);
        }
        
        private IntRange AdjustedRadiusByMapSize(Map map, List<IntRange> range)
        {
            int mapSizeIndex = Mathf.FloorToInt(map.Size.x / 100.0f);
            return mapSizeIndex < range.Count ? range[mapSizeIndex] : new IntRange(1, 1);
        }

        private void GenerateWaterPatch(Map map, IntVec3 center, int radius)
        {
            foreach (IntVec3 cell in GenRadial.RadialCellsAround(center, radius, true))
            {
                if (cell.GetTerrain(map) != spawnOnTerDef) continue;
                map.terrainGrid.SetTerrain(cell, waterDef);
                waterCells.Add(cell);
            }
        }

        private void GenerateSandAroundWater(Map map)
        {
            foreach (IntVec3 waterCell in waterCells)
            {
                int sandRadiusFinal = sandRadius.RandomInRange;
                foreach (IntVec3 cell in GenRadial.RadialCellsAround(waterCell, sandRadiusFinal, true))
                {
                    if (!cell.InBounds(map) || cell.GetTerrain(map) == waterDef) continue;
                    map.terrainGrid.SetTerrain(cell, sandDef);
                }
            }
        }

        private void GenerateThings(Map map, IntVec3 center)
        {
            HashSet<IntVec3> usedCells = new HashSet<IntVec3>();
            foreach (IntVec3 cell in GenRadial.RadialCellsAround(center, plantGenRadius, true))
            {
                if (cell.GetTerrain(map) != sandDef || !cell.Standable(map) || usedCells.Contains(cell)) continue;
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
            int maxDistFromCenter = Mathf.RoundToInt(Mathf.Sqrt(map.Size.x * map.Size.x + map.Size.z * map.Size.z) / 5f);

            return CellFinderLoose.RandomCellWith((IntVec3 cell) =>
                cell.InBounds(map) &&
                cell.DistanceTo(mapCenter) <= maxDistFromCenter &&
                cell.Standable(map) &&
                !cell.Fogged(map) &&
                cell.GetTerrain(map) == spawnOnTerDef, map, 1000);
        }
    }
}
