using Verse;
using Verse.Sound;
using RimWorld;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using UnityEngine;

namespace AlienBiomes
{
    public class Comp_HediffEffecterWithRadius : ThingComp
    {
        private CompProperties_HediffEffecterWithRadius Props
        {
            get
            {
                return (CompProperties_HediffEffecterWithRadius)props;
            }
        }

        public int tickCounter = 0;
        public List<Comp_HediffEffecterWithRadius> comps = new();

        /// <summary>
        /// Applies a hediff within a zone.
        /// Sets initial hediff severity as well.
        /// </summary>
        public override void CompTick()
        {
            base.CompTick();

            tickCounter++;
            if (tickCounter > Props.tickInterval)
            {
                // If parent & map exist...
                if (parent != null && parent.Map != null)
                {
                    // Do for every instance of a Pawn within the release readius.
                    foreach (Thing thing in GenRadial.RadialDistinctThingsAround(parent.Position, parent.Map, Props.releaseRadius, true))
                    {
                        // If thing is a pawn, is alive, and (doesn't affect humanlife OR is humanlike)...
                        if (thing is Pawn pawn && !pawn.Dead && (!Props.onlyAffectHumanlike || pawn.RaceProps.Humanlike))
                        {
                            FleckMaker.AttachedOverlay(parent, Props.fleckReleased, Vector3.zero, 1f, -1f);
                            // 0.01f below means the applied hediff starts at 1% severity.
                            HealthUtility.AdjustSeverity(pawn, Props.appliedHediff, 0.01f);
                            
                            // Null check the SoundDef. Otherwise, error!
                            if (Props.soundOnRelease != null)
                            {
                                SoundDef soundDefUsed = Props.soundOnRelease;
                                soundDefUsed.PlayOneShot(new TargetInfo(parent.Position, parent.Map));
                            }
                        }
                    }
                }
                tickCounter = 0;
            }
        }

        /// <summary>
        /// Draws a visual area of the comps' affected cells.
        /// </summary>
        public override void PostDrawExtraSelectionOverlays()
        {
            
            List<IntVec3> cells = new(GenRadial.RadialCellsAround(parent.Position, Props.releaseRadius, true));
            // Only draws the area when selected.
            if (!parent.def.drawPlaceWorkersWhileSelected)
            {
                // Currently DrawRadiusRing & DrawFieldEdges do the same shit. lol
                //GenDraw.DrawRadiusRing(parent.Position, Props.releaseRadius, Props.radiusOutlineColor);
                GenDraw.DrawFieldEdges(cells, Props.radiusOutlineColor);
            }
        }
    }
}
