using RimWorld;
using System.Collections.Generic;
using JetBrains.Annotations;
using Verse;

namespace AlienBiomes
{
    [UsedImplicitly]
    public class MapComponent_PlantGetter : MapComponent
    {
        public Dictionary<IntVec3, HashSet<Plant_Nastic>> ActiveLocationTriggers = new ();
        public float SunStrength;
        
        public MapComponent_PlantGetter(Map map) : base(map) { }
        
        public override void MapComponentTick()
        {
            base.MapComponentTick();

            // USE THIS SPACE FOR TESTING STUFF SINCE WE DON'T USE IT FOR ANYTHING ELSE :)
            SunStrength = GenCelestial.CurCelestialSunGlow(map);
        }
    }
}