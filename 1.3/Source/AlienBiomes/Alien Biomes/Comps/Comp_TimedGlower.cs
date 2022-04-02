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
            var dP = GenLocalDate.DayPercent(parent.Map);
            return (dP > TimeProps.startTime && dP < 1f) 
                || (dP < TimeProps.stopTime && dP > 0f);
        }
        
        public override void CompTickLong()
        {
            Random rand = new();
            int randomness = rand.Next(120, 240);
            int curTick = Find.TickManager.TicksGame;

            if (curTick - lastUpdateTick > 2500) // Once per hour only.
            {
                lastUpdateTick = curTick + randomness;
                UpdateLit(parent.Map);
            }
        }

        public override void CompTick()
        {
            Random rand = new();
            int randomness = rand.Next(640, 1280);
            int curTick = Find.TickManager.TicksGame;

            if (curTick - lastUpdateTick > 2500)
            {
                lastUpdateTick = curTick + randomness;
                UpdateLit(parent.Map);
            }
        }
    }
}
