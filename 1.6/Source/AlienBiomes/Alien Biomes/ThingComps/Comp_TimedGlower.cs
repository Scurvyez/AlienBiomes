using RimWorld;
using Verse;
using System.Text;
using UnityEngine;

namespace AlienBiomes
{
    public class Comp_TimedGlower : CompGlower
    {
        public CompProperties_TimedGlower TimeProps => (CompProperties_TimedGlower)props;
        
        private int _lastUpdateTick;
        
        public bool AdditionalGlowerLogic()
        {
            float dP = GenLocalDate.DayPercent(parent.Map);
            return (dP > TimeProps.glowStartTime && dP < 1f) 
                   || (dP < TimeProps.glowStopTime && dP > 0f);
        }
        
        public override void CompTickLong()
        {
            int randomness = Rand.RangeInclusive(120, 240);
            int curTick = Find.TickManager.TicksGame;
            
            if (curTick - _lastUpdateTick <= 2500) return; // once per hour only.
            _lastUpdateTick = curTick + randomness;
            UpdateLit(parent.Map);
        }
        
        public override void CompTickInterval(int delta)
        {
            int randomness = Rand.RangeInclusive(640, 1280);
            int curTick = Find.TickManager.TicksGame;
            
            if (curTick - _lastUpdateTick <= 2500) return;
            _lastUpdateTick = curTick + randomness;
            UpdateLit(parent.Map);
        }
        
        public override string CompInspectStringExtra()
        {
            StringBuilder stringBuilder = new ();
            
            int startHour = Mathf.FloorToInt(TimeProps.glowStartTime * 24);
            int stopHour = Mathf.FloorToInt(TimeProps.glowStopTime * 24);
            
            string startTimeFormatted = $"{startHour:D2}00";
            string stopTimeFormatted = $"{stopHour:D2}00";
            
            stringBuilder.AppendLine("SZAB_PlantGlowerInfo"
                .Translate(startTimeFormatted, stopTimeFormatted));
            
            string baseInspectString = base.CompInspectStringExtra();
            if (!string.IsNullOrEmpty(baseInspectString))
            {
                stringBuilder.AppendLine(baseInspectString);
            }
            return stringBuilder.ToString().TrimEndNewlines();
        }
    }
}