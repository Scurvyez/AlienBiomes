using RimWorld;
using Verse;

namespace AlienBiomes
{
    public class MapComponent_Resurrection : MapComponent
    {
        private int ticksToResurrection = 1200;
        private float startResurrectionTime = 0.75f;
        private float stopResurrectionTime = 0.25f;

        public MapComponent_Resurrection(Map map) : base(map)
        {

        }

        public override void MapComponentTick()
        {
            base.MapComponentTick();
            // Check if we are in the correct biome, if not then don't go any further
            if (map.Biome == AlienBiomes_BiomeDefOf.SZ_RadiantPlains)
            {
                // Start counting down our timer towards 0
                ticksToResurrection--;

                // Once our timer his 0, resurrect anbd apply a hediff!
                if (ticksToResurrection == 0)
                {
                    // Check if it's the correct time of day
                    // Use a range here. Otherwise you need to get lucky and have your counter hit 0 right at a specific time of day which is very unlikely.
                    if (GenLocalDate.DayPercent(map) >= startResurrectionTime || GenLocalDate.DayPercent(map) <= stopResurrectionTime)
                    {
                        Log.Message("Starting resurrection(s) in: " + map.Biome.defName);
                        //do a for loop to res everything
                        foreach (Pawn pawn in map.mapPawns.AllPawns)
                        {
                            Log.Message("Resurrecting pawn.");
                            Corpse corpse = pawn.Corpse;
                            if (corpse != null)
                            {
                                ResurrectionUtility.Resurrect(pawn); // Doesn't seem to be working? 
                                BodyPartRecord brain = pawn.health.hediffSet.GetBrain();
                                Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.AlcoholHigh, pawn, null);
                                if (!pawn.health.WouldDieAfterAddingHediff(hediff))
                                {
                                    pawn.health.AddHediff(hediff, brain, null, null);
                                }
                                // Commented out for testing
                                /*
                                if (pawn.Dead)
                                {
                                    Log.Error(pawn.Name + " has died while being resurrected.");
                                    ResurrectionUtility.Resurrect(pawn);
                                }
                                */
                            }
                        }
                        Log.Message("Reseting resurrection ticks field");
                        // Change back after testing
                        ticksToResurrection = 1200;
                    }
                }
            }
        }
    }
}
