using System.Collections.Generic;
using RimWorld;
using Verse;

namespace AlienBiomes
{
    public class IncidentWorker_DesertBloom : IncidentWorker_MakeGameCondition
    {
        private MapComponent_DesertBloomTracker plantTracker;
        
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            base.TryExecuteWorker(parms);
            
            Map map = (Map)parms.target;
            plantTracker = map.GetComponent<MapComponent_DesertBloomTracker>();
            plantTracker.trackedIncidentPlants.Clear();
            DesertBloomExtension incidentExt = def.GetModExtension<DesertBloomExtension>();
            
            if (incidentExt == null) return false;
            
            int spawnedCount = 0;
            float curveMidpoint = incidentExt.sandblossomSpawnCurve.Evaluate(map.Size.x);
            IntRange plantsToSpawnCount = ABRangeMaker
                .GetRangeWithMidpointValue((int)curveMidpoint, (int)(curveMidpoint * 0.15f));
            int totalPlantsToSpawn = plantsToSpawnCount.RandomInRange;

            List<ThingDef> validPlantsToSpawn = incidentExt.plantsToSpawn
                .FindAll(plantDef => plantDef.HasModExtension<Plant_DesertBloom_ModExtension>());

            if (validPlantsToSpawn.Count == 0)
            {
                ABLog.Warning("No valid plants to spawn.");
                return false;
            }

            for (int i = 0; i < totalPlantsToSpawn; i++)
            {
                ThingDef plantDef = validPlantsToSpawn.RandomElement();
                
                if (!CellFinderLoose.TryFindRandomNotEdgeCellWith(10, (IntVec3 x) 
                        => CanSpawnAt(x, map, plantDef, incidentExt), map, out IntVec3 result)) continue;

                Thing plant = GenSpawn.Spawn(plantDef, result, map);
                Plant_DesertBloom_ModExtension plantModExtension = plantDef
                    .GetModExtension<Plant_DesertBloom_ModExtension>();

                int lifetime = plantModExtension.lifeTime.RandomInRange;
                plantTracker.AddPlant(plant, lifetime);
                spawnedCount++;
            }

            if (spawnedCount == 0) return false;

            Find.LetterStack.ReceiveLetter("SZ_LetterLabelDesertBloom".Translate(),
                "SZ_LetterDesertBloom".Translate(), ABDefOf.SZ_DesertBloomLetter, 
                null, null, null);
            return true;
        }
        
        private static bool CanSpawnAt(IntVec3 c, Map map, ThingDef plantDef, DesertBloomExtension extension)
        {
            BiomePlantControl bPC = plantDef.GetModExtension<BiomePlantControl>();
            
            if (!c.Standable(map) || c.Fogged(map) || map.fertilityGrid.FertilityAt(c) < plantDef.plant.fertilityMin 
                || !c.GetRoom(map).PsychologicallyOutdoors || c.GetEdifice(map) != null
                || !bPC.terrainTags.Contains(c.GetTerrain(map).ToString())) return false;

            Plant plant = c.GetPlant(map);
            if (plant != null && plant.def.plant.growDays > 10f) return false; // TODO: CHANGE THIS TO BE 1/2 PLANT LIFE IN MAPCOMP?

            List<Thing> thingList = c.GetThingList(map);
            foreach (Thing t in thingList)
            {
                if (extension.plantsToSpawn.Contains(t.def)) return false;
            }
            return true;
        }
    }
}