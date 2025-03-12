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
            plantTracker.TrackedIncidentPlants.Clear();
            
            Incident_DesertBloom_ModExt incidentExt = def
                .GetModExtension<Incident_DesertBloom_ModExt>();
            
            if (incidentExt == null) return false;
            
            int spawnedCount = 0;
            float curveMidpoint = incidentExt.sandblossomSpawnCurve.Evaluate(map.Size.x);
            IntRange plantsToSpawnCount = ABRangeMaker
                .GetRangeWithMidpointValue((int)curveMidpoint, (int)(curveMidpoint * 0.15f));
            int totalPlantsToSpawn = plantsToSpawnCount.RandomInRange;
            
            List<ThingDef> validPlantsToSpawn = incidentExt.plantsToSpawn
                .FindAll(plantDef => plantDef.HasModExtension<Plant_DesertBloom_ModExt>());
            
            if (validPlantsToSpawn.Count == 0)
            {
                ABLog.Warning("[IncidentWorker_DesertBloom] No valid plants to spawn.");
                return false;
            }
            
            for (int i = 0; i < totalPlantsToSpawn; i++)
            {
                foreach (ThingDef plantDef in validPlantsToSpawn)
                {
                    if (!CellFinderLoose.TryFindRandomNotEdgeCellWith(10,
                            x => CanSpawnAt(x, map, plantDef, incidentExt), 
                            map, out IntVec3 result)) continue;

                    Thing plant = GenSpawn.Spawn(plantDef, result, map);
                    Plant_DesertBloom_ModExt modExt = plantDef
                        .GetModExtension<Plant_DesertBloom_ModExt>();
                    
                    int lifetime = modExt.lifeTime.RandomInRange;
                    plantTracker.AddPlant(plant, lifetime);
                    ABLog.Message($"Added plant: {plant.def } + -{ plant.ThingID}");
                    spawnedCount++;
                }
            }
            
            if (spawnedCount == 0) return false;
            
            Find.LetterStack.ReceiveLetter("SZAB_LetterLabelDesertBloom".Translate(),
                "SZAB_LetterDesertBloom".Translate(), ABDefOf.SZ_DesertBloomLetter, 
                null, null);
            return true;
        }
        
        private static bool CanSpawnAt(IntVec3 c, Map map, ThingDef plantDef, 
            Incident_DesertBloom_ModExt modExt)
        {
            Plant_TerrainControl_ModExt ext = plantDef
                .GetModExtension<Plant_TerrainControl_ModExt>();
            
            if (!c.Standable(map) || c.Fogged(map) || 
                map.fertilityGrid.FertilityAt(c) < plantDef.plant.fertilityMin 
                || !c.GetRoom(map).PsychologicallyOutdoors || c.GetEdifice(map) != null
                || !ext.terrainTags.Contains(c.GetTerrain(map).ToString())) return false;
            
            Plant plant = c.GetPlant(map);
            
            // TODO: CHANGE THIS TO BE 1/2 PLANT LIFE IN MAPCOMP?
            if (plant != null && plant.def.plant.growDays > 10f) return false; 
            
            List<Thing> thingList = c.GetThingList(map);
            foreach (Thing t in thingList)
            {
                if (modExt.plantsToSpawn.Contains(t.def)) return false;
            }
            return true;
        }
    }
}