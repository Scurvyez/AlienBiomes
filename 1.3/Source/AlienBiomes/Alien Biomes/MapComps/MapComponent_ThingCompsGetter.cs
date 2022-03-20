using System.Collections.Generic;
using System.Linq;
using Verse;
using UnityEngine;

namespace AlienBiomes
{
    public class MapComponent_ThingCompsGetter : MapComponent
    {
        //public Color FieldEdgesColor = CompProperties_HediffEffecterWithRadius.radiusOutlineColor;
        public HashSet<Comp_HediffEffectorWithRadius> ActiveThingComps = new();
        public List<IntVec3> Cells = new();
        public bool DoDrawing;
        public Color FieldEdgesColor;
        
        public MapComponent_ThingCompsGetter(Map map) : base(map) {
            
        }

        public override void FinalizeInit() {
            base.FinalizeInit();
        }

        /// <summary>
        /// Adds instances of the specified comp to a collection.
        /// </summary>
        public void AddCompInstancesToMap(Comp_HediffEffectorWithRadius tC)
        {
            if (!ActiveThingComps.Contains(tC))
            {
                Cells.Clear();
                Cells.AddRange(ActiveThingComps.SelectMany(tC => 
                    GenRadial.RadialCellsAround(tC.parent.Position, tC.Props.releaseRadius, true)));
                ActiveThingComps.Add(tC);
            }
        }

        /// <summary>
        /// Removes instances of the specified comp from a collection.
        /// </summary>
        public void RemoveCompInstancesFromMap(Comp_HediffEffectorWithRadius tC)
        {
            if (ActiveThingComps.Contains(tC))
            {
                Cells.Clear();
                Cells.AddRange(ActiveThingComps.SelectMany(tC => 
                    GenRadial.RadialCellsAround(tC.parent.Position, tC.Props.releaseRadius, true)));
                ActiveThingComps.Remove(tC);
            }
        }

        /// <summary>
        /// Draws an overlay upon selection of a parent Thing or multiple parent Things.
        /// </summary>
        public override void MapComponentUpdate()
        {
            if (DoDrawing)
            {
                DoDrawing = false;
                // Color set via CompProperties in xml. :)
                GenDraw.DrawFieldEdges(Cells, FieldEdgesColor);
            }
        }
    }
}
