using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace AlienBiomes
{
    public class BiomeNoiseHelper
    {
        // Keyed cache so multiple biomes can use different perlin settings safely.
        private readonly struct PerlinKey
        {
            public readonly float frequency;
            public readonly float lacunarity;
            public readonly float persistence;
            public readonly int octaves;
            public readonly bool normalized;
            public readonly bool invert;
            public readonly int seed; // final combined seed
            public readonly QualityMode quality;
            
            public PerlinKey(float frequency, float lacunarity, float persistence, int octaves,
                bool normalized, bool invert, int seed, QualityMode quality)
            {
                this.frequency = frequency;
                this.lacunarity = lacunarity;
                this.persistence = persistence;
                this.octaves = octaves;
                this.normalized = normalized;
                this.invert = invert;
                this.seed = seed;
                this.quality = quality;
            }
        }

        private sealed class PerlinKeyComparer : IEqualityComparer<PerlinKey>
        {
            public bool Equals(PerlinKey a, PerlinKey b)
            {
                return a.frequency.Equals(b.frequency)
                       && a.lacunarity.Equals(b.lacunarity)
                       && a.persistence.Equals(b.persistence)
                       && a.octaves == b.octaves
                       && a.normalized == b.normalized
                       && a.invert == b.invert
                       && a.seed == b.seed
                       && a.quality == b.quality;
            }

            public int GetHashCode(PerlinKey k)
            {
                unchecked
                {
                    int h = 17;
                    h = h * 31 + k.frequency.GetHashCode();
                    h = h * 31 + k.lacunarity.GetHashCode();
                    h = h * 31 + k.persistence.GetHashCode();
                    h = h * 31 + k.octaves;
                    h = h * 31 + (k.normalized ? 1 : 0);
                    h = h * 31 + (k.invert ? 1 : 0);
                    h = h * 31 + k.seed;
                    h = h * 31 + (int)k.quality;
                    return h;
                }
            }
        }

        private static readonly Dictionary<PerlinKey, Perlin> PerlinCache = new(new PerlinKeyComparer());

        private static Perlin GetOrCreatePerlin(PerlinKey key)
        {
            if (!PerlinCache.TryGetValue(key, out Perlin perlin))
            {
                perlin = new Perlin(
                    key.frequency,
                    key.lacunarity,
                    key.persistence,
                    key.octaves,
                    key.normalized,
                    key.invert,
                    key.seed,
                    key.quality);

                PerlinCache.Add(key, perlin);
            }

            return perlin;
        }

        /// <summary>
        /// Samples Perlin noise at the world-space center of a tile.
        /// If normalized=true, the output is typically 0..1.
        /// </summary>
        public static float PerlinAtTileCenter(WorldGrid worldGrid, PlanetTile planetTile, float frequency,
            float lacunarity, float persistence, int octaves, bool normalized, bool invert, int seedPart,
            QualityMode quality = QualityMode.Medium)
        {
            // deterministic per world + per biome (seedPart)
            int seed = Gen.HashCombineInt(Find.World.info.Seed, seedPart);

            var key = new PerlinKey(frequency, lacunarity, persistence, octaves, normalized, invert, seed, quality);
            Perlin perlin = GetOrCreatePerlin(key);
            Vector3 c = worldGrid.GetTileCenter(planetTile);

            // note: Verse.Noise modules return double; cast to float
            return (float)perlin.GetValue(c.x, c.y, c.z);
        }

        /// <summary>
        /// Convenience: noise gate like vanilla (noise >= threshold).
        /// </summary>
        public static bool PerlinPassesThreshold(WorldGrid worldGrid, PlanetTile planetTile, float threshold,
            float frequency, float lacunarity, float persistence, int octaves, bool normalized, bool invert,
            int seedPart, QualityMode quality = QualityMode.Medium)
        {
            float n = PerlinAtTileCenter(worldGrid, planetTile, frequency, lacunarity, persistence, octaves, 
                normalized, invert, seedPart, quality);
            return n >= threshold;
        }
    }
}