using Verse;

namespace AlienBiomes
{
    public class CompProperties_SoundEmanate : CompProperties
    {
        public SoundDef triggeredSound = null;
        
        public CompProperties_SoundEmanate() => compClass = typeof(Comp_SoundEmanate);
    }
}