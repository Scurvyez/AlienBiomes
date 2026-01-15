using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace AlienBiomes
{
    [StaticConstructorOnStartup]
    public static class NasticScaleCache
    {
        private static readonly Dictionary<ThingDef, float[]> Cache = new ();

        public static float[] Get(ThingDef def, ModExt_PlantNastic ext, int maxTicks)
        {
            if (def == null || ext == null || maxTicks <= 0)
                return null;

            // one cached array per ThingDef.
            if (Cache.TryGetValue(def, out float[] array) && array != null && array.Length == maxTicks)
                return array;

            array = Build(ext, maxTicks);
            Cache[def] = array;
            return array;
        }

        private static float[] Build(ModExt_PlantNastic ext, int maxTicks)
        {
            var arr = new float[maxTicks];

            // normalize 0..1 across the full tick range so it's independent of MaxTicks.
            float denom = Mathf.Max(1f, maxTicks - 1f);

            for (int i = 0; i < maxTicks; i++)
            {
                float t = i / denom; // 0..1

                // Your original shape, but driven by per-def ext values.
                float decrease = Mathf.Lerp(-ext.scaleDeltaDecrease, 0f, t);
                float increaseT = EasingFunctions.EaseOutQuad(t);
                float increase = Mathf.Lerp(0f, ext.scaleDeltaIncrease, increaseT);

                arr[i] = decrease + increase;
            }

            return arr;
        }
    }
}