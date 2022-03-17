﻿using RimWorld;
using Verse;

namespace AlienBiomes
{
    public class Comp_TimedHarvest : ThingComp
    {
        public CompProperties_TimedHarvest HarvestProps => (CompProperties_TimedHarvest)props;

        public bool AdditionalPlantHarvestLogic()
        {
            base.CompTickLong();
            var dayPercent = GenLocalDate.DayPercent(parent.Map);
            return (dayPercent > HarvestProps.harvestStartTime && dayPercent < 1f) || (dayPercent < HarvestProps.harvestStopTime && dayPercent > 0f);
        }
    }
}
