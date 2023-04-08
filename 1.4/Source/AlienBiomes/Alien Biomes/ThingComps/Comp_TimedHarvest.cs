using RimWorld;
using Verse;
using UnityEngine;

namespace AlienBiomes
{
    public class Comp_TimedHarvest : ThingComp
    {
        public CompProperties_TimedHarvest Props => (CompProperties_TimedHarvest)props;

        private float dayPercent;
        private Season season;
        public int TickCounter = 0;

        public bool AdditionalPlantHarvestLogic()
        {
            base.CompTickLong();
            dayPercent = GenLocalDate.DayPercent(parent.Map);
            season = GenLocalDate.Season(parent.Map);

            switch (season)
            {
                case Season.Spring:
                    if (Props.harvestSeasons.Contains(Season.Spring))
                    {
                        return true;
                    }
                    break;
                case Season.Summer:
                    if (Props.harvestSeasons.Contains(Season.Summer))
                    {
                        return true;
                    }
                    break;
                case Season.Fall:
                    if (Props.harvestSeasons.Contains(Season.Fall))
                    {
                        return true;
                    }
                    break;
                case Season.Winter:
                    if (Props.harvestSeasons.Contains(Season.Winter))
                    {
                        return true;
                    }
                    break;
            }

            if ((dayPercent > Props.harvestStartTime && dayPercent < 1f) 
                || (dayPercent < Props.harvestStopTime && dayPercent > 0f))
            {
                return true;
            }
            return false;
        }
    }
}
