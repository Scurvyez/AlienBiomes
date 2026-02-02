using Verse;
using RimWorld;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace AlienBiomes
{
    [UsedImplicitly]
    public class CompProperties_TimedHarvest : CompProperties
    {
        public float harvestStartTime = 0.75f;
        public float harvestStopTime = 0.20f;
        public List<Season> harvestSeasons = [];
        
        public CompProperties_TimedHarvest() => compClass = typeof(Comp_TimedHarvest);
    }
}