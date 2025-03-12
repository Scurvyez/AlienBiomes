using System.Collections.Generic;
using JetBrains.Annotations;
using Verse;

namespace AlienBiomes
{
    [UsedImplicitly]
    public class MapComponent_DesertBloomTracker : MapComponent
    {
        public HashSet<Thing> TrackedIncidentPlants = [];
        
        private Dictionary<Thing, int> _plantLifetimes = new();
        
        public MapComponent_DesertBloomTracker(Map map) : base(map) { }
        
        public override void MapComponentTick()
        {
            List<Thing> plantsToRemove = [];
            foreach (Thing plant in TrackedIncidentPlants)
            {
                if (!_plantLifetimes.ContainsKey(plant)) continue;
                if (!plant.IsHashIntervalTick(250)) continue;
                _plantLifetimes[plant]--;
                
                if (_plantLifetimes[plant] <= 0)
                {
                    plantsToRemove.Add(plant);
                }
            }
            
            foreach (Thing plant in plantsToRemove)
            {
                TrackedIncidentPlants.Remove(plant);
                _plantLifetimes.Remove(plant);
                plant.Destroy();
            }
        }
        
        public void AddPlant(Thing plant, int lifetime)
        {
            TrackedIncidentPlants.Add(plant);
            _plantLifetimes[plant] = lifetime;
        }
    }
}