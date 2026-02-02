using RimWorld;
using Verse;

namespace AlienBiomes
{
    public class TileMutatorWorker_DeliriousOasis : TileMutatorWorker_Oasis
    {
        public TileMutatorWorker_DeliriousOasis(TileMutatorDef def)
            : base(def)
        {
            
        }

        protected override void ProcessCell(IntVec3 cell, Map map)
        {
            float valAt = GetValAt(cell, map);
            TerrainDef terrainDef = map.terrainGrid.TerrainAt(cell);
            switch (valAt)
            {
                case > 0.75f:
                    map.terrainGrid.SetTerrain(cell, MapGenUtility.DeepFreshWaterTerrainAt(cell, map));
                    break;
                case > 0.57f:
                    map.terrainGrid.SetTerrain(cell, MapGenUtility.ShallowFreshWaterTerrainAt(cell, map));
                    break;
                case > 0.45f when !terrainDef.IsWater:
                    map.terrainGrid.SetTerrain(cell, InternalDefOf.SZ_DeliriousRichBlackSand);
                    break;
                case > 0.3f when !terrainDef.IsWater:
                    map.terrainGrid.SetTerrain(cell, InternalDefOf.SZ_DeliriousBlackSand);
                    break;
            }
        }
    }
}