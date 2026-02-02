using JetBrains.Annotations;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    [UsedImplicitly]
    public class ModExt_PlantVisuallyReactive : DefModExtension
    {
        public float triggerRadius = 2f;
        public Color triggerRadiusColor = Color.white;
        public int textureInstancesPerMesh = 4;
        public float minDrawScale = 0.1f;
        public float scaleDeltaDecrease = 0.08f;
        public float scaleDeltaIncrease = 0.01f;
    }
}