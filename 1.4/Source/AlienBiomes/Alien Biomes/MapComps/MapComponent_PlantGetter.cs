using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Verse;
using UnityEngine;
using RimWorld;

namespace AlienBiomes
{
    public class MapComponent_PlantGetter : MapComponent
    {
        public HashSet<IntVec3> Cells = new ();

        public MapComponent_PlantGetter(Map map) : base(map)
        {

        }

        public override void FinalizeInit()
        {
            base.FinalizeInit();

            // Loop over all things on the map
            foreach (Thing thing in map.listerThings.AllThings)
            {
                // Check if it is a plant of the desired type
                if (thing is Plant plant && (plant.def == AlienBiomes_ThingDefOf.SZ_RadiantGrass
                    || plant.def == AlienBiomes_ThingDefOf.SZ_RadiantTallGrass))
                {
                    // Add its position to the Cells list
                    Cells.Add(thing.Position);
                }
            }
        }
    }
}
