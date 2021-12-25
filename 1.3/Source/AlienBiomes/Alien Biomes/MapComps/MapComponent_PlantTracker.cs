/*using System;
using RimWorld;
using Verse;
using UnityEngine;
using System.Collections.Generic;

namespace AlienBiomes
{
    public class MapComponent_PlantTracker : MapComponent
    {
        public int tickCounter = 0;
        public int tickInterval = 10000;
        public int plantsOnMap_backup = 0;

        public MapComponent_PlantTracker(Map map) : base(map)
        {

        }

        public override void FinalizeInit()
        {
            if (map.IsPlayerHome)
            {
                StaticCollenctionsClass.plantsOnMap = plantsOnMap_backup;
            }
            base.FinalizeInit();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<int>(ref this.plantsOnMap_backup, "plantsOnMap_backup", 0, true);
            Scribe_Values.Look<int>(ref this.tickCounter, "tickCounterPlants", 0, true);
        }

        public override void MapComponentTick()
        {
            tickCounter++;
            if (tickCounter > tickInterval)
            {
                if (map.IsPlayerHome && map.Biome.defName == "SZ_EnlightenedPlains")
                {

                }
            }
        }
    }
}
*/