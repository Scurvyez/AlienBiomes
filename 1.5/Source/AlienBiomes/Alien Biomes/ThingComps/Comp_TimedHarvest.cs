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
            
            return (_dayPercent >= Props.harvestStartTime && _dayPercent <= 1f) 
                || (_dayPercent <= Props.harvestStopTime && _dayPercent >= 0f) 
                && Props.harvestSeasons.Contains(_season);
        }
        
        public override string CompInspectStringExtra()
        {
            StringBuilder stringBuilder = new ();
            
            int startHour = Mathf.FloorToInt(Props.harvestStartTime * 24);
            int stopHour = Mathf.FloorToInt(Props.harvestStopTime * 24);
            
            string startTimeFormatted = $"{startHour:D2}00";
            string stopTimeFormatted = $"{stopHour:D2}00";
            
            string seasonsFormatted = string.Join(", ", Props.harvestSeasons);
            
            stringBuilder.AppendLine("SZAB_PlantHarvestSeasonInfo"
                .Translate(seasonsFormatted));
            stringBuilder.AppendLine("SZAB_PlantHarvestTimeInfo"
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