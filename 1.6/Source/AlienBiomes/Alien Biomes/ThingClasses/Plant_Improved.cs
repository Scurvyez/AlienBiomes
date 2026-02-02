using System.Linq;
using RimWorld;
using Verse;

namespace AlienBiomes
{
    public class Plant_Improved : Plant
    {
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            
            TryPopulateMapComp_PlantGetter_HediffGiver(map);
            TryPopulateMapComp_PlantGetter_Explosive(map);
        }

        public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
        {
            TryRemoveFromMapComp_PlantGetter_HediffGiver(Map, mode);
            TryRemoveFromMapComp_PlantGetter_Explosive(Map, mode);
        }

        private void TryPopulateMapComp_PlantGetter_HediffGiver(Map map)
        {
            var comp = this.TryGetComp<Comp_HediffGiver>();
            var props = (CompProperties_HediffGiver)comp?.props;

            if (props != null)
            {
                IntVec3[] cells = GenRadial.RadialCellsAround(Position, props.triggerRadius, useCenter: true).ToArray();
                var mapComp = map.GetComponent<MapComponent_PlantGetter_HediffGiver>();
            
                foreach (IntVec3 cell in cells)
                {
                    if (!mapComp.ActiveLocationTriggers.ContainsKey(cell))
                    {
                        mapComp.ActiveLocationTriggers[cell] = [];
                    }
                    mapComp.ActiveLocationTriggers[cell].Add(this);
                }
            }
        }
        
        private void TryPopulateMapComp_PlantGetter_Explosive(Map map)
        {
            var comp = this.TryGetComp<Comp_Explosive>();
            var props = (CompProperties_Explosive)comp?.props;

            if (props != null)
            {
                IntVec3[] cells = GenRadial.RadialCellsAround(Position, props.triggerRadius, useCenter: true).ToArray();
                var mapComp = map.GetComponent<MapComponent_PlantGetter_Explosive>();
            
                foreach (IntVec3 cell in cells)
                {
                    if (!mapComp.ActiveLocationTriggers.ContainsKey(cell))
                    {
                        mapComp.ActiveLocationTriggers[cell] = [];
                    }
                    mapComp.ActiveLocationTriggers[cell].Add(this);
                }
            }
        }
        
        private void TryRemoveFromMapComp_PlantGetter_HediffGiver(Map map, DestroyMode mode = DestroyMode.Vanish)
        {
            var plantGetterExplosive = map?.GetComponent<MapComponent_PlantGetter_HediffGiver>();
            plantGetterExplosive?.ActiveLocationTriggers
                .Where(kvp => kvp.Value.Remove(this) && 
                              kvp.Value.Count == 0)
                .Select(kvp => kvp.Key)
                .ToList()
                .ForEach(key => plantGetterExplosive.ActiveLocationTriggers.Remove(key));
            
            base.DeSpawn(mode);
        }
        
        private void TryRemoveFromMapComp_PlantGetter_Explosive(Map map, DestroyMode mode = DestroyMode.Vanish)
        {
            var plantGetterExplosive = map?.GetComponent<MapComponent_PlantGetter_Explosive>();
            plantGetterExplosive?.ActiveLocationTriggers
                .Where(kvp => kvp.Value.Remove(this) && 
                              kvp.Value.Count == 0)
                .Select(kvp => kvp.Key)
                .ToList()
                .ForEach(key => plantGetterExplosive.ActiveLocationTriggers.Remove(key));
            
            base.DeSpawn(mode);
        }
    }
}