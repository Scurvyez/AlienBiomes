using Verse;

namespace AlienBiomes
{
    public class CompProperties_ChangePlantDef : CompProperties
    {
        public string defToChangeTo = "";

        public CompProperties_ChangePlantDef()
        {
            this.compClass = typeof(CompChangePlantDef);
        }
    }
}
