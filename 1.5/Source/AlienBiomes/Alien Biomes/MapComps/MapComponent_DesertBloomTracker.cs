using System.Collections.Generic;
using JetBrains.Annotations;
using Verse;

namespace AlienBiomes
{
    [UsedImplicitly]
    public class MapComponent_DesertBloomTracker : MapComponent
    {
        public HashSet<Thing> trackedIncidentPlants = [];
        private Dictionary<Thing, int> _plantLifetimes = new();

        public MapComponent_DesertBloomTracker(Map map) : base(map) { }

        public override void MapComponentTick()
        {
            List<Thing> plantsToRemove = [];

            foreach (Thing plant in trackedIncidentPlants)
            {
                if (!_plantLifetimes.ContainsKey(plant)) continue;
                _plantLifetimes[plant]--;

                if (_plantLifetimes[plant] <= 0)
                {
                    plantsToRemove.Add(plant);
                }
            }

            foreach (Thing plant in plantsToRemove)
            {
                trackedIncidentPlants.Remove(plant);
                _plantLifetimes.Remove(plant);
                plant.Destroy();
            }
        }
        
        public void AddPlant(Thing plant, int lifetime)
        {
            trackedIncidentPlants.Add(plant);
            _plantLifetimes[plant] = lifetime;
        }
    }
}