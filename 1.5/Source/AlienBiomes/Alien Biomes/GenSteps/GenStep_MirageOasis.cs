using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;
using Verse.Noise;

namespace AlienBiomes
{
    /// <summary>
    /// Generates a single little oasis patch on the map.
    /// Make sure to use the correct order in the map gen process to avoid things like
    /// generating steam geysers in the middle of the oasis. lol
    /// </summary>
    public class GenStep_DeliriousDunesOasis : GenStep
    {
        public override int SeedPart => 931457342;
        public TerrainDef spawnOnTerDef = null;
        public TerrainDef outerWaterDef = null;
        public TerrainDef outerSandDef = null;
        public List<ThingDef> plantsToGen = new();
        public float plantGenChance = 1f;
        public float plantGenRadius = 10f;
        public List<IntRange> waterRadiusOne = new();
        public List<IntRange> waterRadiusTwo = new();
        public IntRange outerSandRadius = new();
        public List<IntRange> mapSizeRadiusAdjust = new();

        private HashSet<IntVec3> waterCells = new HashSet<IntVec3>();

        public override void Generate(Map map, GenStepParams parms)
        {
            if (map.Biome == ABDefOf.SZ_DeliriousDunes)
            {
                IntVec3 center1 = ValidCentralSpawnCell(map);
                if (center1.IsValid)
                {
                    int mapAdjustmentFactor = AdjustedRadiusByMapSize(map, mapSizeRadiusAdjust).RandomInRange;
                    int waterRadiusOneFinal = AdjustedRadiusByMapSize(map, waterRadiusOne).RandomInRange + mapAdjustmentFactor;
                    int waterRadiusTwoFinal = AdjustedRadiusByMapSize(map, waterRadiusTwo).RandomInRange + mapAdjustmentFactor;

                    Log.Message($"Map adjustment factor: <color=#ff8c66>{mapAdjustmentFactor}</color>");
                    Log.Message($"Final water 1 radius: <color=#ff8c66>{waterRadiusOneFinal}</color>");
                    Log.Message($"Final water 2 radius: <color=#ff8c66>{waterRadiusTwoFinal}</color>");

                    IntVec3 center2 = FindSecondPatchCenter(map, center1, waterRadiusOneFinal);
                    if (center2.IsValid)
                    {
                        GeneratePatch(map, center1, waterRadiusOneFinal);
                        //GeneratePatch(map, center2, waterRadiusTwoFinal);
                        GenerateThings(map, center1);
                        //GenerateThings(map, center2);

                        Log.Message($"Total oasis water cells: <color=#ff8c66>{waterCells.Count}</color>");
                    }
                }
            }
        }
        
        private IntRange AdjustedRadiusByMapSize(Map map, List<IntRange> range)
        {
            int mapSizeIndex = Mathf.FloorToInt(map.Size.x / 100);
            return mapSizeIndex < range.Count ? range[mapSizeIndex] : new IntRange(1, 1);
        }

        private void GeneratePatch(Map map, IntVec3 center, int waterRadius)
        {
            // Generate water terrain (our first pass for the entire oasis)
            foreach (IntVec3 cell in GenRadial.RadialCellsAround(center, waterRadius, true))
            {
                if (cell.GetTerrain(map) == spawnOnTerDef)
                {
                    map.terrainGrid.SetTerrain(cell, outerWaterDef);
                    waterCells.Add(cell);
                }
            }

            // Generate sand terrain around the waters' edge
            foreach (IntVec3 waterCell in waterCells)
            {
                int outerSandRadiusFinal = outerSandRadius.RandomInRange;
                Log.Message($"Final sand radius for {waterCell}: <color=#ff8c66>{outerSandRadiusFinal}</color>");

                foreach (IntVec3 cell in GenRadial.RadialCellsAround(center, outerSandRadiusFinal, true))
                {
                    if (cell.InBounds(map) && cell.GetTerrain(map) != outerWaterDef)
                    {
                        map.terrainGrid.SetTerrain(cell, outerSandDef);
                    }
                }
            }
        }

        private void GenerateThings(Map map, IntVec3 center)
        {
            HashSet<IntVec3> usedCells = new HashSet<IntVec3>();

            foreach (IntVec3 cell in GenRadial.RadialCellsAround(center, plantGenRadius, true))
            {
                if (cell.GetTerrain(map) == outerSandDef && cell.Standable(map) && !usedCells.Contains(cell))
                {
                    if (plantsToGen != null && Rand.Chance(plantGenChance))
                    {
                        ThingDef plantDef = plantsToGen.RandomElement();
                        Plant plant = (Plant)ThingMaker.MakeThing(plantDef);
                        plant.Growth = Rand.Value;
                        GenSpawn.Spawn(plant, cell, map);

                        // Mark the cell as used
                        usedCells.Add(cell);
                    }
                }
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

        /// <summary>
        /// Try to find a second patch center within the water radius of the first patch center.
        /// </summary>
        private IntVec3 FindSecondPatchCenter(Map map, IntVec3 firstPatchCenter, int waterRadius)
        {
            IntVec3 center = IntVec3.Invalid;

            for (int i = 0; i < 10; i++)
            {
                IntVec3 candidate = CellFinderLoose.RandomCellWith(cell =>
                    cell.InBounds(map) &&
                    cell.DistanceTo(firstPatchCenter) <= waterRadius &&
                    cell.Standable(map) &&
                    !cell.Fogged(map), map, 100);

                if (candidate.IsValid)
                {
                    center = candidate;
                    break;
                }
            }
            return center;
        }
    }
}
