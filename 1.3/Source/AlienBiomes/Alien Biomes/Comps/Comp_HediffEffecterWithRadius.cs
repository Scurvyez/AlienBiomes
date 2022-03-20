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
            if (TickCounter < Props.tickInterval)
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
                                //Log.Message("<color=cyan>Crystallizing </color>"+ pawn.Name);
                                FleckMaker.AttachedOverlay(parent, Props.fleckReleased, Vector3.zero, 1f, -1f);

                                if (!pawn.health.hediffSet.HasHediff(AlienBiomes_HediffDefOf.SZ_Crystallize))
                                {
                                    Find.LetterStack.ReceiveLetter("SZ_LetterLabelCrystallizing".Translate(), "SZ_LetterCrystallizing".Translate(pawn), AlienBiomes_LetterDefOf.SZ_PawnCrystallizing, null, null, null);
                                    Find.TickManager.slower.SignalForceNormalSpeedShort();
                                }

                                // Adds the target hediff.
                                pawn.health.AddHediff(Props.appliedHediff);

                                // Null check the SoundDef. Otherwise... Behold, error.
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
                MapComponent_ThingCompsGetter mapComp = parent.Map.GetComponent<MapComponent_ThingCompsGetter>();
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
                MapComponent_ThingCompsGetter mapComp = parent.Map.GetComponent<MapComponent_ThingCompsGetter>();
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
                parent.Map.GetComponent<MapComponent_ThingCompsGetter>().DoDrawing = true;
                parent.Map.GetComponent<MapComponent_ThingCompsGetter>().FieldEdgesColor = Props.radiusOutlineColor;
            }
        }
    }
}
