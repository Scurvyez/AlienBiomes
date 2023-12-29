﻿using System.Collections.Generic;
using Verse;

namespace AlienBiomes
{
    public class MapComponent_PlantGetter : MapComponent
    {
        public Dictionary<IntVec3, HashSet<Plant_Nastic>> ActiveLocationTriggers = new ();

        public MapComponent_PlantGetter(Map map) : base(map) { }
    }
}
