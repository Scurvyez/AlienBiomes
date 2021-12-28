using RimWorld;
using Verse;

namespace AlienBiomes
{
    public class CompTimedGlower : CompGlower
    {
        public CompProperties_TimedGlower TimeProps => (CompProperties_TimedGlower)props;
        protected int lastUpdateTick = 0;

        public bool AdditionalGlowerLogic()
        {
            var dayPercent = GenLocalDate.DayPercent(parent.Map);
            return (dayPercent > TimeProps.startTime && dayPercent < 1f) || (dayPercent < TimeProps.stopTime && dayPercent > 0f);
        }

        public override void CompTickLong()
        {
            int currentTick = Find.TickManager.TicksGame;
            if (currentTick - lastUpdateTick > 2500) //Once per hour only
            {
                lastUpdateTick = currentTick;
                UpdateLit(parent.Map);
            }
        }
    }
}
