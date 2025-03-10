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
        
        public override void CompTick()
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
            
            // convert start and stop times to hours
            int startHour = Mathf.FloorToInt(TimeProps.glowStartTime * 24);
            int stopHour = Mathf.FloorToInt(TimeProps.glowStopTime * 24);
            
            // add leading zeros for single digit hours
            string startTimeFormatted = $"{startHour:D2}00";
            string stopTimeFormatted = $"{stopHour:D2}00";
            
            // append info to the string builder
            stringBuilder.AppendLine("SZAB_PlantGlowerInfo"
                .Translate(startTimeFormatted, stopTimeFormatted));
            
            // include any additional information from base components
            string baseInspectString = base.CompInspectStringExtra();
            if (!string.IsNullOrEmpty(baseInspectString))
            {
                stringBuilder.AppendLine(baseInspectString);
            }
            return stringBuilder.ToString().TrimEndNewlines();
        }
    }
}