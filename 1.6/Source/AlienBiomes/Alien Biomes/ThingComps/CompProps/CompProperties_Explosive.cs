using JetBrains.Annotations;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    [UsedImplicitly]
    public class CompProperties_Explosive : CompProperties
    {
        public float triggerRadius = 2f;
        public Color triggerRadiusColor = Color.white;
        public float triggerGrowthThreshold = 0f;
        public int triggerReleaseCooldown = 2000;
        public DamageDef triggeredDamageDef = null;
        public IntRange triggeredDamageAmount = new (1, 2);
        
        public CompProperties_Explosive() => compClass = typeof(Comp_Explosive);
    }
}