using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace AlienBiomes
{
    public static class BiomeGenHelper
    {
        private static ModuleBase PerlinNoise;
        
        /// <summary>
        /// Determines whether the biome can be generated at the given tile based on the tile's existing biome.
        /// The method checks if the tile's biome is in the list of allowed biomes specified in <paramref name="ext"/>.
        /// </summary>
        /// <param name="tile">The world tile being evaluated for biome generation.</param>
        /// <param name="ext">The biome generation extension containing the allowed existing biomes.</param>
        /// <returns>
        /// <c>true</c> if the tile's biome is in the list of allowed biomes; otherwise, <c>false</c>.
        /// If no specific biomes are defined in <paramref name="ext"/>, the method defaults to <c>true</c>.
        /// </returns>
        public static bool CanGenerateOnExistingBiome(Tile tile, Biome_Generation_ModExt ext)
        {
            foreach (BiomeDef allowedBiome in ext.spawnOnBiomes)
            {
                return tile.biome == allowedBiome;
            }
            return true;
        }
        
        /// <summary>
        /// Determines whether a biome can be generated at the given tile based on its latitude.
        /// The method checks if the tile's latitude falls within the specified northern or southern range
        /// defined in <paramref name="ext"/>.
        /// </summary>
        /// <param name="tileIndex">The index of the tile in the world grid.</param>
        /// <param name="ext">The biome generation extension containing latitude constraints.</param>
        /// <param name="worldGrid">The world grid used to retrieve tile latitude.</param>
        /// <returns>
        /// <c>true</c> if the tile's latitude falls within the allowed northern or southern range,
        /// or if latitude restrictions are disabled. Otherwise, <c>false</c>.
        /// </returns>
        public static bool CanGenerateAtTileLatitude(int tileIndex, Biome_Generation_ModExt ext, WorldGrid worldGrid)
        {
            float latitude = worldGrid.LongLatOf(tileIndex).y;
            float minSouthLAT = ext.minSouthLatitude * -1f;
            float maxSouthLAT = ext.maxSouthLatitude * -1f;
            bool isInSouthernRange = latitude < minSouthLAT && latitude > maxSouthLAT;
            bool isInNorthernRange = latitude > ext.minNorthLatitude && latitude < ext.maxNorthLatitude;
            
            bool hasLatitudeRestrictions = Mathf.Approximately(ext.minSouthLatitude, -9999f)
                                           && Mathf.Approximately(ext.minNorthLatitude, -9999f)
                                           && Mathf.Approximately(ext.maxSouthLatitude, -9999f)
                                           && Mathf.Approximately(ext.maxNorthLatitude, 9999f);
            
            return (isInNorthernRange || isInSouthernRange || !hasLatitudeRestrictions);
        }
        
        /// <summary>
        /// Determines whether a biome can be generated on the given tile based on water coverage
        /// and river requirements. This method ensures the tile meets the biome's constraints
        /// for land, water, and the presence of a river if required.
        /// </summary>
        /// <param name="tile">The tile being evaluated for biome generation.</param>
        /// <param name="ext">The biome generation extension containing water and river constraints.</param>
        /// <returns>
        /// <c>true</c> if the tile meets the biome's water and river requirements;
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool CanGenerateAtTileIfWaterCoverageOrRiverNeeded(Tile tile, Biome_Generation_ModExt ext)
        {
            bool validWaterCheck = (!tile.WaterCovered || ext.allowOnWater) && (tile.WaterCovered || ext.allowOnLand);
            bool validRiverCheck = !ext.needRiver || (tile.Rivers != null && tile.Rivers.Count != 0);

            return validWaterCheck && validRiverCheck;
        }
        
        /// <summary>
        /// Determines whether a biome should be generated at the given tile based on Perlin noise.
        /// This method evaluates the Perlin noise value at the tile's position and compares it
        /// against the biome's culling threshold to decide if the tile should be culled.
        /// </summary>
        /// <param name="ext">The biome generation extension containing Perlin noise settings.</param>
        /// <param name="tileCenter">The world position of the tile being evaluated.</param>
        /// <param name="tileIndex">The index of the tile in the world grid.</param>
        /// <param name="worldSeed">
        /// A reference to the world seed used for Perlin noise calculations.
        /// This may be modified based on custom or alternative Perlin seed settings.
        /// </param>
        /// <returns>
        /// <c>true</c> if the tile passes the Perlin noise check and should be considered for biome generation;
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool ShouldUsePerlinNoise(Biome_Generation_ModExt ext, Vector3 tileCenter, int tileIndex,
            ref int worldSeed)
        {
            if (ext.perlinCustomSeed.HasValue)
            {
                worldSeed = ext.perlinCustomSeed.Value;
            }
            else if (ext.useAlternativePerlinSeedPreset)
            {
                worldSeed = tileIndex;
            }
            
            if (!ext.usePerlin)
            {
                return true; // no Perlin noise culling
            }
            
            PerlinNoise = new Perlin(ext.perlinFrequency, ext.perlinLacunarity, 
                ext.perlinPersistence, ext.perlinOctaves, worldSeed, QualityMode.Low);
            return PerlinNoise.GetValue(tileCenter) > ext.perlinCulling;
        }
        
        /// <summary>
        /// Determines whether a tile meets the abiotic conditions required for a biome to be generated.
        /// This includes checks for elevation, temperature, rainfall, and hilliness,
        /// as well as a frequency-based probability.
        /// </summary>
        /// <param name="tile">The tile being evaluated.</param>
        /// <param name="ext">The biome generation extension containing abiotic condition thresholds.</param>
        /// <returns>
        /// <c>true</c> if the tile meets all abiotic requirements for biome generation; otherwise, <c>false</c>.
        /// </returns>
        public static bool MeetsAbioticBiomeConditions(Tile tile, Biome_Generation_ModExt ext)
        {
            return Rand.Value <= Math.Pow(ext.frequency, 2.0) / 10000.0
                   && tile.elevation >= ext.minElevation
                   && tile.elevation <= ext.maxElevation
                   && tile.temperature >= ext.minTemperature
                   && tile.temperature <= ext.maxTemperature
                   && tile.rainfall >= ext.minRainfall
                   && tile.rainfall <= ext.maxRainfall
                   && (int)tile.hilliness >= (int)ext.minHilliness
                   && (int)tile.hilliness <= (int)ext.maxHilliness;
        }
        
        /// <summary>
        /// Determines whether a tile meets the required number of neighboring water and land tiles 
        /// for biome generation. This ensures that biomes requiring proximity to water or land 
        /// have the necessary environmental conditions.
        /// </summary>
        /// <param name="tileIndex">The index of the tile being evaluated.</param>
        /// <param name="worldGrid">The world grid containing all tiles.</param>
        /// <param name="ext">The biome generation extension containing neighbor requirements.</param>
        /// <returns>
        /// <c>true</c> if the tile meets the required number of water and land neighbors; otherwise, <c>false</c>.
        /// </returns>
        public static bool MeetsNeighboringTileRequirements(int tileIndex, WorldGrid worldGrid,
            Biome_Generation_ModExt ext)
        {
            if (ext.minimumWaterNeighbors <= 0 && ext.minimumLandNeighbors <= 0)
                return true;
            
            List<int> tileNeighborIndexes = [];
            worldGrid.GetTileNeighbors(tileIndex, tileNeighborIndexes);
            int waterNeighbors = 0;
            int landNeighbors = 0;
            
            foreach (int index in tileNeighborIndexes)
            {
                if (worldGrid[index].biome == BiomeDefOf.Ocean)
                {
                    waterNeighbors++;
                }
                else
                {
                    landNeighbors++;
                }
            }
            
            bool meetsWaterReqs = ext.minimumWaterNeighbors <= 0 
                                  || waterNeighbors >= ext.minimumWaterNeighbors;
            bool meetsLandReqs = ext.minimumLandNeighbors <= 0
                                 || landNeighbors >= ext.minimumLandNeighbors;
            return meetsWaterReqs && meetsLandReqs;
        }
        
        /// <summary>
        /// Sets the hilliness and elevation of a tile based on the biome generation settings.  
        /// If specific hilliness or elevation values are provided, they are applied directly;  
        /// otherwise, random hilliness may be assigned within a defined range.  
        /// </summary>
        /// <param name="tile">The tile whose hilliness and elevation are being modified.</param>
        /// <param name="bioExt">The biome generation extension containing hilliness and elevation settings.</param>
        public static void SetTileHillsAndElevation(Tile tile, Biome_Generation_ModExt bioExt)
        {
            if (bioExt.setHills.HasValue)
            {
                _ = bioExt.setHills.Value;
                if (bioExt.spawnHills.HasValue && !bioExt.setHills.HasValue)
                {
                    _ = bioExt.spawnHills.Value;
                    bioExt.setHills = bioExt.spawnHills;
                }
                
                if (bioExt.minRandomHills != null && bioExt.maxRandomHills != null)
                {
                    tile.hilliness = bioExt.setHills switch
                    {
                        BiomeHilliness.Random => Rand.Range((int)bioExt.minRandomHills.Value,
                                (int)bioExt.maxRandomHills.Value) switch
                            {
                                1 => Hilliness.Flat,
                                2 => Hilliness.SmallHills,
                                3 => Hilliness.LargeHills,
                                4 => Hilliness.Mountainous,
                                _ => tile.hilliness
                            },
                        < BiomeHilliness.Random => (Hilliness)bioExt.setHills.Value,
                        _ => tile.hilliness
                    };
                }
            }
            
            if (bioExt.setElevation.HasValue)
            {
                tile.elevation = bioExt.setElevation.Value;
            }
        }
    }
}