using Verse;

namespace AlienBiomes
{
    public class CompProperties_TimedHarvest : CompProperties
    {
        public float harvestStartTime = 0.75f;
        public float harvestStopTime = 0.20f;

        public CompProperties_TimedHarvest()
        {
            this.compClass = typeof(Comp_TimedHarvest);
        }
    }
}
