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

        private bool IsProperTerrain()
        {
            if (this.RandomAdjacentCell8Way().IsValid == true
                && this.RandomAdjacentCell8Way().Filled(this.Map) == false
                && ter.defName == "SZ_RadiantWaterShallow")
            {
                return true;
            }
            return false;
        }

        public override void TickLong()
        {
            base.TickLong();

            bool iPT = IsProperTerrain();
            Plant_Mobile_ModExtension mobileExt = def.GetModExtension<Plant_Mobile_ModExtension>();

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
                    Position = availCells.RandomElement().Key;
                    Log.Message(this.def + "<color=green> at </color>" + this.Position + "<color=green> moved to </color>" + availCells.RandomElement().Key);
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
