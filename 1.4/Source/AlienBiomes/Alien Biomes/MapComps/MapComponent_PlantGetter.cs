using System.Collections.Generic;
using Verse;
using RimWorld;
using System.Linq;
using System;

namespace AlienBiomes
{
    public class MapComponent_PlantGetter : MapComponent
    {
        public Dictionary<IntVec3, HashSet<Plant>> ActiveLocationTriggers = new ();

        public MapComponent_PlantGetter(Map map) : base(map) { }
    }
}
