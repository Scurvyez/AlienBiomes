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
        private Plant_Mobile_ModExtension plantExt;
        private List<IntVec3> validNextCells;
        private IntVec3 oldPos;
        private IntVec3 newPos;

        public override void TickLong()
        {
            base.TickLong();

            oldPos = Position;
            validNextCells = new ();
            plantExt = def.GetModExtension<Plant_Mobile_ModExtension>();

            foreach (IntVec3 cell in GenAdj.CellsAdjacent8Way(this))
            {
                if (cell.InBounds(Map)
                    && newPos.IsValid
                    && !Map.Size.OnEdge(Map)
                    && !newPos.Filled(Map)
                    && Map.terrainGrid.TerrainAt(cell) == Map.terrainGrid.TerrainAt(oldPos))
                {
                    validNextCells.Add(cell);
                }
            }

            Counter++;
            if (Counter > this.HashOffsetTicks())
            {
                // check to make sure it's within the allowed time range of the day on the current map
                if (GenLocalDate.DayPercent(this) >= plantExt.startTime || GenLocalDate.DayPercent(this) <= plantExt.stopTime)
                {
                    if (Map != null && !LeaflessNow)
                    {
                        // Store the old position before moving
                        oldPos = Position;

                        // Move to a random adjacent unoccupied cell
                        newPos = validNextCells.RandomElement();

                        CompGlower compGlower = GetComp<Comp_TimedGlower>() ?? GetComp<CompGlower>();
                        if (compGlower != null)
                        {
                            Map.glowGrid.DeRegisterGlower(compGlower);
                            Position = newPos;
                            Map.glowGrid.RegisterGlower(compGlower);
                        }
                        else
                        {
                            Position = newPos;
                        }

                        validNextCells.Clear();
                        Map.mapDrawer.MapMeshDirty(Position, MapMeshFlag.Things);

                        // check position of the plant after moving
                        //Log.Message("<color=#4494E3FF>Plant_Mobile at: </color>" + oldPos + "<color=#4494E3FF> moved to </color>" + Position);
                    }
                }
                Counter = 0;
            }
        }
    }
}
