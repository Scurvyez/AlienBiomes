using RimWorld.Planet;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    public class WorldGenStep_Late : WorldGenStep
    {
        private readonly List<BiomeDef> firstOrderBiomes = [];
        private readonly List<BiomeDef> secondOrderBiomes = [];
        private readonly List<BiomeDef> thirdOrderBiomes = [];
        
        public override int SeedPart => 123456789;
        
        public override void GenerateFresh(string seed) => GenerateFreshBiomesViaPriority();
        
        private void GenerateFreshBiomesViaPriority()
        {
            foreach (BiomeDef biome in DefDatabase<BiomeDef>.AllDefsListForReading
                         .Where(x => x.HasModExtension<Biome_Generation_ModExt>()))
            {
                int? priority = biome.GetModExtension<Biome_Generation_ModExt>().biomePriority;
                switch (priority)
                {
                    case 2:
                        secondOrderBiomes.Add(biome);
                        break;
                    case 3:
                        thirdOrderBiomes.Add(biome);
                        break;
                    default:
                        firstOrderBiomes.Add(biome);
                        break;
                }
            }
            
            foreach (List<BiomeDef> biomeList in new[] { firstOrderBiomes, secondOrderBiomes, thirdOrderBiomes })
            {
                foreach (BiomeDef biome in biomeList)
                {
                    BiomeGenCalculations(biome);
                }
            }
        }
        
        private static void BiomeGenCalculations(BiomeDef biomeDef)
        {
            WorldGrid worldGrid = Find.WorldGrid;
            Biome_Generation_ModExt bioExt = biomeDef.GetModExtension<Biome_Generation_ModExt>();
            
            for (int i = 0; i < worldGrid.TilesCount; i++)
            {
                int worldSeed = Find.World.info.Seed;
                Tile tile = worldGrid[i];
                Vector3 tileCenter = worldGrid.GetTileCenter(i);
                
                if (!BiomeGenHelper.CanGenerateOnExistingBiome(tile, bioExt)) continue;
                if (!BiomeGenHelper.CanGenerateAtTileLatitude(i, bioExt, worldGrid)) continue;
                if (!BiomeGenHelper.CanGenerateAtTileIfWaterCoverageOrRiverNeeded(tile, bioExt)) continue;
                if (!BiomeGenHelper.ShouldUsePerlinNoise(bioExt, tileCenter, i, ref worldSeed)) continue;
                if (!BiomeGenHelper.MeetsAbioticBiomeConditions(tile, bioExt)) continue;
                if (!BiomeGenHelper.MeetsNeighboringTileRequirements(i, worldGrid, bioExt)) continue;
                
                if (biomeDef.workerClass == typeof(BiomeWorker_Universal))
                {
                    tile.biome = biomeDef;
                }
                
                BiomeGenHelper.SetTileHillsAndElevation(tile, bioExt);
            }
        }
    }
}