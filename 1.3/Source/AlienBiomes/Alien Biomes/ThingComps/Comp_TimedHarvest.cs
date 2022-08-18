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
            var dP = GenLocalDate.DayPercent(parent.Map);
            var sTS = GenLocalDate.Season(parent.Map).ToString();

            TickCounter++;
            if (TickCounter > 6000)
            {
                Color c = new (0.145f, 0.588f, 0.745f, 1f);

                Log.Message(sTS.ToString().Colorize(c));
                TickCounter = 0;
            }

            if (((dP > Props.harvestStartTime && dP < 1f) || (dP < Props.harvestStopTime && dP > 0f))
                || (GenLocalDate.Season(parent.Map).ToString() == Props.harvestSeasons.Any().ToString()))
            {
                return true;
            }
            return false;
        }
    }
}
