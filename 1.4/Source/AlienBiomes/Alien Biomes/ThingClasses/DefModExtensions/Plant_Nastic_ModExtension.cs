using System.Collections.Generic;
using Verse;

namespace AlienBiomes
{
    public class Plant_Nastic_ModExtension : DefModExtension
    {
        public float effectRadius = 2f;
        public bool emitFlecks;
        public FleckDef nasticEffectDef = null;
        public bool isTouchSensitive;
        public bool isExplosive;
        public bool isAutochorous;
    }
}
