using Verse;
using Verse.Sound;
using RimWorld;
using UnityEngine;

namespace AlienBiomes
{
    public class Comp_EffectorWithRadius : ThingComp
    {
        public CompProperties_EffectorWithRadius Props
        {
            get
            {
                return (CompProperties_EffectorWithRadius)props;
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
            SoundDef sDef = Props.soundOnRelease; // sound emitted in-game

            TickCounter++;
            if (TickCounter < Props.tickInterval)
            {
                // If parent & map exist...
                if (parent != null && parent.Map != null)
                {
                    // Do for every instance of a Pawn within the release readius.
                    foreach (Thing t in GenRadial.RadialDistinctThingsAround(parent.Position, parent.Map, Props.releaseRadius, true))
                    {
                        // If thing is a pawn, is alive, and (doesn't affect humanlife OR is humanlike)...
                        if (t is Pawn p && !p.Dead 
                            && (!Props.onlyAffectHumanlike 
                            || p.RaceProps.Humanlike))
                        {
                            if (Props.appliedHediff != null 
                                && Props.appliedHediff != AlienBiomes_HediffDefOf.SZ_Crystallize)
                            {
                                if (AlienBiomesSettings.ShowSpecialEffects == true)
                                {
                                    FleckMaker.AttachedOverlay(parent, Props.fleckReleased, Vector3.zero, 1f, -1f);
                                }

                                // Null check the SoundDef. Otherwise... Behold, error.
                                if (Props.soundOnRelease != null 
                                    && AlienBiomesSettings.AllowCompEffectSounds == true)
                                {
                                    SoundInfo sI = new TargetInfo(parent.Position, parent.Map);
                                    sI.volumeFactor = AlienBiomesSettings.PlantSoundEffectVolume;
                                    sDef.PlayOneShot(sI);
                                }
                                // Adds the target hediff.
                                p.health.AddHediff(Props.appliedHediff);
                            }

                            else if (Props.appliedHediff == AlienBiomes_HediffDefOf.SZ_Crystallize 
                                && !p.health.hediffSet.HasHediff(AlienBiomes_HediffDefOf.SZ_Crystallize))
                            {
                                if (AlienBiomesSettings.AllowCrystallizing == true)
                                {
                                    Find.LetterStack.ReceiveLetter("SZ_LetterLabelCrystallizing".Translate(), 
                                        "SZ_LetterCrystallizing".Translate(p), 
                                        AlienBiomes_LetterDefOf.SZ_PawnCrystallizing, null, null, null);
                                    Find.TickManager.slower.SignalForceNormalSpeedShort();

                                    if (AlienBiomesSettings.ShowSpecialEffects == true)
                                    {
                                        FleckMaker.AttachedOverlay(parent, Props.fleckReleased, Vector3.zero, 1f, -1f);
                                    }

                                    if (Props.soundOnRelease != null 
                                        && AlienBiomesSettings.AllowCompEffectSounds == true)
                                    {
                                        SoundInfo sI = new TargetInfo(parent.Position, parent.Map);
                                        sI.volumeFactor = AlienBiomesSettings.PlantSoundEffectVolume;
                                        sDef.PlayOneShot(sI);
                                    }
                                    p.health.AddHediff(Props.appliedHediff);
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
                MapComponent_ThingCompsGetter mC = 
                    parent.Map.GetComponent<MapComponent_ThingCompsGetter>(); // map comp
                if (mC != null)
                {
                    mC.AddCompInstancesToMap(this);
                }
            }
        }

        public override void PostDeSpawn(Map map)
        {
            if (parent.Map != null)
            {
                MapComponent_ThingCompsGetter mC = 
                    parent.Map.GetComponent<MapComponent_ThingCompsGetter>();
                if (mC != null)
                {
                    mC.RemoveCompInstancesFromMap(this);
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
