using RimWorld;
using Verse;
using System;

namespace AlienBiomes
{
    public class Comp_TimedGlower : CompGlower
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
            Random rand = new();
            int randomness = rand.Next(120, 240);
            int currentTick = Find.TickManager.TicksGame;

            if (currentTick - lastUpdateTick > 2500) // Once per hour only.
            {
                lastUpdateTick = currentTick + randomness;
                UpdateLit(parent.Map);
            }
        }

        public override void CompTick()
        {
            Random rand = new();
            int randomness = rand.Next(640, 1280);
            int currentTick = Find.TickManager.TicksGame;

            if (currentTick - lastUpdateTick > 2500)
            {
                lastUpdateTick = currentTick + randomness;
                UpdateLit(parent.Map);
            }
        }
    }
}
