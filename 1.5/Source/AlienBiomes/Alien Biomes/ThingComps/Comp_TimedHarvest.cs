using RimWorld;
using Verse;

namespace AlienBiomes
{
    public class Comp_TimedHarvest : ThingComp
    {
        public CompProperties_TimedHarvest Props => (CompProperties_TimedHarvest)props;

        private float dayPercent;
        private Season season;

        public bool AdditionalPlantHarvestLogic()
        {
            base.CompTickLong();
            dayPercent = GenLocalDate.DayPercent(parent.Map);
            season = GenLocalDate.Season(parent.Map);

            //Example: if (dayPercent > 0.8 && dayPercent < 1.0 || dayPercent < 0.2 && dayPercent > 0.0)
            return (dayPercent >= Props.harvestStartTime && dayPercent <= 1f) 
                || (dayPercent <= Props.harvestStopTime && dayPercent >= 0f) 
                && Props.harvestSeasons.Contains(season);
        }
    }
}
