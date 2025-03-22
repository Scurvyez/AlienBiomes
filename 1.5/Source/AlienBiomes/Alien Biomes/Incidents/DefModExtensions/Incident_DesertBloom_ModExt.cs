using System.Collections.Generic;
using JetBrains.Annotations;
using Verse;

namespace AlienBiomes
{
    [UsedImplicitly]
    public class Incident_DesertBloom_ModExt : DefModExtension
    {
        //public List<ThingDef> plantsToSpawn;
        public Dictionary<ThingDef, float> plantsToSpawn = [];
        public SimpleCurve sandblossomSpawnCurve;
    }
}