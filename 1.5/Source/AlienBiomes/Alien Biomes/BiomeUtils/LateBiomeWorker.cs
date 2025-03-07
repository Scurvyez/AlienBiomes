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
        private readonly List<BiomeDef> firstOrderBiomes = [];
        private readonly List<BiomeDef> secondOrderBiomes = [];
        private readonly List<BiomeDef> thirdOrderBiomes = [];
        
        public override int SeedPart => 123456789;
        
        public override void GenerateFresh(string seed) => GenerateFreshBiomesViaPriority();
        
        private void GenerateFreshBiomesViaPriority()
        {
            foreach (BiomeDef biome in DefDatabase<BiomeDef>.AllDefsListForReading
                         .Where(x => x.HasModExtension<BiomeControls>()))
            {
                int? priority = biome.GetModExtension<BiomeControls>().biomePriority;
                switch (priority)
                {
                    case 2:
                        ABLog.Message($"Adding {biome.defName} to secondary biomes.");
                        secondOrderBiomes.Add(biome);
                        break;
                    case 3:
                        ABLog.Message($"Adding {biome.defName} to tertiary biomes.");
                        thirdOrderBiomes.Add(biome);
                        break;
                    default:
                        ABLog.Message($"Adding {biome.defName} to primary biomes.");
                        firstOrderBiomes.Add(biome);
                        break;
                }
            }
            
            foreach (var biomeList in new[] { firstOrderBiomes, secondOrderBiomes, thirdOrderBiomes })
            {
                foreach (BiomeDef biome in biomeList)
                {
                    BiomeCalculations(biome);
                }
            }
            
            ABLog.Message("Finished biome cycle");
        }
        
        private void BiomeCalculations(BiomeDef biomeDef)
        {
            WorldGrid worldGrid = Find.WorldGrid;
            BiomeControls bioExt = biomeDef.GetModExtension<BiomeControls>();
            float minSouthLAT = bioExt.minSouthLatitude * -1f;
            float maxSouthLAT = bioExt.maxSouthLatitude * -1f;
            
            for (int i = 0; i < worldGrid.TilesCount; i++)
            {
                bool flag = true;
                int worldSeed = Find.World.info.Seed;
                
                Tile tile = worldGrid[i];
                float latitude = worldGrid.LongLatOf(i).y;
                Vector3 tileCenter = worldGrid.GetTileCenter(i);
                
                foreach (BiomeDef spawnOnBiome in bioExt.spawnOnBiomes)
                {
                    if (tile.biome == spawnOnBiome)
                    {
                        flag = true;
                        break;
                    }
                    flag = false;
                }
                
                if (!flag) continue;
                
                bool flag2 = latitude < minSouthLAT && latitude > maxSouthLAT;
                bool flag3 = latitude > bioExt.minNorthLatitude && latitude < bioExt.maxNorthLatitude;
                
                if ((!flag3 && !flag2 && Mathf.Approximately(bioExt.minSouthLatitude, -9999f) 
                     && Mathf.Approximately(bioExt.minNorthLatitude, -9999f) 
                     && Mathf.Approximately(bioExt.maxSouthLatitude, -9999f) 
                     && Mathf.Approximately(bioExt.maxNorthLatitude, 9999f)) 
                    || (tile.WaterCovered && !bioExt.allowOnWater) 
                    || (!tile.WaterCovered && !bioExt.allowOnLand) 
                    || (bioExt.needRiver && (tile.Rivers == null || tile.Rivers.Count == 0)))
                {
                    continue;
                }
                
                if (bioExt.perlinCustomSeed.HasValue)
                {
                    worldSeed = bioExt.perlinCustomSeed.Value;
                }
                else if (bioExt.useAlternativePerlinSeedPreset)
                {
                    worldSeed = i;
                }
                
                if (bioExt.usePerlin)
                {
                    PerlinNoise = new Perlin(bioExt.perlinFrequency, bioExt.perlinLacunarity, 
                        bioExt.perlinPersistence, bioExt.perlinOctaves, worldSeed, QualityMode.Low);
                    
                    if (PerlinNoise.GetValue(tileCenter) <= bioExt.perlinCulling)
                    {
                        continue;
                    }
                }
                
                if (Rand.Value > Math.Pow(bioExt.frequency, 2.0) / 10000.0 
                    || tile.elevation < bioExt.minElevation 
                    || tile.elevation > bioExt.maxElevation 
                    || tile.temperature < bioExt.minTemperature 
                    || tile.temperature > bioExt.maxTemperature 
                    || tile.rainfall < bioExt.minRainfall 
                    || tile.rainfall > bioExt.maxRainfall 
                    || (int)tile.hilliness < (int)bioExt.minHilliness 
                    || (int)tile.hilliness > (int)bioExt.maxHilliness)
                {
                    continue;
                }
                
                List<int> list = [];
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
                            ABLog.Message("Water Neighbors =" + num3);
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
                
                bool flag5 = true;
                if (bioExt.minimumWaterNeighbors > 0)
                {
                    for (int count2 = list.Count; j < count2; j++)
                    {
                        if (worldGrid[list[j]].biome != BiomeDefOf.Ocean)
                        {
                            
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
                        tile.hilliness =
                            Rand.Range((int)bioExt.minRandomHills.Value, 
                                    (int)bioExt.maxRandomHills.Value) switch
                            {
                                1 => Hilliness.Flat,
                                2 => Hilliness.SmallHills,
                                3 => Hilliness.LargeHills,
                                4 => Hilliness.Mountainous,
                                _ => tile.hilliness
                            };
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
        }
    }
}