using RimWorld;
using Verse;

namespace AlienBiomes
{
    /*
    public class MapComponent_Resurrection : MapComponent
    {
        private int ticksToResurrection = 120;
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
                //Log.Message("map.Biome is: " + map.Biome);
                // Start counting down our timer towards 0
                ticksToResurrection--;

                // Once our timer his 0, resurrect anbd apply a hediff!
                if (ticksToResurrection == 0)
                {
                    // Check if it's the correct time of day
                    // Use a range here. Otherwise you need to get lucky and have your counter hit 0 right at a specific time of day which is very unlikely.
                    if (GenLocalDate.DayPercent(map) >= startResurrectionTime || GenLocalDate.DayPercent(map) <= stopResurrectionTime)
                    {
                        Log.Message("Starting resurrection(s) in: " + map.Biome.defName + "at time: " + GenLocalDate.DayPercent(map));
                        foreach (Pawn pawn in map.mapPawns.AllPawns)
                        {
                            ResurrectionUtility.Resurrect(pawn);
                            Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.AlcoholHigh, pawn);
                            if (!pawn.health.WouldDieAfterAddingHediff(hediff))
                            {
                                pawn.health.AddHediff(hediff);
                            }
                            if (pawn.Dead)
                            {
                                Log.Error("The pawn has died while being resurrected.");
                                ResurrectionUtility.Resurrect(pawn);
                            }
                        }
                        Log.Message("Reseting resurrection ticks field to: " + ticksToResurrection);
                        // Change back after testing
                        ticksToResurrection = 120;
                    }
                }
            }
        }

        private void ResurrectWithHediff(Pawn pawn)
        {
            ResurrectionUtility.Resurrect(pawn);
            Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.AlcoholHigh, pawn, null);

            if (!pawn.health.WouldDieAfterAddingHediff(hediff))
            {
                pawn.health.AddHediff(hediff);
            }
            if (pawn.Dead)
            {
                Log.Error("The pawn has died while being resurrected.");
                ResurrectionUtility.Resurrect(pawn);
            }
        }
    }
    */
}
