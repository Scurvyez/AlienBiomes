using RimWorld;
using Verse;

namespace AlienBiomes
{
    public class Comp_TimedHarvest : ThingComp
    {
        public CompProperties_TimedHarvest HarvestProps => (CompProperties_TimedHarvest)props;

        public bool AdditionalPlantHarvestLogic()
        {
            base.CompTickLong();
            var dP = GenLocalDate.DayPercent(parent.Map);
            return (dP > HarvestProps.harvestStartTime && dP < 1f) 
                || (dP < HarvestProps.harvestStopTime && dP > 0f);
        }
    }
}
