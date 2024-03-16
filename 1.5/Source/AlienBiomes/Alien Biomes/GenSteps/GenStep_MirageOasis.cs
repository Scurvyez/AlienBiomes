using System.Collections.Generic;
using UnityEngine;
using Verse;

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
        public IntRange? waterRadiusOne;
        public IntRange? waterRadiusTwo;
        public IntRange? outerSandRadius;
        public TerrainDef innerWaterDef = null;
        public TerrainDef outerWaterDef = null;
        public TerrainDef outlayingSandDef = null;

        public override void Generate(Map map, GenStepParams parms)
        {
            if (map.Biome == ABDefOf.SZ_DeliriousDunes)
            {
                IntVec3 center1 = ValidCentralSpawnCell(map);
                if (center1.IsValid)
                {
                    int waterRadiusOneFinal = waterRadiusOne.Value.RandomInRange;
                    int waterRadiusTwoFinal = waterRadiusOne.Value.RandomInRange;

                    IntVec3 center2 = FindSecondPatchCenter(map, center1, waterRadiusOneFinal);
                    if (center2.IsValid)
                    {
                        GeneratePatch(map, center1, waterRadiusOneFinal);
                        GeneratePatch(map, center2, waterRadiusTwoFinal);
                        GenerateInnerWater(map, center1, waterRadiusOneFinal);
                        GenerateInnerWater(map, center2, waterRadiusTwoFinal);
                    }
                }
            }
        }

        private void GeneratePatch(Map map, IntVec3 center, int waterRadius)
        {
            HashSet<IntVec3> waterCells = new HashSet<IntVec3>();

            // Generate water terrain (our first pass for the entire oasis)
            foreach (IntVec3 cell in CellRect.CenteredOn(center, waterRadius))
            {
                if (cell.GetTerrain(map) == ABDefOf.SZ_DeliriousMellowSand)
                {
                    map.terrainGrid.SetTerrain(cell, outerWaterDef);
                    waterCells.Add(cell);
                }
            }

            // Generate sand terrain around the waters' edge
            foreach (IntVec3 waterCell in waterCells)
            {
                int outerSandRadiusFinal = outerSandRadius.Value.RandomInRange;
                foreach (IntVec3 cell in GenRadial.RadialCellsAround(waterCell, outerSandRadiusFinal, true))
                {
                    if (cell.InBounds(map) && cell.GetTerrain(map) != outerWaterDef)
                    {
                        map.terrainGrid.SetTerrain(cell, outlayingSandDef);
                    }
                }
            }
        }

        private void GenerateInnerWater(Map map, IntVec3 center, int waterRadius)
        {
            // Generate deep water within our original water terrain
            foreach (IntVec3 cell in GenRadial.RadialCellsAround(center, 4, true))
            {
                if (cell.GetTerrain(map) == outerWaterDef)
                {
                    map.terrainGrid.SetTerrain(cell, innerWaterDef);
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
                cell.GetTerrain(map) == ABDefOf.SZ_DeliriousMellowSand, map, 1000);
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
