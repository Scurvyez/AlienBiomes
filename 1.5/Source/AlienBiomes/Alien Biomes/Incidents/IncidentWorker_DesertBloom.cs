using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace AlienBiomes
{
    public class IncidentWorker_DesertBloom : IncidentWorker_MakeGameCondition
    {
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            base.TryExecuteWorker(parms);
            
            Map map = (Map)parms.target;
            Incident_DesertBloom_ModExt incidentExt = def
                .GetModExtension<Incident_DesertBloom_ModExt>();
            
            if (incidentExt == null || incidentExt.plantsToSpawn.Count == 0) return false;
            
            int spawnedCount = 0;
            float curveMidpoint = incidentExt.sandblossomSpawnCurve.Evaluate(map.Size.x);
            IntRange plantsToSpawnCount = ABRangeMaker
                .GetRangeWithMidpointValue((int)curveMidpoint, (int)(curveMidpoint * 0.15f));
            int totalPlantsToSpawn = plantsToSpawnCount.RandomInRange / 5;
            
            ABLog.Message($"Total plants to spawn: {totalPlantsToSpawn}");
            
            // TESTING
            Dictionary<ThingDef, float> plantSpawnCounts = [];
            // TESTING
            
            for (int i = 0; i < totalPlantsToSpawn; i++)
            {
                ThingDef selectedPlant = GetWeightedRandomPlant(incidentExt.plantsToSpawn);
                if (selectedPlant == null) continue;
                
                if (!CellFinderLoose.TryFindRandomNotEdgeCellWith(10, cell => 
                        CanSpawnAt(cell, map, selectedPlant, incidentExt), map, out IntVec3 result))
                    continue;
                
                GenSpawn.Spawn(selectedPlant, result, map);
                spawnedCount++;
                
                // TESTING
                if (plantSpawnCounts.ContainsKey(selectedPlant))
                    plantSpawnCounts[selectedPlant]++;
                else
                    plantSpawnCounts[selectedPlant] = 1;
                // TESTING
            }
            
            if (spawnedCount == 0) return false;
            Find.LetterStack.ReceiveLetter("SZAB_LetterLabelDesertBloom".Translate(),
                "SZAB_LetterDesertBloom".Translate(), ABDefOf.SZ_DesertBloomLetter, 
                null, null);
            
            // TESTING
            foreach (var plant in plantSpawnCounts)
            {
                ABLog.Message($"Spawned {plant.Value} of {plant.Key.defName}");
            }
            // TESTING
            
            return true;
        }
        
        private static ThingDef GetWeightedRandomPlant(Dictionary<ThingDef, float> plantWeights)
        {
            float totalWeight = plantWeights.Values.Sum();
            if (totalWeight <= 0) return null;
            
            float randValue = Rand.Range(0, totalWeight);
            float cumulativeWeight = 0;
            
            foreach (var plant in plantWeights)
            {
                cumulativeWeight += plant.Value;
                if (randValue <= cumulativeWeight)
                    return plant.Key;
            }
            return null;
        }
        
        private static bool CanSpawnAt(IntVec3 c, Map map, ThingDef plantDef, 
            Incident_DesertBloom_ModExt modExt)
        {
            Plant_TerrainControl_ModExt ext = plantDef.GetModExtension<Plant_TerrainControl_ModExt>();
            Plant_TerrainControl_ModExt cExt = c.GetTerrain(map).GetModExtension<Plant_TerrainControl_ModExt>();
            
            if (!c.Standable(map)) return false;
            if (c.Fogged(map)) return false;
            if (map.fertilityGrid.FertilityAt(c) < plantDef.plant.fertilityMin) return false;
            if (!c.GetRoom(map).PsychologicallyOutdoors) return false;
            if (c.GetEdifice(map) != null) return false;
            
            if (cExt == null || !cExt.terrainTags.Any(tag => ext.terrainTags.Contains(tag))) return false;
            
            Plant plant = c.GetPlant(map);
            if (plant != null) return false;
            
            List<Thing> thingList = c.GetThingList(map);
            foreach (Thing t in thingList)
            {
                if (modExt.plantsToSpawn.ContainsKey(t.def))
                {
                    return false;
                }
            }
            return true;
        }
    }
}