using RimWorld;
using Verse;

namespace AlienBiomes
{
    public class CompChangePlantDef : ThingComp
    {
        public CompProperties_ChangePlantDef Props => (CompProperties_ChangePlantDef)props;

        public override void CompTick()
        {
            base.CompTick();

            if (GenLocalDate.DayPercent(this.parent.Map) >= Rand.Range(0.72f, 0.78f) || (GenLocalDate.DayPercent(this.parent.Map) <= Rand.Range(0.17f, 0.23f)))
            {
                ThingDef thingToSpawn = ThingDef.Named(Props.defToChangeTo);
                GenSpawn.Spawn(thingToSpawn, this.parent.Position, parent.Map, WipeMode.Vanish);
                this.parent.Destroy();
                //Log.Message("Turning lights on.... NOW");
            }
        }
    }
}
