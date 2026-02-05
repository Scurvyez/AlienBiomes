using System.Collections.Generic;
using RimWorld;
using Verse;

namespace AlienBiomes
{
    public class Plant_Improved : Plant
    {
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);

            TryRegisterInHediffGiverMapComp(map);
            TryRegisterInExplosiveMapComp(map);
        }

        public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
        {
            TryUnregisterFromHediffGiverMapComp(Map);
            TryUnregisterFromExplosiveMapComp(Map);

            base.DeSpawn(mode);
        }

        private void TryRegisterInHediffGiverMapComp(Map map)
        {
            var comp = this.TryGetComp<Comp_HediffGiver>();
            if (comp?.props is not CompProperties_HediffGiver props) return;

            var mapComp = map?.GetComponent<MapComponent_PlantGetter_HediffGiver>();
            if (mapComp == null) return;

            RegisterInCells(mapComp.ActiveLocationTriggers, Position, props.triggerRadius);
        }

        private void TryRegisterInExplosiveMapComp(Map map)
        {
            var comp = this.TryGetComp<Comp_Explosive>();
            if (comp?.props is not CompProperties_Explosive props) return;

            var mapComp = map?.GetComponent<MapComponent_PlantGetter_Explosive>();
            if (mapComp == null) return;

            RegisterInCells(mapComp.ActiveLocationTriggers, Position, props.triggerRadius);
        }

        private void TryUnregisterFromHediffGiverMapComp(Map map)
        {
            var comp = this.TryGetComp<Comp_HediffGiver>();
            if (comp?.props is not CompProperties_HediffGiver props) return;

            var mapComp = map?.GetComponent<MapComponent_PlantGetter_HediffGiver>();
            if (mapComp == null) return;

            UnregisterFromCells(mapComp.ActiveLocationTriggers, Position, props.triggerRadius);
        }

        private void TryUnregisterFromExplosiveMapComp(Map map)
        {
            var comp = this.TryGetComp<Comp_Explosive>();
            if (comp?.props is not CompProperties_Explosive props) return;

            var mapComp = map?.GetComponent<MapComponent_PlantGetter_Explosive>();
            if (mapComp == null) return;

            UnregisterFromCells(mapComp.ActiveLocationTriggers, Position, props.triggerRadius);
        }

        private void RegisterInCells(Dictionary<IntVec3, HashSet<Plant_Improved>> triggers, 
            IntVec3 center, float radius)
        {
            foreach (IntVec3 cell in GenRadial.RadialCellsAround(center, radius, useCenter: true))
            {
                if (!triggers.TryGetValue(cell, out var plants))
                {
                    plants = new HashSet<Plant_Improved>();
                    triggers[cell] = plants;
                }
                plants.Add(this);
            }
        }

        private void UnregisterFromCells(Dictionary<IntVec3, HashSet<Plant_Improved>> triggers,
            IntVec3 center, float radius)
        {
            foreach (IntVec3 cell in GenRadial.RadialCellsAround(center, radius, useCenter: true))
            {
                if (!triggers.TryGetValue(cell, out var plants)) continue;

                plants.Remove(this);
                if (plants.Count == 0)
                    triggers.Remove(cell);
            }
        }
    }
}