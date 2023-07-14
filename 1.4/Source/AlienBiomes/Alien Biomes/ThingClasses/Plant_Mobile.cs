using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    [StaticConstructorOnStartup]
    public class Plant_Mobile : Plant
    {
        private int Counter = 0;
        private Plant_Mobile_ModExtension plantExt; // Reference to the Plant_Mobile_ModExtension
        private MapComponent_MobilePlantCellsGetter mapComponent; // Reference to the MapComponent_MobilePlantCellsGetter
        private List<IntVec3> validNextCells = new ();
        private IntVec3 oldPos;
        private IntVec3 newPos;

        // Store references to avoid repetitive lookups
        private GlowGrid glowGrid;
        private MapDrawer mapDrawer;

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            // Get the Plant_Mobile_ModExtension for this plant
            plantExt = def.GetModExtension<Plant_Mobile_ModExtension>();
            // Get the MapComponent_MobilePlantCellsGetter from the map
            mapComponent = map.GetComponent<MapComponent_MobilePlantCellsGetter>();
            // Store a reference to the GlowGrid for the map
            glowGrid = map.glowGrid;
            // Store a reference to the MapDrawer for the map
            mapDrawer = map.mapDrawer;

            if (mapComponent == null)
            {
                // Create a new MapComponent_MobilePlantCellsGetter if it doesn't exist
                mapComponent = new MapComponent_MobilePlantCellsGetter(map);
                // Add the MapComponent_MobilePlantCellsGetter to the map's components
                map.components.Add(mapComponent);
            }
            // Register this plant with the MapComponent_MobilePlantCellsGetter
            mapComponent.RegisterPlant(this);
        }

        public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
        {
            // Unregister this plant from the MapComponent_MobilePlantCellsGetter
            mapComponent.UnregisterPlant(this);
            base.DeSpawn(mode);
        }

        public override void TickLong()
        {
            base.TickLong();
            oldPos = Position;
            // Get the valid next cells for this plant from the MapComponent_MobilePlantCellsGetter
            validNextCells = mapComponent.GetValidNextCells(this);

            Counter++;
            if (Map != null)
            {
                if (Counter > this.HashOffsetTicks())
                {
                    // Check to make sure it is within the allowed time range of the day on the current map
                    if (GenLocalDate.DayPercent(this) >= plantExt.startTime || GenLocalDate.DayPercent(this) <= plantExt.stopTime)
                    {
                        // Check if the plant is leafless or not
                        if (!LeaflessNow)
                        {
                            // Store the old position before moving
                            oldPos = Position;

                            // Move to a random adjacent unoccupied cell
                            newPos = validNextCells.RandomElement();

                            CompGlower compGlower = GetComp<Comp_TimedGlower>() ?? GetComp<CompGlower>();
                            if (compGlower != null)
                            {
                                // Deregister the glower from the GlowGrid
                                glowGrid.DeRegisterGlower(compGlower);
                                // Move the plant to the new position
                                Position = newPos;
                                // Register the glower with the GlowGrid
                                glowGrid.RegisterGlower(compGlower);
                            }
                            else
                            {
                                // Move the plant to the new position
                                Position = newPos;
                            }
                            // Mark the map mesh at the new position as dirty to update visuals
                            mapDrawer.MapMeshDirty(Position, MapMeshFlag.Things);
                        }
                    }
                }
                Counter = 0;
            }
        }
    }
}
