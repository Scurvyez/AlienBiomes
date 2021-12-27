using RimWorld;
using Verse;

namespace AlienBiomes
{
    public class CompTimedGlower : CompGlower
    {
        public CompProperties_TimedGlower TimeProps => (CompProperties_TimedGlower)props;

        public bool AdditionalGlowerLogic()
        {
            var dayPercent = GenLocalDate.DayPercent(parent.Map);
            return (dayPercent > TimeProps.startTime && dayPercent < 1f) || (dayPercent < TimeProps.stopTime && dayPercent > 0f);
        }
    }
}
