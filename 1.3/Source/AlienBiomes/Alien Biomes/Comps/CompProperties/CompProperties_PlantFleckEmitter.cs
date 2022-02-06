using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    public class CompProperties_PlantFleckEmitter : CompProperties
    {
        public FleckDef fleck;
        public Vector3 offset;
        public FloatRange scale;
        public FloatRange rotationRate;
        public int burstCount = 1;
        public SoundDef soundOnEmission = null;
        public Color colorA = Color.white;
        public Color colorB = Color.white;
        public FloatRange velocityX;
        public FloatRange velocityY;
        public float fadeOutTime = 3f;

        public CompProperties_PlantFleckEmitter() => compClass = typeof(Comp_PlantFleckEmitter);

        public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
        {
            if (fleck == null)
                yield return "Comp_PlantFleckEmitter must have a fleck assigned.";
        }
    }
}