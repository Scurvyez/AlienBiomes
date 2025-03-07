using System.Text;
using RimWorld;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    public class Comp_TimedHarvest : ThingComp
    {
        public CompProperties_TimedHarvest Props => (CompProperties_TimedHarvest)props;

        private float _dayPercent;
        private Season _season;

        public bool AdditionalPlantHarvestLogic()
        {
            base.CompTickLong();
            _dayPercent = GenLocalDate.DayPercent(parent.Map);
            _season = GenLocalDate.Season(parent.Map);

            //Example: if (dayPercent > 0.8 && dayPercent < 1.0 || dayPercent < 0.2 && dayPercent > 0.0)
            return (_dayPercent >= Props.harvestStartTime && _dayPercent <= 1f) 
                || (_dayPercent <= Props.harvestStopTime && _dayPercent >= 0f) 
                && Props.harvestSeasons.Contains(_season);
        }
        
        public override string CompInspectStringExtra()
        {
            StringBuilder stringBuilder = new ();
            
            // Format the start and stop times into hours
            int startHour = Mathf.FloorToInt(Props.harvestStartTime * 24);
            int stopHour = Mathf.FloorToInt(Props.harvestStopTime * 24);

            // Add leading zeros for single digit hours
            string startTimeFormatted = $"{startHour:D2}00";
            string stopTimeFormatted = $"{stopHour:D2}00";
            
            // Format the seasons into a readable string
            string seasonsFormatted = string.Join(", ", Props.harvestSeasons);

            // Append the information to the string builder
            stringBuilder.AppendLine("SZ_PlantHarvestSeasonInfo".Translate(seasonsFormatted));
            stringBuilder.AppendLine("SZ_PlantHarvestTimeInfo".Translate(startTimeFormatted, stopTimeFormatted));
            
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