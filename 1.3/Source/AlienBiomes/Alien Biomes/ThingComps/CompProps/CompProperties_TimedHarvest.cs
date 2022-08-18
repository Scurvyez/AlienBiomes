using Verse;
using System.Collections.Generic;

namespace AlienBiomes
{
    public class CompProperties_TimedHarvest : CompProperties
    {
        public float harvestStartTime = 0.75f;
        public float harvestStopTime = 0.20f;
        /// <summary>
        /// Seasons = Spring, Fall, Winter, Summer, Permanent Summer, Permanent Winter.
        /// </summary>
        public List<string> harvestSeasons = null;
        //public bool seasonalHarvesting = false;

        public CompProperties_TimedHarvest()
        {
            compClass = typeof(Comp_TimedHarvest);
        }
    }
}
