using System.Collections.Generic;
using JetBrains.Annotations;
using RimWorld;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    [UsedImplicitly]
    public class MapComponent_DesertBloomTracker : MapComponent
    {
        //public HashSet<Thing> TrackedIncidentPlants = [];
        //public Dictionary<Thing, int> MaturityTimers = new();
        
        //private const int TicksPerDay = 60000;
        //private const float MinDestroyGrowth = 0.05f;
        //private const int MatureDuration = TicksPerDay * 2;
        
        public MapComponent_DesertBloomTracker(Map map) : base(map) { }
        
        /*public override void MapComponentTick()
        {
            List<Thing> plantsToRemove = [];
            foreach (Thing plant in TrackedIncidentPlants)
            {
                if (!plant.IsHashIntervalTick(2500)) continue;
                if (plant is Plant plantObj)
                {
                    if (MaturityTimers.ContainsKey(plant))
                    {
                        // Maturity Phase: Countdown until degradation
                        MaturityTimers[plant] -= 2500;
                        if (MaturityTimers[plant] <= 0)
                        {
                            MaturityTimers.Remove(plant);
                        }
                    }
                    else
                    {
                        // Degradation Phase: Slowly decay plant growth
                        //plantObj.Growth = Mathf.Max(plantObj.Growth - 0.01f, MinDestroyGrowth);

                        // Remove plant if it shrinks too much
                        if (plantObj.Growth <= MinDestroyGrowth)
                        {
                            plantsToRemove.Add(plant);
                        }
                    }
                }
            }
            
            foreach (Thing plant in plantsToRemove)
            {
                TrackedIncidentPlants.Remove(plant);
                MaturityTimers.Remove(plant);
                plant.Destroy();
            }
        }*/
        
        // public void AddPlant(Thing plant)
        // {
        //     if (plant is Plant plantObj)
        //     {
        //         float initialGrowth = Random.Range(MinDestroyGrowth + 0.025f, 0.15f);
        //         plantObj.Growth = initialGrowth;
        //     }
        //     
        //     //TrackedIncidentPlants.Add(plant);
        //     //MaturityTimers[plant] = MatureDuration;
        // }
    }
}