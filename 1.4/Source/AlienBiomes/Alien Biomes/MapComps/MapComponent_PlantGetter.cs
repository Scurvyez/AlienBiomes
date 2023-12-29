using System.Collections.Generic;
using Verse;

namespace AlienBiomes
{
    public class MapComponent_PlantGetter : MapComponent
    {
        public Dictionary<IntVec3, HashSet<Plant_Nastic>> ActiveLocationTriggers = new ();

        public MapComponent_PlantGetter(Map map) : base(map) { }

        public override void MapComponentTick()
        {
            base.MapComponentTick();
            LogRandomPlantInfo();
        }

        private void LogRandomPlantInfo()
        {
            foreach (HashSet<Plant_Nastic> plantsInCell in ActiveLocationTriggers.Values)
            {
                if (plantsInCell.Count > 0)
                {
                    Plant_Nastic randomPlant = plantsInCell.RandomElement();

                    // Log information about a single random plant from our collection
                    //Log.Message($"Random plant triggered! Plant def: {randomPlant.def.LabelCap}, Position: {randomPlant.Position}, Scale: {randomPlant.currentScale}");
                    return;
                }
            }
        }
    }
}
