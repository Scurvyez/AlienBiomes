using RimWorld;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    public class Plant_Dynamic : Plant
    {
        public float? __duskTime = null;
        public float? __dawnTime = null;

        public override void TickLong()
        {
            base.TickLong();
            var dayPercent = GenLocalDate.DayPercent(Map);
            Dynamic_ModExtension plantExt = def.GetModExtension<Dynamic_ModExtension>();

            if (this.Spawned && ((dayPercent >= __duskTime && dayPercent <= 1f) || (dayPercent <= __dawnTime && dayPercent >= 0f)))
            {
                ThingDef thingToSpawn = ThingDef.Named(plantExt.__defToChangeTo);
                GenSpawn.Spawn(thingToSpawn, this.Position, this.Map, WipeMode.Vanish);
            }
        }
    }
}
