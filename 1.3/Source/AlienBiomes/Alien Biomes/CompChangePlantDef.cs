using RimWorld;
using Verse;

namespace AlienBiomes
{
    public class CompChangePlantDef : ThingComp
    {
        // This comp changes a def to another def based on the time of day (GenLocalDate.DayPercent).

        public CompProperties_ChangePlantDef Props => (CompProperties_ChangePlantDef)props;
        public override void CompTickLong()
        {
            var dayPercent = GenLocalDate.DayPercent(this.parent.Map);

            if (this.parent.Spawned && (dayPercent >= Props.__duskTime || dayPercent <= Props.__dawnTime))
            {
                ThingDef thingToSpawn = ThingDef.Named(Props.__defToChangeTo);
                GenSpawn.Spawn(thingToSpawn, this.parent.Position, this.parent.Map, WipeMode.Vanish);
                this.parent.Destroy();
            }
        }
    }
}
