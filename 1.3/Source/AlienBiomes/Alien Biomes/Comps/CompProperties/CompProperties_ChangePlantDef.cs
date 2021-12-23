using Verse;

namespace AlienBiomes
{
    public class CompProperties_ChangePlantDef : CompProperties
    {
        public string __defToChangeTo = ""; // New DefName to change to.
        public float? __duskTime = null; // Time window when def changes at sundown.
        public float? __dawnTime = null; // Time window when def changes at sunrise.

        public CompProperties_ChangePlantDef()
        {
            this.compClass = typeof(CompChangePlantDef);
        }
    }
}
