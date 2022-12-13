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
        public int Counter = 0;
        public TerrainDef ter = new ();

        // Grab all cells with existing ThingDef in them already and cache those for use...
        // instead of scanning over the entire map.

        /// <summary>
        /// Checks to see if a given cell on the map is not occupied.
        /// Swap into a MapComp?
        /// </summary>
        private bool IsProperTerrain()
        {
            if (this.RandomAdjacentCell8Way().IsValid == true
                && this.RandomAdjacentCell8Way().Filled(this.Map) == false)
            {
                return true;
            }
            return false;
        }

        public override void TickLong()
        {
            base.TickLong();

            // See if the tile is a good cell to move to.
            bool iPT = IsProperTerrain();
            Plant_Mobile_ModExtension mobileExt = def.GetModExtension<Plant_Mobile_ModExtension>();

            // Dict = cell + is valid/unoccupied.
            Dictionary<IntVec3, bool> availCells = new();
            foreach (IntVec3 cell in Map.AllCells)
            {
                if (this.Map.terrainGrid.TerrainAt(cell).IsWater
                    && !this.Map.Size.OnEdge(this.Map))
                {
                    availCells.SetOrAdd(cell, iPT);
                    //int count = availCells.Count;
                    //Log.Message(count + "<color=green> cells are available.</color>");
                }
            }

            Counter++;
            if (Counter > mobileExt.movementCounter)
            {
                if (this.Map != null && !this.LeaflessNow)
                {
                    //Map.glowGrid.DeRegisterGlower(GetComp<Comp_TimedGlower>());
                    Position = availCells.RandomElement().Key;
                    //Map.glowGrid.RegisterGlower(GetComp<Comp_TimedGlower>());
                    //Log.Message(this.def + "<color=green> at </color>" + this.Position + "<color=green> moved to </color>" + availCells.RandomElement().Key);
                }
                Counter = 0;
            }
        }

        public override void DrawAt(Vector3 drawLoc, bool flip = false)
        {
            base.DrawAt(drawLoc, flip);


        }
    }
}
