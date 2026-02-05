using RimWorld;
using Verse;

namespace AlienBiomes
{
    public class TileMutatorWorker_DeliriousOasis : TileMutatorWorker_Lake
    {
        public TileMutatorWorker_DeliriousOasis(TileMutatorDef def) : base(def) { }

        public override void GeneratePostTerrain(Map map)
        {
            foreach (IntVec3 cell in map.AllCells)
            {
                ProcessCell(cell, map);
            }
        }
        
        protected override void ProcessCell(IntVec3 cell, Map map)
        {
            float valAt = GetValAt(cell, map);
            var terrainDef = map.terrainGrid.TerrainAt(cell);
            
            switch (valAt)
            {
                case > 0.85f:
                    map.terrainGrid.SetTerrain(cell, MapGenUtility.DeepFreshWaterTerrainAt(cell, map));
                    break;
                case > 0.79f:
                    map.terrainGrid.SetTerrain(cell, MapGenUtility.ShallowFreshWaterTerrainAt(cell, map));
                    break;
                case > 0.73f when !terrainDef.IsWater:
                    map.terrainGrid.SetTerrain(cell, InternalDefOf.SZ_DeliriousRichBlackSand);
                    break;
                case > 0.69f when !terrainDef.IsWater:
                    map.terrainGrid.SetTerrain(cell, InternalDefOf.SZ_DeliriousBlackSand);
                    break;
            }
        }
    }
}