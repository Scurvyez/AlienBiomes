using System.Collections.Generic;
using JetBrains.Annotations;
using Verse;

namespace AlienBiomes
{
    [UsedImplicitly]
    public class Biome_Rocks_ModExt : DefModExtension
    {
        public List<ThingDef> allowedRockTypes;
        public List<ThingDef> disallowedRockTypes;
    }
}