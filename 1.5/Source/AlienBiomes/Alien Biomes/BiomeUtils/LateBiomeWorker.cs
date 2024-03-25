using RimWorld.Planet;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse.Noise;
using Verse;

namespace AlienBiomes
{
    public class LateBiomeWorker : WorldGenStep
    {
        private static ModuleBase PerlinNoise;
        private List<BiomeDef> firstOrderBiomes = new List<BiomeDef>();
        private List<BiomeDef> secondOrderBiomes = new List<BiomeDef>();
        private List<BiomeDef> thirdOrderBiomes = new List<BiomeDef>();

        public override int SeedPart => 123456789;

        public override void GenerateFresh(string seed)
        {
            BiomesKit();
        }

        private void BiomesKit()
        {
            foreach (BiomeDef item in DefDatabase<BiomeDef>.AllDefsListForReading.Where((BiomeDef x) => x.HasModExtension<BiomeControls>()))
            {
                switch (item.GetModExtension<BiomeControls>().biomePriority)
                {
                    default:
                        Log.Message("adding " + item.defName + "to primary biomes");
                        firstOrderBiomes.Add(item);
                        break;
                    case 2:
                        Log.Message("adding " + item.defName + "to secondary biomes");
                        secondOrderBiomes.Add(item);
                        break;
                    case 3:
                        Log.Message("adding " + item.defName + "to tertiary biomes");
                        thirdOrderBiomes.Add(item);
                        break;
                }
            }
            foreach (BiomeDef firstOrderBiome in firstOrderBiomes)
            {
                BiomeCalcs(firstOrderBiome);
            }
            foreach (BiomeDef secondOrderBiome in secondOrderBiomes)
            {
                BiomeCalcs(secondOrderBiome);
            }
            foreach (BiomeDef thirdOrderBiome in thirdOrderBiomes)
            {
                BiomeCalcs(thirdOrderBiome);
            }
        }
        
        private void BiomeCalcs(BiomeDef biomeDef)
        {
            BiomeControls bioExt = biomeDef.GetModExtension<BiomeControls>();
            float minSLAT = bioExt.minSouthLatitude * -1f;
            float maxSLAT = bioExt.maxSouthLatitude * -1f;
            WorldGrid worldGrid = Find.WorldGrid;

            for (int i = 0; i < worldGrid.TilesCount; i++)
            {
                Tile tile = worldGrid[i];
                float y = worldGrid.LongLatOf(i).y;
                int seed = Find.World.info.Seed;
                Vector3 tileCenter = worldGrid.GetTileCenter(i);
                bool flag = true;
                foreach (BiomeDef spawnOnBiome in bioExt.spawnOnBiomes)
                {
                    if (tile.biome == spawnOnBiome)
                    {
                        flag = true;
                        break;
                    }
                    flag = false;
                }
                if (!flag)
                {
                    continue;
                }
                bool flag2 = true;
                flag2 = ((y < minSLAT && y > maxSLAT) ? true : false);
                bool flag3 = true;
                flag3 = ((y > bioExt.minNorthLatitude && y < bioExt.maxNorthLatitude) ? true : false);
                if ((!flag3 && !flag2 && bioExt.minSouthLatitude != -9999f && bioExt.minNorthLatitude != -9999f && bioExt.maxSouthLatitude != -9999f && bioExt.maxNorthLatitude != 9999f) || (tile.WaterCovered && !bioExt.allowOnWater) || (!tile.WaterCovered && !bioExt.allowOnLand) || (bioExt.needRiver && (tile.Rivers == null || tile.Rivers.Count == 0)))
                {
                    continue;
                }
                if (bioExt.perlinCustomSeed.HasValue)
                {
                    seed = bioExt.perlinCustomSeed.Value;
                }
                else if (bioExt.useAlternativePerlinSeedPreset)
                {
                    seed = i;
                }
                if (bioExt.usePerlin)
                {
                    PerlinNoise = new Perlin(bioExt.perlinFrequency, bioExt.perlinLacunarity, bioExt.perlinPersistence, bioExt.perlinOctaves, seed, QualityMode.Low);
                    if (PerlinNoise.GetValue(tileCenter) <= bioExt.perlinCulling)
                    {
                        continue;
                    }
                }
                if ((double)Rand.Value > Math.Pow(bioExt.frequency, 2.0) / 10000.0 || tile.elevation < bioExt.minElevation || tile.elevation > bioExt.maxElevation || tile.temperature < bioExt.minTemperature || tile.temperature > bioExt.maxTemperature || tile.rainfall < bioExt.minRainfall || tile.rainfall > bioExt.maxRainfall || (int)tile.hilliness < (int)bioExt.minHilliness || (int)tile.hilliness > (int)bioExt.maxHilliness)
                {
                    continue;
                }
                List<int> list = new ();
                worldGrid.GetTileNeighbors(i, list);
                int j = 0;
                int num3 = 0;
                bool flag4 = true;
                if (bioExt.minimumWaterNeighbors > 0)
                {
                    for (int count = list.Count; j < count; j++)
                    {
                        if (worldGrid[list[j]].biome == BiomeDefOf.Ocean)
                        {
                            num3++;
                            Log.Message("Water Neighbors =" + num3);
                        }
                        if (num3 < bioExt.minimumWaterNeighbors)
                        {
                            flag4 = false;
                        }
                        if (num3 == bioExt.minimumWaterNeighbors)
                        {
                            flag4 = true;
                        }
                    }
                }
                if (!flag4)
                {
                    continue;
                }
                int num4 = 0;
                bool flag5 = true;
                if (bioExt.minimumWaterNeighbors > 0)
                {
                    for (int count2 = list.Count; j < count2; j++)
                    {
                        if (worldGrid[list[j]].biome != BiomeDefOf.Ocean)
                        {
                            num4++;
                        }
                        if (num3 < bioExt.minimumLandNeighbors)
                        {
                            flag5 = false;
                        }
                        if (num3 == bioExt.minimumLandNeighbors)
                        {
                            flag5 = true;
                        }
                    }
                }
                if (!flag5)
                {
                    continue;
                }
                if (biomeDef.workerClass.Name == "UniversalBiomeWorker")
                {
                    tile.biome = biomeDef;
                }
                if (bioExt.setHills.HasValue)
                {
                    _ = bioExt.setHills.Value;
                    if (bioExt.spawnHills.HasValue && !bioExt.setHills.HasValue)
                    {
                        _ = bioExt.spawnHills.Value;
                        bioExt.setHills = bioExt.spawnHills;
                    }
                    if (bioExt.setHills == BiomeHilliness.Random)
                    {
                        switch (Rand.Range((int)bioExt.minRandomHills.Value, (int)bioExt.maxRandomHills.Value))
                        {
                            case 1:
                                tile.hilliness = Hilliness.Flat;
                                break;
                            case 2:
                                tile.hilliness = Hilliness.SmallHills;
                                break;
                            case 3:
                                tile.hilliness = Hilliness.LargeHills;
                                break;
                            case 4:
                                tile.hilliness = Hilliness.Mountainous;
                                break;
                        }
                    }
                    else if (bioExt.setHills < BiomeHilliness.Random)
                    {
                        tile.hilliness = (Hilliness)bioExt.setHills.Value;
                    }
                }
                if (bioExt.setElevation.HasValue)
                {
                    tile.elevation = bioExt.setElevation.Value;
                }
            }
            Log.Message("finished biome cycle");
        }
    }
}
