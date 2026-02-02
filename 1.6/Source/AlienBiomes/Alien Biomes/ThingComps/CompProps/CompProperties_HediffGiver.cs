using JetBrains.Annotations;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    [UsedImplicitly]
    public class CompProperties_HediffGiver : CompProperties
    {
        public float triggerRadius = 2f;
        public Color triggerRadiusColor = Color.white;
        public float triggerGrowthThreshold = 0f;
        public int triggerReleaseCooldown = 2000;
        public float chanceToGive = 1f;
        public HediffDef hediffToGive = null;
        
        public CompProperties_HediffGiver() => compClass = typeof(Comp_HediffGiver);
    }
}