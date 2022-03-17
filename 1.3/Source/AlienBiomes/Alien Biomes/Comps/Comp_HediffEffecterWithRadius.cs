using Verse;
using Verse.Sound;
using RimWorld;
using UnityEngine;

namespace AlienBiomes
{
    public class Comp_HediffEffectorWithRadius : ThingComp
    {
        public CompProperties_HediffEffectorWithRadius Props
        {
            get
            {
                return (CompProperties_HediffEffectorWithRadius)props;
            }
        }
        
        public int TickCounter = 0;

        /// <summary>
        /// Applies a hediff within a zone.
        /// Sets initial hediff severity as well.
        /// </summary>
        public override void CompTickLong()
        {
            base.CompTickLong();
            SoundDef soundDefUsed = Props.soundOnRelease;

            TickCounter++;
            if (TickCounter > Props.tickInterval)
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
                            if (Props.appliedHediff != null)
                            {
                                FleckMaker.AttachedOverlay(parent, Props.fleckReleased, Vector3.zero, 1f, -1f);
                                // 0.01f below means the applied hediff starts at 1% severity.
                                HealthUtility.AdjustSeverity(pawn, Props.appliedHediff, 0.01f);

                                // Null check the SoundDef. Otherwise, error!
                                if (Props.soundOnRelease != null) {
                                    soundDefUsed.PlayOneShot(new TargetInfo(parent.Position, parent.Map));
                                }
                            }
                        }
                    }
                }
                TickCounter = 0;
            }
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            if (parent.Map != null)
            {
                MapThingCompsGetter mapComp = parent.Map.GetComponent<MapThingCompsGetter>();
                if (mapComp != null)
                {
                    mapComp.AddCompInstancesToMap(this);
                }
            }
        }

        public override void PostDeSpawn(Map map)
        {
            if (parent.Map != null)
            {
                MapThingCompsGetter mapComp = parent.Map.GetComponent<MapThingCompsGetter>();
                if (mapComp != null)
                {
                    mapComp.RemoveCompInstancesFromMap(this);
                }
            }
        }

        /// <summary>
        /// Draws a visual area of the comps' affected cells.
        /// </summary>
        public override void PostDrawExtraSelectionOverlays()
        {
            // Only draws the area when selected.
            // Also uses the color defined in Props to populate one in a MapComp.
            if (!parent.def.drawPlaceWorkersWhileSelected)
            {
                parent.Map.GetComponent<MapThingCompsGetter>().DoDrawing = true;
                parent.Map.GetComponent<MapThingCompsGetter>().FieldEdgesColor = Props.radiusOutlineColor;
            }
        }
    }
}
