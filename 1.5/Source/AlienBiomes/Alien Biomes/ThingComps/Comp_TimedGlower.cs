using RimWorld;
using Verse;
using System.Text;
using UnityEngine;
using Random = System.Random;

namespace AlienBiomes
{
    public class Comp_TimedGlower : CompGlower
    {
        public CompProperties_TimedGlower TimeProps => (CompProperties_TimedGlower)props;
        protected int lastUpdateTick = 0;

        public bool AdditionalGlowerLogic()
        {
            var dP = GenLocalDate.DayPercent(parent.Map);
            return (dP > TimeProps.startTime && dP < 1f) || (dP < TimeProps.stopTime && dP > 0f);
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

        public override string CompInspectStringExtra()
        {
            StringBuilder stringBuilder = new ();
            
            // Format the start and stop times into hours
            int startHour = Mathf.FloorToInt(TimeProps.startTime * 24);
            int stopHour = Mathf.FloorToInt(TimeProps.stopTime * 24);

            // Add leading zeros for single digit hours
            string startTimeFormatted = $"{startHour:D2}00";
            string stopTimeFormatted = $"{stopHour:D2}00";

            // Append the information to the string builder
            stringBuilder.AppendLine("SZ_PlantGlowerInfo".Translate(startTimeFormatted, stopTimeFormatted));
            
            // Include any additional information from base components
            string baseInspectString = base.CompInspectStringExtra();
            if (!string.IsNullOrEmpty(baseInspectString))
            {
                stringBuilder.AppendLine(baseInspectString);
            }
            return stringBuilder.ToString().TrimEndNewlines();
        }
    }
}
