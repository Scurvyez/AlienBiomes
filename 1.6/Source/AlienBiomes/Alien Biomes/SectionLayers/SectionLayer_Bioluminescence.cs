using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace AlienBiomes
{
    public class SectionLayer_Bioluminescence : SectionLayer
    {
        private const string RequiredTerrainTag = "Shallow";
        private const float CellsPerTextureRepeat = 4.25f;
        
        private const int ShoreMinDistance = 0;
        private const int ShoreMaxDistance = 5;
        
        private const float NoiseFrequency = 0.13f;
        private const float NoiseLacunarity = 2.0f;
        private const float NoisePersistence = 0.5f;
        private const int NoiseOctaves = 6;
        private const float NoiseThreshold = 0.62f;
        private const float NoiseCoordScale = 0.20f;
        
        // Cache shoreline distance per MAP INSTANCE (not uniqueID) to avoid stale data across sessions/worlds.
        private static Map _cachedMap;
        private static ushort[] _distToShore;
        private static readonly Queue<int> BfsQueue = new();
        
        // Cache Perlin instance per SEED (seed now includes World seed, so it changes between worlds).
        private static int _cachedPerlinSeed = int.MinValue;
        private static Perlin _perlin;
        
        private readonly List<Vector2> _uv2D = new(2048);
        
        public override bool Visible => true;
        
        public SectionLayer_Bioluminescence(Section section) : base(section)
        {
            relevantChangeTypes = MapMeshFlagDefOf.Terrain;
        }
        
        public override void Regenerate()
        {
            ClearSubMeshes(MeshParts.All);
            
            LayerSubMesh sm = GetSubMesh(AssetCompendium.GetBiolumMaterialForBiome(Map.Biome));
            _uv2D.Clear();

            bool any = false;
            float altitude = AltitudeLayer.VisEffects.AltitudeFor();

            int index = section.botLeft.x;
            foreach (IntVec3 c in section.CellRect)
            {
                float glow = GlowAtCell(Map, c);
                if (glow > 0f)
                {
                    int startVertIndex = sm.verts.Count;
                    AddCell(c, index, startVertIndex, sm, altitude, glow);
                    any = true;
                }
                index++;
            }

            if (!any)
            {
                sm.disabled = true;
                return;
            }

            sm.disabled = false;
            sm.FinalizeMesh(MeshParts.All);

            if (sm.mesh != null && _uv2D.Count == sm.mesh.vertexCount)
            {
                sm.mesh.uv = _uv2D.ToArray();
            }
        }
        
        private void AddCell(IntVec3 c, int index, int startVertIndex, LayerSubMesh sm, float altitude, float glow)
        {
            Rand.PushState(index);

            float x0 = c.x;
            float x1 = c.x + 1;
            float z0 = c.z;
            float z1 = c.z + 1;

            float y = altitude + Rand.Range(-0.01f, 0.01f);

            sm.verts.Add(new Vector3(x0, y, z0));
            sm.verts.Add(new Vector3(x0, y, z1));
            sm.verts.Add(new Vector3(x1, y, z1));
            sm.verts.Add(new Vector3(x1, y, z0));

            float scale = 1f / CellsPerTextureRepeat;

            float u0 = c.x * scale;
            float v0 = c.z * scale;
            float u1 = (c.x + 1) * scale;
            float v1 = (c.z + 1) * scale;

            _uv2D.Add(new Vector2(u0, v0));
            _uv2D.Add(new Vector2(u0, v1));
            _uv2D.Add(new Vector2(u1, v1));
            _uv2D.Add(new Vector2(u1, v0));

            byte a = (byte)(glow * 255f);
            sm.colors.Add(new Color32(255, 255, 255, a));
            sm.colors.Add(new Color32(255, 255, 255, a));
            sm.colors.Add(new Color32(255, 255, 255, a));
            sm.colors.Add(new Color32(255, 255, 255, a));

            sm.tris.Add(startVertIndex);
            sm.tris.Add(startVertIndex + 1);
            sm.tris.Add(startVertIndex + 2);
            sm.tris.Add(startVertIndex);
            sm.tris.Add(startVertIndex + 2);
            sm.tris.Add(startVertIndex + 3);

            Rand.PopState();
        }
        
        private static void EnsureShoreDistanceCache(Map map)
        {
            if (map == null) return;

            int n = map.cellIndices.NumGridCells;

            // Rebuild if the map instance changed or the grid size changed.
            if (_cachedMap == map && _distToShore != null && _distToShore.Length == n)
                return;

            _cachedMap = map;
            _distToShore = new ushort[n];

            const ushort inf = ushort.MaxValue;
            for (int i = 0; i < n; i++) _distToShore[i] = inf;

            BfsQueue.Clear();

            CellIndices ci = map.cellIndices;

            // Seed BFS with shoreline water cells (distance 0)
            foreach (IntVec3 c in map.AllCells)
            {
                TerrainDef t = c.GetTerrain(map);
                if (t is not { IsWater: true }) continue;

                if (!IsWaterCellShoreline(map, c))
                    continue;

                int idx = ci.CellToIndex(c);
                _distToShore[idx] = 0;
                BfsQueue.Enqueue(idx);
            }

            // BFS over WATER only, computing distance from shoreline
            while (BfsQueue.Count > 0)
            {
                int curIdx = BfsQueue.Dequeue();
                IntVec3 cur = ci.IndexToCell(curIdx);
                ushort curD = _distToShore[curIdx];

                for (int i = 0; i < 4; i++)
                {
                    IntVec3 nCell = cur + GenAdj.CardinalDirections[i];
                    if (!nCell.InBounds(map)) continue;

                    TerrainDef nt = nCell.GetTerrain(map);
                    if (nt is not { IsWater: true }) continue;

                    int nIdx = ci.CellToIndex(nCell);
                    if (_distToShore[nIdx] != inf) continue;

                    _distToShore[nIdx] = (ushort)(curD + 1);
                    BfsQueue.Enqueue(nIdx);
                }
            }
        }
        
        private static bool IsWaterCellShoreline(Map map, IntVec3 c)
        {
            for (int i = 0; i < 8; i++)
            {
                IntVec3 n = c + GenAdj.AdjacentCells[i];
                if (!n.InBounds(map)) continue;

                TerrainDef t = n.GetTerrain(map);
                if (t == null) continue;

                if (!t.IsWater)
                    return true;
            }

            return false;
        }
        
        private static ushort ShoreDistance(Map map, IntVec3 c)
        {
            EnsureShoreDistanceCache(map);
            return _distToShore[map.cellIndices.CellToIndex(c)];
        }
        
        private static Perlin GetPerlin(int seed)
        {
            if (_perlin != null && _cachedPerlinSeed == seed) return _perlin;

            _cachedPerlinSeed = seed;
            _perlin = new Perlin(NoiseFrequency, NoiseLacunarity, NoisePersistence,
                NoiseOctaves, seed, QualityMode.Medium);

            return _perlin;
        }

        private static float GlowAtCell(Map map, IntVec3 c)
        {
            TerrainDef t = c.GetTerrain(map);
            if (t is not { IsWater: true }) return 0f;

            ModExt_PlantTerrainControl ext = t.GetModExtension<ModExt_PlantTerrainControl>();
            if (ext?.terrainTags == null || !ext.terrainTags.Contains(RequiredTerrainTag)) return 0f;

            ushort d = ShoreDistance(map, c);
            if (d == ushort.MaxValue) return 0f;
            if (d > ShoreMaxDistance) return 0f;

            // IMPORTANT: include WORLD seed so new worlds generate different biolum patterns.
            int seed = Gen.HashCombineInt(Find.World.info.Seed, 9137421);
            seed = Gen.HashCombineInt(seed, map.uniqueID);

            float x = c.x * NoiseCoordScale;
            float z = c.z * NoiseCoordScale;

            double raw = GetPerlin(seed).GetValue(x, 0.0, z);
            float n01 = Mathf.InverseLerp(-1f, 1f, (float)raw);

            if (n01 < NoiseThreshold) return 0f;

            float intensity = Mathf.InverseLerp(NoiseThreshold, 1f, n01);

            float distFactor = 1f - Mathf.InverseLerp(ShoreMinDistance, ShoreMaxDistance, d);
            intensity *= distFactor;

            return intensity;
        }
    }
}