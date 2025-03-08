using JetBrains.Annotations;
using Verse;

namespace AlienBiomes
{
    [UsedImplicitly]
    public class Plant_DesertBloom_ModExt : DefModExtension
    {
        public IntRange lifeTime = new (400000, 700000);
    }
}