using RimWorld;
using Verse;
using UnityEngine;

namespace AlienBiomes
{
    public class Comp_TimedHarvest : ThingComp
    {
        public CompProperties_TimedHarvest Props => (CompProperties_TimedHarvest)props;
        public int TickCounter = 0;

        public bool AdditionalPlantHarvestLogic()
        {
            base.CompTickLong();
            float dayPercent = GenLocalDate.DayPercent(parent.Map);
            string season = GenLocalDate.Season(parent.Map).ToString();

            TickCounter++;
            if (TickCounter > 6000)
            {
                Color color = new (0.145f, 0.588f, 0.745f, 1f);

                Log.Message(season.ToString().Colorize(color));
                TickCounter = 0;
            }

            if (Props.harvestSeasons.Contains(GenLocalDate.Season(parent.Map)))
            {
                return true;
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
